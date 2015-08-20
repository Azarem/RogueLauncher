using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class FieldGraph : MemberGraph<FieldInfo, FieldBuilder, FieldAttributes, TypeGraph, FieldGraph>
    {
        protected Type _fieldType;
        protected object _constantValue;
        protected byte[] _initializedData;

        public Type FieldType { get { return _fieldType; } set { _fieldType = value; } }
        public object ConstantValue { get { return _constantValue; } set { _constantValue = value; } }
        public byte[] InitializedData { get { return _initializedData; } set { _initializedData = value; } }

        public FieldGraph() : this(null) { }
        public FieldGraph(FieldInfo fieldInfo, TypeGraph parentGraph = null)
            : base(fieldInfo, parentGraph)
        {
            if (fieldInfo != null)
            {
                _attributes = fieldInfo.Attributes;

                _fieldType = fieldInfo.FieldType;

                if ((fieldInfo.Attributes & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
                {
                    int size = 0;
                    if (_fieldType.Name.StartsWith("__StaticArrayInitTypeSize="))
                        size = Int32.Parse(_fieldType.Name.Substring(26));
                    else if (fieldInfo.Name.StartsWith("$ArrayType$"))
                        size = Int32.Parse(_fieldType.Name.Substring(11));
                    else if (_fieldType.IsValueType)
                        size = System.Runtime.InteropServices.Marshal.SizeOf(_fieldType);
                    else
                        throw new NotSupportedException();

                    byte[] data = new byte[size];
                    System.Runtime.CompilerServices.RuntimeHelpers.InitializeArray(data, fieldInfo.FieldHandle);
                    _initializedData = data;
                }

                if ((_attributes & FieldAttributes.Literal) == FieldAttributes.Literal)
                    _constantValue = fieldInfo.GetRawConstantValue();
            }
        }

        internal void DeclareMember(GraphManager translator)
        {
            _builder = _parentObject.Builder.DefineField(_name, translator.GetType(_fieldType), _attributes);

            translator.SetField(_sourceObject ?? _builder, _builder);
        }

        internal void DefineMember(GraphManager translator)
        {
            if ((_attributes & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
                Util.FieldSetData(_builder, _initializedData, _initializedData.Length);

            if ((_attributes & FieldAttributes.Literal) == FieldAttributes.Literal)
                _builder.SetConstant(_constantValue);

            foreach (var att in _customAttributes)
                _builder.SetCustomAttribute(att.CreateBuilder(translator));
        }
    }
}
