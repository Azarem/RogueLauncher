using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class CustomAttributeGraph
    {
        protected ConstructorInfo _constructor;
        protected List<object> _constructorArguments = new List<object>();
        protected List<AttributeFieldGraph> _fieldData = new List<AttributeFieldGraph>();
        protected List<AttributePropertyGraph> _propertyData = new List<AttributePropertyGraph>();

        public ConstructorInfo Constructor { get { return _constructor; } set { _constructor = value; } }
        public List<object> ConstructorArguments { get { return _constructorArguments; } }
        public List<AttributeFieldGraph> FieldData { get { return _fieldData; } }
        public List<AttributePropertyGraph> PropertyData { get { return _propertyData; } }

        public CustomAttributeGraph() { }

        public CustomAttributeGraph(CustomAttributeData data)
        {
            _constructor = data.Constructor;
            _constructorArguments.AddRange(data.ConstructorArguments.Select(x => x.Value));
            _fieldData.AddRange(data.NamedArguments.Where(x => x.MemberInfo is FieldInfo).Select(x => new AttributeFieldGraph(x.MemberInfo as FieldInfo, x.TypedValue.Value)));
            _propertyData.AddRange(data.NamedArguments.Where(x => x.MemberInfo is PropertyInfo).Select(x => new AttributePropertyGraph(x.MemberInfo as PropertyInfo, x.TypedValue.Value)));
        }

        public CustomAttributeBuilder CreateBuilder(GraphManager translator)
        {
            return new CustomAttributeBuilder(translator.GetConstructor(_constructor),
                _constructorArguments.ToArray(),
                _propertyData.Select(x => translator.GetProperty(x.PropertyInfo)).ToArray(),
                _propertyData.Select(x => x.Value).ToArray(),
                _fieldData.Select(x => translator.GetField(x.FieldInfo)).ToArray(),
                _fieldData.Select(x => x.Value).ToArray());
        }
    }

    public class AttributeFieldGraph
    {
        protected FieldInfo _fieldInfo;
        protected object _value;

        public FieldInfo FieldInfo { get { return _fieldInfo; } set { _fieldInfo = value; } }
        public object Value { get { return _value; } set { _value = value; } }

        public AttributeFieldGraph(FieldInfo fieldInfo, object value)
        {
            _fieldInfo = fieldInfo;
            _value = value;
        }
    }

    public class AttributePropertyGraph
    {
        protected PropertyInfo _propertyInfo;
        protected object _value;

        public PropertyInfo PropertyInfo { get { return _propertyInfo; } set { _propertyInfo = value; } }
        public object Value { get { return _value; } set { _value = value; } }

        public AttributePropertyGraph(PropertyInfo propertyInfo, object value)
        {
            _propertyInfo = propertyInfo;
            _value = value;
        }
    }
}
