using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class PropertyGraph : MemberGraph<PropertyInfo, PropertyBuilder, PropertyAttributes, TypeGraph, PropertyGraph>
    {
        protected MethodGraph _getAccessor;
        protected MethodGraph _setAccessor;
        protected Type _propertyType;
        protected object _defaultValue;
        //protected List<Type> _parameterTypes = new List<Type>();

        public MethodGraph GetAccessor { get { return _getAccessor; } set { _getAccessor = value; } }
        public MethodGraph SetAccessor { get { return _setAccessor; } set { _setAccessor = value; } }
        public Type PropertyType { get { return _propertyType; } set { _propertyType = value; } }
        public object DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }
        //public List<Type> ParameterTypes { get { return _parameterTypes; } }

        public PropertyGraph() : this(null) { }
        public PropertyGraph(PropertyInfo propertyInfo, TypeGraph parentGraph = null)
            : base(propertyInfo, parentGraph)
        {
            if (propertyInfo != null)
            {
                _attributes = propertyInfo.Attributes;
                _propertyType = propertyInfo.PropertyType;
                //_parameterTypes.AddRange(propertyInfo.GetIndexParameters());

                var accessor = propertyInfo.GetGetMethod(true);
                if (accessor != null)
                    _getAccessor = parentGraph.Methods.Single(x => x.Source == accessor);

                accessor = propertyInfo.GetSetMethod(true);
                if (accessor != null)
                    _setAccessor = parentGraph.Methods.Single(x => x.Source == accessor);

                if ((_attributes & PropertyAttributes.HasDefault) == PropertyAttributes.HasDefault)
                    _defaultValue = propertyInfo.GetConstantValue();

                //Todo: other accessors
            }
        }


        internal void DeclareMember(GraphManager translator)
        {
            _builder = _parentObject.Builder.DefineProperty(_name, _attributes, translator.GetType(_propertyType), null);

            translator.SetProperty(_sourceObject ?? _builder, _builder);
        }

        internal void DefineMember(GraphManager translator)
        {
            if (_getAccessor != null)
                _builder.SetGetMethod(_getAccessor.Builder);

            if (_setAccessor != null)
                _builder.SetSetMethod(_setAccessor.Builder);

            if ((_attributes & PropertyAttributes.HasDefault) == PropertyAttributes.HasDefault)
                _builder.SetConstant(_defaultValue);

            foreach (var att in _customAttributes)
                _builder.SetCustomAttribute(att.CreateBuilder(translator));
        }
    }
}
