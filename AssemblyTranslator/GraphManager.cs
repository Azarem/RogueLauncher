using AssemblyTranslator.Graphs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator
{
    public class GraphManager
    {
        private Dictionary<Module, Module> moduleCache = new Dictionary<Module, Module>();
        private Dictionary<Type, Type> typeCache = new Dictionary<Type, Type>();
        private Dictionary<MethodBase, MethodBase> methodCache = new Dictionary<MethodBase, MethodBase>();
        private Dictionary<FieldInfo, FieldInfo> fieldCache = new Dictionary<FieldInfo, FieldInfo>();
        private Dictionary<PropertyInfo, PropertyInfo> propertyCache = new Dictionary<PropertyInfo, PropertyInfo>();
        private Dictionary<EventInfo, EventInfo> eventCache = new Dictionary<EventInfo, EventInfo>();
        //private Dictionary<Type, Type> typeReplacements = new Dictionary<Type, Type>();
        private Assembly sourceAssembly;
        internal ModuleBuilder CurrentModuleBuilder;
        private Assembly _callingAssembly;
        private Assembly rewriteAssembly;
        //private Module currentModule;
        //private Assembly asm;

        public AssemblyGraph Graph { get { return _graph; } }

        private bool _created;
        private AssemblyGraph _graph;

        public GraphManager(string path)
        {
            //Create graphs from source assembly
            sourceAssembly = Assembly.LoadFrom(path);
            _graph = new AssemblyGraph(sourceAssembly);
        }


        public Assembly TypeResolve(object sender, ResolveEventArgs e)
        {
            var builder = typeCache.Values.FirstOrDefault(z => z.Name == e.Name);
            if (builder is TypeBuilder)
                return ((TypeBuilder)builder).CreateType().Assembly;
            return builder != null ? builder.Assembly : null;
        }

        public void CreateAssembly(string newName, bool debug = false)
        {
            if (_created)
                return;

            //Bind type resolve handler
            AppDomain.CurrentDomain.TypeResolve += TypeResolve;

            //Set new assembly name
            _graph.AssemblyName.Name = newName;

            //Process rewriters
            rewriteAssembly = Assembly.GetCallingAssembly();

            ProcessReplacements();

            //Attach extenders
            //ProcessExtenders(entryAsm);

            //Rebuild assembly using the new name + extension
            _graph.Rebuild(this, newName + Path.GetExtension(sourceAssembly.Location), debug);

            //Set created flag
            _created = true;

            //Unbind type resolve handler
            AppDomain.CurrentDomain.TypeResolve -= TypeResolve;
        }

        public string Save(PortableExecutableKinds peKind, ImageFileMachine machine)
        {
            if (!_created)
                throw new InvalidOperationException("The Assembly has not been created.");

            var path = _graph.FileName;

            _graph.Builder.Save(path, peKind, machine);

            return path;
        }

        public byte[] SaveAndGetBytes(PortableExecutableKinds peKind, ImageFileMachine machine, bool delete = false)
        {
            if (!_created)
                throw new InvalidOperationException("The Assembly has not been created.");

            byte[] outBytes = null;

            //string oldDir = null;
            //if (delete)
            //{
            //    oldDir = Environment.CurrentDirectory;
            //    Environment.CurrentDirectory = Path.GetTempPath();
            //}

            string path = Save(peKind, machine);
            try
            {
                using (var file = File.OpenRead(path))
                {
                    outBytes = new byte[file.Length];
                    file.Read(outBytes, 0, outBytes.Length);
                }
            }
            finally
            {
                if (delete)
                {
                    if (File.Exists(path))
                        File.Delete(path);

                    path = _graph.AssemblyName.Name + ".pdb";
                    if (File.Exists(path))
                        File.Delete(path);

                    //Environment.CurrentDirectory = oldDir;
                }
            }

            return outBytes;
        }

        internal void ProcessReplacements()
        {
            foreach (var t in rewriteAssembly.GetTypes())
            {
                var repl = t.GetCustomAttribute<RewriteAttribute>(false);
                TypeGraph typeTarget = null;
                if (repl != null)
                {
                    typeTarget = (from x in Graph.Modules
                                  from y in x.TypeGraphs
                                  where y.FullName == repl.typeName
                                  select y).Single();

                    if (repl.action == RewriteAction.Replace)
                        typeTarget._replacementType = t;
                }

                foreach (var m in t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    var mRep = m.GetCustomAttribute<RewriteAttribute>();
                    if (mRep == null || mRep.action == RewriteAction.None)
                        continue;

                    var mTypeTarget = mRep.typeName == null || (repl != null && repl.typeName == mRep.typeName) ? typeTarget : (from x in Graph.Modules
                                                                                                                                from y in x.TypeGraphs
                                                                                                                                where y.FullName == mRep.typeName
                                                                                                                                select y).Single();

                    var name = mRep.targetName ?? m.Name;

                    if (m is MethodBase)
                    {
                        var target = mTypeTarget.Methods.FirstOrDefault(x => x.Name == name);
                        MethodGraph newTarget;

                        if (mRep.action == RewriteAction.Add)
                            newTarget = new MethodGraph((MethodBase)m, mTypeTarget);
                        else
                        {
                            newTarget = target.SwitchImpl((MethodBase)m, mRep.newName, mRep.oldName);
                            if (mRep.action == RewriteAction.Replace)
                                target.DeclaringObject = null;
                            else if (mRep.action == RewriteAction.Swap && newTarget.Name == target.Name)
                                target.Name += "_Orig";
                        }

                        if (mRep.contentHandler != null)
                        {
                            var handler = t.GetMethod(mRep.contentHandler);
                            handler.Invoke(null, new object[] { target, newTarget });
                        }
                    }
                    else if (m is PropertyInfo)
                    {
                        var p = m as PropertyInfo;
                        var target = mTypeTarget.Properties.FirstOrDefault(x => x.Name == name);
                        PropertyGraph newTarget;

                        if (mRep.action == RewriteAction.Add)
                        {
                            newTarget = new PropertyGraph(p, mTypeTarget);
                            if (newTarget._getAccessor != null)
                                new MethodGraph(newTarget._getAccessor, mTypeTarget);
                            if (newTarget._setAccessor != null)
                                new MethodGraph(newTarget._setAccessor, mTypeTarget);
                        }
                    }
                }
            }

        }

        internal void ProcessTypeRewriters()
        {
            foreach (var t in rewriteAssembly.GetTypes())
            {
                var repl = t.GetCustomAttribute<RewriteAttribute>(false);
                TypeGraph typeTarget = null;
                if (repl != null)
                {
                    typeTarget = (from x in Graph.Modules
                                  from y in x.TypeGraphs
                                  where y.FullName == repl.typeName
                                  select y).Single();

                    typeCache[t] = typeTarget.Builder;
                }
            }
        }

        internal void ProcessRewriters()
        {
            foreach (var t in rewriteAssembly.GetTypes())
            {
                var repl = t.GetCustomAttribute<RewriteAttribute>(false);
                TypeGraph typeTarget = null;
                if (repl != null)
                {
                    typeTarget = (from x in Graph.Modules
                                  from y in x.TypeGraphs
                                  where y.FullName == repl.typeName
                                  select y).Single();

                    //typeCache[t] = typeTarget.Builder;
                }

                foreach (var m in t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    var stub = m.GetCustomAttribute<RewriteAttribute>(false);
                    if (stub == null)
                        continue;

                    var mTypeTarget = stub.typeName == null ? typeTarget : (from x in Graph.Modules
                                                                            from y in x.TypeGraphs
                                                                            where y.FullName == stub.typeName
                                                                            select y).Single();

                    var name = (stub.stubAction == StubAction.UseOld ? (stub.oldName ?? stub.targetName) : (stub.newName ?? stub.targetName)) ?? m.Name;

                    if (stub.action == RewriteAction.Swap && stub.oldName == stub.newName)
                        name += "_Orig";

                    if (m is MethodBase)
                        methodCache[(MethodBase)m] = mTypeTarget.Methods.Single(x => x.Name == name).Builder;
                    else if (m is FieldInfo)
                        fieldCache[(FieldInfo)m] = mTypeTarget.Fields.Single(x => x.Name == name).Builder;
                    else if (m is PropertyInfo)
                    {
                        var p = m as PropertyInfo;
                        var target = mTypeTarget.Properties.Single(x => x.Name == name);

                        SetMethod(p.GetGetMethod(true), GetMethod(target.GetAccessor));
                        SetMethod(p.GetSetMethod(true), GetMethod(target.SetAccessor));
                        propertyCache[p] = target.Builder;
                    }
                }
            }
        }



        internal ConstructorInfo GetConstructor(MethodBase info)
        {
            return (ConstructorInfo)GetMethod(info);
            //if (info.DeclaringType.Assembly == sourceAssembly)
            //    return (ConstructorInfo)GetMethod(info);
            //return (ConstructorInfo)info;
        }

        internal PropertyInfo GetProperty(PropertyInfo info)
        {
            PropertyInfo newProp;
            if (propertyCache.TryGetValue(info, out newProp))
                return newProp;
            //if (info.DeclaringType.Assembly == sourceAssembly)
            //    return propertyCache[info];
            return info;
        }
        internal MethodBase GetMethod(MethodBase info, Type[] genericLookup = null)
        {
            if (info == null)
                return null;

            var type = GetType(info.DeclaringType, genericLookup);
            if (type != info.DeclaringType)
            {
                if (type.IsArray)
                {
                    Type ret = null;

                    if (!info.IsConstructor)
                    {
                        var m = info as MethodInfo;
                        ret = GetType(m.ReturnType, genericLookup);
                    }

                    return CurrentModuleBuilder.GetArrayMethod(type, info.Name, info.CallingConvention, ret, info.GetParameters().Select(x => GetType(x.ParameterType, genericLookup)).ToArray());
                }

                if (info.DeclaringType.Assembly == sourceAssembly)
                {
                    if (info.IsGenericMethod)
                    {
                        var def = (MethodBuilder)methodCache[((MethodInfo)info).GetGenericMethodDefinition()];
                        var args = def.GetGenericArguments();
                        return def.MakeGenericMethod(info.GetGenericArguments().Select(x => GetType(x, args)).ToArray());
                    }
                    else
                        return methodCache[info];
                }

                if (type.IsGenericType)
                {
                    var def = (type is TypeBuilder || type.GetType().Name == "TypeBuilderInstantiation") ? type.GetGenericTypeDefinition() : type;
                    var par = info.GetParameters();
                    if (info.IsConstructor)
                    {
                        info = def.GetConstructors().Single(x => x.GetParameters().Length == par.Length);
                        if (def != type)
                            info = TypeBuilder.GetConstructor(type, (ConstructorInfo)info);
                    }
                    else
                    {
                        //var range = Enumerable.Range(0, par.Length);
                        info = (from m in def.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic)
                                where m.Name == info.Name
                                let p = m.GetParameters()
                                where p.Length == par.Length
                                //&& !range.Any(x => p[x].ParameterType.IsGenericParameter)
                                select m).Single();
                        //var met = def.GetMethod(info.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                        if (def != type)
                            info = TypeBuilder.GetMethod(type, (MethodInfo)info);
                    }
                    return info;
                }

                //throw new InvalidOperationException();

            }

            if (info.IsGenericMethod)
            {
                var def = ((MethodInfo)info).GetGenericMethodDefinition();
                var args = def.GetGenericArguments();
                return def.MakeGenericMethod(info.GetGenericArguments().Select(x => GetType(x, args)).ToArray());
            }

            MethodBase newInfo;
            if (methodCache.TryGetValue(info, out newInfo))
                return newInfo;

            return info;
        }
        internal FieldInfo GetField(FieldInfo info)
        {
            FieldInfo newField;
            if (fieldCache.TryGetValue(info, out newField))
                return newField;
            //if (info.DeclaringType.Assembly == sourceAssembly)
            //    return fieldCache[info];
            return info;
        }
        internal Type GetType(Type info, Type[] genericLookup = null)
        {
            if (info == null)
                return null;

            if (info.IsArray)
            {
                var rank = info.GetArrayRank();

                info = GetType(info.GetElementType(), genericLookup);
                //if (create)
                //    info = ((TypeBuilder)info).CreateType();
                //if (info is TypeBuilder)
                //    info = ((TypeBuilder)info).CreateType();

                return rank == 1 ? info.MakeArrayType() : info.MakeArrayType(rank);
            }

            if (info.Assembly == sourceAssembly)
            {

                if (info.IsGenericParameter)
                    return genericLookup.Single(x => x.Name == info.Name);

                if (info.IsPointer)
                    return GetType(info.GetElementType(), genericLookup).MakePointerType();

                if (info.IsByRef)
                    return GetType(info.GetElementType(), genericLookup).MakeByRefType();

                info = typeCache[info];
                //if (create)
                //    info = ((TypeBuilder)info).CreateType();

                return info;
            }

            if (info.IsGenericType)
            {
                var genericArguments = info.GetGenericArguments();
                for (int i = 0; i < genericArguments.Length; i++)
                    genericArguments[i] = GetType(genericArguments[i], genericLookup);
                return info.GetGenericTypeDefinition().MakeGenericType(genericArguments);
            }

            Type newType;
            if (typeCache.TryGetValue(info, out newType))
                info = newType;

            return info;
        }

        internal void SetType(Type source, Type builder) { if (source != null && builder != null) typeCache[source] = builder; }
        internal void SetProperty(PropertyInfo source, PropertyInfo builder) { if (source != null && builder != null) propertyCache[source] = builder; }
        internal void SetMethod(MethodBase source, MethodBase builder) { if (source != null && builder != null) methodCache[source] = builder; }
        internal void SetEvent(EventInfo source, EventInfo builder) { if (source != null && builder != null) eventCache[source] = builder; }
        internal void SetField(FieldInfo source, FieldInfo builder) { if (source != null && builder != null) fieldCache[source] = builder; }
        internal void SetModule(Module source, Module builder) { if (source != null && builder != null) moduleCache[source] = builder; }

        internal T GetMember<T>(object member, Type[] genericLookup = null) where T : class
        {
            if (member is MethodBase)
                return GetMethod(member as MethodBase, genericLookup) as T;
            if (member is FieldInfo)
                return GetField(member as FieldInfo) as T;
            if (member is Type)
                return GetType(member as Type, genericLookup) as T;
            if (member is PropertyInfo)
                return GetProperty(member as PropertyInfo) as T;
            if (typeof(T) != typeof(string))
            {

            }
            return member as T;
        }

        //private EventBuilder GetEvent(EventInfo info)
        //{
        //    if (info.DeclaringType.Assembly == sourceAssembly)
        //        return eventCache[info];
        //    return info;
        //}

        public void ReplaceType(string typeName, Type newType)
        {
            var graph = _graph._modules.SelectMany(x => x._typeGraphs.Where(y => y._fullName == typeName)).First();
            graph._replacementType = newType;
        }


    }
}
