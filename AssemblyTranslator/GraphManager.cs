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
        private Dictionary<Module, ModuleBuilder> moduleCache = new Dictionary<Module, ModuleBuilder>();
        private Dictionary<Type, TypeBuilder> typeCache = new Dictionary<Type, TypeBuilder>();
        private Dictionary<MethodBase, MethodBuilder> methodCache = new Dictionary<MethodBase, MethodBuilder>();
        private Dictionary<FieldInfo, FieldBuilder> fieldCache = new Dictionary<FieldInfo, FieldBuilder>();
        private Dictionary<PropertyInfo, PropertyBuilder> propertyCache = new Dictionary<PropertyInfo, PropertyBuilder>();
        private Dictionary<EventInfo, EventBuilder> eventCache = new Dictionary<EventInfo, EventBuilder>();
        private Assembly sourceAssembly;
        internal ModuleBuilder CurrentModuleBuilder;
        private Assembly _callingAssembly;
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
            return builder != null ? builder.CreateType().Assembly : null;
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
            //var entryAsm = Assembly.GetExecutingAssembly();
            //ProcessRewriters(entryAsm);

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

        private void ProcessRewriters(Assembly asm)
        {
            foreach (var t in asm.GetTypes().Where(x => x.Namespace != null && x.Namespace.StartsWith("Rewrite.")))
            {
                var name = t.FullName.Substring(8);
                var graph = _graph.Modules.SelectMany(x => x.TypeGraphs.Where(y => y.Source.FullName == name)).FirstOrDefault();
                if (graph == null)
                    continue;

                t.GetMethod("Rewrite").Invoke(null, new object[] { graph });
            }
        }

        private void ProcessExtenders(Assembly asm)
        {

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
            PropertyBuilder newProp;
            if (propertyCache.TryGetValue(info, out newProp))
                return newProp;
            //if (info.DeclaringType.Assembly == sourceAssembly)
            //    return propertyCache[info];
            return info;
        }
        internal MethodBase GetMethod(MethodBase info, Type[] genericLookup = null)
        {
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

                    info = CurrentModuleBuilder.GetArrayMethod(type, info.Name, info.CallingConvention, ret, info.GetParameters().Select(x => GetType(x.ParameterType, genericLookup)).ToArray());
                }
                else if (info.DeclaringType.Assembly == sourceAssembly)
                {
                    if (info.IsGenericMethod)
                    {
                        var def = methodCache[((MethodInfo)info).GetGenericMethodDefinition()];
                        var args = def.GetGenericArguments();
                        info = def.MakeGenericMethod(info.GetGenericArguments().Select(x => GetType(x, args)).ToArray());
                    }
                    else
                        info = methodCache[info];
                }
                else if (type.IsGenericType)
                {
                    var def = type.GetGenericTypeDefinition();
                    var par = info.GetParameters();
                    if (info.IsConstructor)
                    {
                        var met = def.GetConstructors().Single(x => x.GetParameters().Length == par.Length);
                        info = TypeBuilder.GetConstructor(type, met);
                    }
                    else
                    {
                        //var range = Enumerable.Range(0, par.Length);
                        var met = (from m in def.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic)
                                   where m.Name == info.Name
                                   let p = m.GetParameters()
                                   where p.Length == par.Length
                                   //&& !range.Any(x => p[x].ParameterType.IsGenericParameter)
                                   select m).Single();
                        //var met = def.GetMethod(info.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                        info = TypeBuilder.GetMethod(type, met);
                    }
                }
                else
                    throw new InvalidOperationException();
            }
            else if (info.IsGenericMethod)
            {
                var def = ((MethodInfo)info).GetGenericMethodDefinition();
                var args = def.GetGenericArguments();
                info = def.MakeGenericMethod(info.GetGenericArguments().Select(x => GetType(x, args)).ToArray());
            }

            return info;
        }
        internal FieldInfo GetField(FieldInfo info)
        {
            FieldBuilder newField;
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

            return info;
        }

        internal void SetType(Type source, TypeBuilder builder) { typeCache[source] = builder; }
        internal void SetProperty(PropertyInfo source, PropertyBuilder builder) { propertyCache[source] = builder; }
        internal void SetMethod(MethodBase source, MethodBuilder builder) { methodCache[source] = builder; }
        internal void SetEvent(EventInfo source, EventBuilder builder) { eventCache[source] = builder; }
        internal void SetField(FieldInfo source, FieldBuilder builder) { fieldCache[source] = builder; }
        internal void SetModule(Module source, ModuleBuilder builder) { moduleCache[source] = builder; }

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


    }
}
