using AssemblyTranslator.IL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class MethodGraph : MemberGraph<MethodBase, MethodBuilder, MethodAttributes, TypeGraph, MethodGraph>, IParentObject<ParameterGraph>
    {
        private List<Type> _genericArguments = new List<Type>();
        private GraphList<ParameterGraph, MethodGraph> _parameters;
        protected InstructionList _instructionList;
        protected CallingConventions _callingConvention;

        public InstructionList InstructionList { get { return _instructionList; } set { _instructionList = value; } }
        public CallingConventions CallingConvention { get { return _callingConvention; } set { _callingConvention = value; } }

        public MethodGraph() : this(null) { }
        public MethodGraph(MethodBase method, TypeGraph parentGraph = null)
            : base(method, parentGraph)
        {
            _parameters = new GraphList<ParameterGraph, MethodGraph>(this);

            if (method != null)
            {
                Attributes = method.Attributes;

                _callingConvention = method.CallingConvention;

                if (method.IsGenericMethod)
                    _genericArguments.AddRange(method.GetGenericArguments());

                new ParameterGraph(method is MethodInfo ? ((MethodInfo)method).ReturnParameter : null, this);

                foreach (var p in method.GetParameters())
                    new ParameterGraph(p, this);

                _instructionList = new InstructionList(_sourceObject);
            }
        }

        internal void DeclareMember(GraphManager translator)
        {
            _builder = _parentObject.Builder.DefineMethod(_name, _attributes, _callingConvention);

            translator.SetMethod(_sourceObject ?? _builder, _builder);

            GenericTypeParameterBuilder[] genericTypes = null;
            int ix;
            if (_genericArguments.Count > 0)
            {
                genericTypes = _builder.DefineGenericParameters(_genericArguments.Select(x => x.Name).ToArray());

                for (ix = 0; ix < _genericArguments.Count; ix++)
                {
                    var g = genericTypes[ix];
                    var old = _genericArguments[ix];
                    g.SetGenericParameterAttributes(old.GenericParameterAttributes);
                    g.SetInterfaceConstraints(old.GetGenericParameterConstraints().Select(x => translator.GetType(x, genericTypes)).ToArray());
                }

            }

            var ret = _parameters[0];
            var par = _parameters.Skip(1).ToArray();

            _builder.SetSignature(translator.GetType(ret.ParameterType, genericTypes),
                ret.RequiredCustomModifiers.Select(x => translator.GetType(x, genericTypes)).ToArray(),
                ret.OptionalCustomModifiers.Select(x => translator.GetType(x, genericTypes)).ToArray(),
                par.Select(x => translator.GetType(x.ParameterType, genericTypes)).ToArray(),
                par.Select(x => x.RequiredCustomModifiers.Select(y => translator.GetType(y, genericTypes)).ToArray()).ToArray(),
                par.Select(x => x.OptionalCustomModifiers.Select(y => translator.GetType(y, genericTypes)).ToArray()).ToArray());

            ix = 0;
            foreach (var p in _parameters)
            {
                p.Index = ix++;
                p.DeclareMember(translator);
            }
        }

        internal void DefineMember(GraphManager translator)
        {
            foreach (var p in _parameters)
                p.DefineMember(translator);

            foreach (var att in _customAttributes)
                _builder.SetCustomAttribute(att.CreateBuilder(translator));
        }

        internal void DefineCode(GraphManager translator)
        {
            var generics = _builder.GetGenericArguments();
            _instructionList.TranslateTypes(translator, generics);
            _instructionList.Rebuild(_builder);
        }



        IList<ParameterGraph> IParentObject<ParameterGraph>.ChildList { get { return _parameters; } }
    }
}
