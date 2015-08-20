using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class ParameterGraph : ChildGraph<ParameterInfo, ParameterBuilder, MethodGraph, ParameterGraph>
    {
        protected ParameterAttributes _attributes;
        protected Type _parameterType;
        protected string _name;
        protected List<Type> _requiredCustomModifiers = new List<Type>();
        protected List<Type> _optionalCustomModifiers = new List<Type>();
        protected object _defaultValue;
        protected int _index;

        public ParameterAttributes Attributes { get { return _attributes; } set { _attributes = value; } }
        public Type ParameterType { get { return _parameterType; } set { _parameterType = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public List<Type> RequiredCustomModifiers { get { return _requiredCustomModifiers; } }
        public List<Type> OptionalCustomModifiers { get { return _optionalCustomModifiers; } }
        public object DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }
        public int Index { get { return _index; } set { _index = value; } }

        public ParameterGraph() : this(null) { }

        public ParameterGraph(ParameterInfo sourceInfo, MethodGraph parentMethod = null)
            : base(sourceInfo, parentMethod)
        {
            if (sourceInfo != null)
            {
                SetAttributeData(sourceInfo.GetCustomAttributesData());

                _attributes = sourceInfo.Attributes;
                _parameterType = sourceInfo.ParameterType;
                _name = sourceInfo.Name;
                _requiredCustomModifiers.AddRange(sourceInfo.GetRequiredCustomModifiers());
                _optionalCustomModifiers.AddRange(sourceInfo.GetOptionalCustomModifiers());

                if ((sourceInfo.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault)
                    _defaultValue = sourceInfo.DefaultValue;
            }
        }

        internal void DeclareMember(GraphManager translator)
        {
            if (_parameterType == null)
                return;

            _builder = _parentObject.Builder.DefineParameter(_index, _attributes, _name);
        }

        internal void DefineMember(GraphManager translator)
        {
            if ((_attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault)
                _builder.SetConstant(_defaultValue);

            foreach (var att in _customAttributes)
                _builder.SetCustomAttribute(att.CreateBuilder(translator));
        }
    }
}
