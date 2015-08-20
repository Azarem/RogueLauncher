using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class ModuleGraph : ChildGraph<Module, ModuleBuilder, AssemblyGraph, ModuleGraph>, IParentObject<TypeGraph>
    {
        protected GraphList<TypeGraph, ModuleGraph> _typeGraphs;
        protected string _name;

        public GraphList<TypeGraph, ModuleGraph> TypeGraphs { get { return _typeGraphs; } }

        public ModuleGraph() : this(null) { }
        public ModuleGraph(Module module, AssemblyGraph parent = null)
            : base(module, parent)
        {
            _typeGraphs = new GraphList<TypeGraph, ModuleGraph>(this);
            if (module != null)
            {
                SetAttributeData(module.GetCustomAttributesData());

                _name = module.Name;

                foreach (var t in module.GetTypes())
                    if (t.DeclaringType == null)
                        new TypeGraph(t, null, this);
            }
        }

        internal void DeclareTypes(GraphManager translator, bool debug = false)
        {
            _builder = _parentObject.Builder.DefineDynamicModule(_name, _parentObject.FileName, debug);

            translator.SetModule(_sourceObject ?? _builder, _builder);
            translator.CurrentModuleBuilder = _builder;

            foreach (var t in _typeGraphs)
                t.DeclareType(translator);
        }

        internal void DeclareMembers(GraphManager translator)
        {
            translator.CurrentModuleBuilder = _builder;
            foreach (var t in _typeGraphs)
                t.DeclareMembers(translator);
        }

        internal void DefineMembers(GraphManager translator)
        {
            translator.CurrentModuleBuilder = _builder;
            foreach (var t in _typeGraphs)
                t.DefineMembers(translator);

            foreach (var att in _customAttributes)
                _builder.SetCustomAttribute(att.CreateBuilder(translator));
        }

        internal void DefineCode(GraphManager translator)
        {
            translator.CurrentModuleBuilder = _builder;
            foreach (var t in _typeGraphs)
                t.DefineCode(translator);
        }
        
        internal void CreateTypes()
        {
            foreach (var t in _typeGraphs)
                t.CreateType();
        }

        IList<TypeGraph> IParentObject<TypeGraph>.ChildList { get { return _typeGraphs; } }

        public override string ToString() { return _name; }
    }
}
