﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace AssemblyTranslator.Graphs
{
    public class TypeGraph : MemberGraph<Type, TypeBuilder, TypeAttributes, TypeGraph, TypeGraph>, IChildObject<ModuleGraph>, IParentObject<MethodGraph>, IParentObject<FieldGraph>, IParentObject<EventGraph>, IParentObject<TypeGraph>, IParentObject<PropertyGraph>
    {
        private static ConstructorInfo _typeBuildConstructor = typeof(TypeBuilder).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(string), typeof(TypeAttributes), typeof(Type), typeof(Type[]), typeof(ModuleBuilder), typeof(PackingSize), typeof(int), typeof(TypeBuilder) }, null);

        internal ModuleGraph _moduleGraph;
        internal GraphList<MethodGraph, TypeGraph> _methodGraphs;
        internal GraphList<PropertyGraph, TypeGraph> _propertyGraphs;
        internal GraphList<FieldGraph, TypeGraph> _fieldGraphs;
        internal GraphList<EventGraph, TypeGraph> _eventGraphs;
        internal GraphList<TypeGraph, TypeGraph> _typeGraphs;
        internal List<Type> _interfaces = new List<Type>();
        internal TypeGraph _declaringType;
        internal PackingSize _packingSize;
        internal string _fullName;
        internal int _typeSize;
        internal Type _baseType;
        internal Type _replacementType;

        public ModuleGraph Module
        {
            get { return _moduleGraph; }
            set
            {
                if (_moduleGraph == value)
                    return;
                if (_moduleGraph != null)
                    _moduleGraph.TypeGraphs.Remove(this);
                if (value != null)
                    value.TypeGraphs.Add(this);
            }
        }
        public GraphList<MethodGraph, TypeGraph> Methods { get { return _methodGraphs; } }
        public GraphList<PropertyGraph, TypeGraph> Properties { get { return _propertyGraphs; } }
        public GraphList<FieldGraph, TypeGraph> Fields { get { return _fieldGraphs; } }
        public GraphList<EventGraph, TypeGraph> Events { get { return _eventGraphs; } }
        public GraphList<TypeGraph, TypeGraph> NestedTypes { get { return _typeGraphs; } }
        public string FullName { get { return _fullName; } set { _fullName = value; } }
        public Type BaseType { get { return _baseType; } set { _baseType = value; } }

        public List<Type> Interfaces { get { return _interfaces; } set { _interfaces = value; } }

        public TypeGraph() : this(null) { }
        public TypeGraph(Type t, TypeGraph parentType = null, ModuleGraph module = null)
            : base(t, parentType)
        {
            if (module != null)
                Module = module;

            _methodGraphs = new GraphList<MethodGraph, TypeGraph>(this);
            _propertyGraphs = new GraphList<PropertyGraph, TypeGraph>(this);
            _fieldGraphs = new GraphList<FieldGraph, TypeGraph>(this);
            _eventGraphs = new GraphList<EventGraph, TypeGraph>(this);
            _typeGraphs = new GraphList<TypeGraph, TypeGraph>(this);
            if (t != null)
            {
                Attributes = t.Attributes;

                _fullName = t.FullName;
                _baseType = t.BaseType;

                IEnumerable<Type> interfaces = t.GetInterfaces();
                if (t.BaseType != null)// && interfaces.Any())
                    interfaces = interfaces.Except(t.BaseType.GetInterfaces());

                _interfaces.AddRange(interfaces);

                if ((t.Attributes & TypeAttributes.ExplicitLayout) == TypeAttributes.ExplicitLayout && t.IsValueType)
                {
                    _packingSize = PackingSize.Size1;
                    _typeSize = System.Runtime.InteropServices.Marshal.SizeOf(t);
                }
                else
                {
                    _packingSize = PackingSize.Unspecified;
                    _typeSize = 0;
                }

                //if (t.DeclaringType != null && module != null)
                //    DeclaringObject = module.TypeGraphs.First(x => x.Source == t);

                foreach (var m in t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    if (m is MethodBase)
                        new MethodGraph(m as MethodBase, this);
                    else if (m is FieldInfo)
                        new FieldGraph(m as FieldInfo, this);
                    else if (m is PropertyInfo)
                        new PropertyGraph(m as PropertyInfo, this);
                    else if (m is EventInfo)
                        new EventGraph(m as EventInfo, this);
                    else if (m is Type)
                        new TypeGraph(m as Type, this, module);
                    else
                        throw new InvalidOperationException();
                }
            }
        }

        internal void DeclareType(GraphManager translator)
        {
            if (_replacementType == null)
            {
                TypeBuilder parent = null;
                string name = _fullName;
                if (_parentObject != null)
                {
                    name = Regex.Match(_fullName, "([^+.]+)$").Groups[1].Value;
                    parent = _parentObject._builder;
                }
                _builder = Util.DefineTypeBuilder(name, _attributes, translator.GetType(_baseType), _interfaces.Select(x => translator.GetType(x)).ToArray(), _moduleGraph.Builder, _packingSize, _typeSize, parent);
            }

            translator.SetType(_sourceObject ?? _builder, _replacementType ?? _builder);
        }

        internal void DeclareMembers(GraphManager translator)
        {
            if (_replacementType == null)
            {
                foreach (var f in _fieldGraphs)
                    f.DeclareMember(translator);

                foreach (var p in _propertyGraphs)
                    p.DeclareMember(translator);

                foreach (var e in _eventGraphs)
                    e.DeclareMember(translator);

                foreach (var m in _methodGraphs)
                    m.DeclareMember(translator);
            }
            else
            {
                var members = _replacementType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                foreach (var f in _fieldGraphs)
                    translator.SetField(f._sourceObject, members.FirstOrDefault(x => x is FieldInfo && x.Name == f.Name) as FieldInfo);

                foreach (var p in _propertyGraphs)
                    translator.SetProperty(p._sourceObject, members.FirstOrDefault(x => x is PropertyInfo && x.Name == p.Name) as PropertyInfo);

                foreach (var e in _eventGraphs)
                    translator.SetEvent(e._sourceObject, members.FirstOrDefault(x => x is EventInfo && x.Name == e.Name) as EventInfo);

                foreach (var m in _methodGraphs)
                    translator.SetMethod(m._sourceObject, members.FirstOrDefault(x =>
                    {
                        if (!(x is MethodBase) || x.Name != m.Name)
                            return false;

                        var p = ((MethodBase)x).GetParameters();

                        if (p.Length + 1 != m._parameters.Count)
                            return false;

                        return p.All(y => y.ParameterType.IsAssignableFrom(m._parameters[y.Position + 1].ParameterType));
                        //return p.All(y => m._parameters[y.Position + 1].ParameterType == y.ParameterType);
                    }) as MethodBase);
            }
        }

        internal void DefineMembers(GraphManager translator)
        {
            if (_replacementType != null)
                return;

            foreach (var f in _fieldGraphs)
                f.DefineMember(translator);

            foreach (var p in _propertyGraphs)
                p.DefineMember(translator);

            foreach (var e in _eventGraphs)
                e.DefineMember(translator);

            foreach (var m in _methodGraphs)
                m.DefineMember(translator);

            foreach (var att in _customAttributes)
                _builder.SetCustomAttribute(att.CreateBuilder(translator));
        }

        internal void DefineCode(GraphManager translator)
        {
            if (_replacementType != null)
                return;

            foreach (var m in _methodGraphs)
                m.DefineCode(translator);
        }

        internal void CreateType()
        {
            if (_replacementType != null)
                return;

            _builder.CreateType();
        }

        public void SetParentInternal(ModuleGraph parent) { _moduleGraph = parent; }

        IList<MethodGraph> IParentObject<MethodGraph>.ChildList { get { return _methodGraphs; } }
        IList<FieldGraph> IParentObject<FieldGraph>.ChildList { get { return _fieldGraphs; } }
        IList<EventGraph> IParentObject<EventGraph>.ChildList { get { return _eventGraphs; } }
        IList<TypeGraph> IParentObject<TypeGraph>.ChildList { get { return _typeGraphs; } }
        IList<PropertyGraph> IParentObject<PropertyGraph>.ChildList { get { return _propertyGraphs; } }

        public override string ToString() { return _fullName; }
    }
}
