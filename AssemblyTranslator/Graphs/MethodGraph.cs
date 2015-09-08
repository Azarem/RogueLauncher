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
        internal List<Type> _genericArguments = new List<Type>();
        internal GraphList<ParameterGraph, MethodGraph> _parameters;
        internal InstructionList _instructionList;
        internal CallingConventions _callingConvention;

        public InstructionList InstructionList { get { return _instructionList; } set { _instructionList = value; } }
        public CallingConventions CallingConvention { get { return _callingConvention; } set { _callingConvention = value; } }
        public GraphList<ParameterGraph, MethodGraph> Parameters { get { return _parameters; } }

        public MethodGraph() : this((MethodBase)null) { }
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
        public MethodGraph(Delegate expression, TypeGraph parentGraph = null) : this(expression.Method, parentGraph) { }
        public MethodGraph(System.Linq.Expressions.LambdaExpression expression, TypeGraph parentGraph = null) : this(expression.Compile().Method, parentGraph) { }

        public MethodGraph SwitchImpl(Delegate expression, string newName = null, string oldName = null) { return SwitchImpl(expression.Method, newName, oldName); }
        public MethodGraph SwitchImpl(System.Linq.Expressions.LambdaExpression expression, string newName = null, string oldName = null) { return SwitchImpl(expression.Compile().Method, newName, oldName); }
        public MethodGraph SwitchImpl(MethodBase method, string newName = null, string oldName = null)
        {
            var newMethod = new MethodGraph(method, this.DeclaringObject)
            {
                Name = newName ?? this.Name,
                Source = this.Source,
                Attributes = this.Attributes
            };

            if (oldName != null)
                this.Name = oldName;

            this.Attributes &= ~MethodAttributes.Virtual;
            this.Source = null;

            return newMethod;
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
            //try
            //{
                _instructionList.TranslateTypes(translator, generics);
                _instructionList.Rebuild(_builder);
            //}
            //catch (Exception x)
            //{
            //    throw x;
            //}
        }



        IList<ParameterGraph> IParentObject<ParameterGraph>.ChildList { get { return _parameters; } }
    }
}
