using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class AssemblyGraph : ObjectGraph<Assembly, AssemblyBuilder>, IParentObject<ModuleGraph>
    {
        protected string _fileName;
        protected GraphList<ModuleGraph, AssemblyGraph> _modules;
        protected AssemblyName _assemblyName = new AssemblyName();

        public GraphList<ModuleGraph, AssemblyGraph> Modules { get { return _modules; } }
        public AssemblyName AssemblyName { get { return _assemblyName; } set { _assemblyName = value; } }
        public string FileName { get { return _fileName; } }


        public AssemblyGraph() : this(null) { }
        public AssemblyGraph(Assembly asm)
            : base(asm)
        {
            _modules = new GraphList<ModuleGraph, AssemblyGraph>(this);
            if (asm != null)
            {
                SetAttributeData(asm.GetCustomAttributesData());

                _assemblyName = asm.GetName();

                foreach (var m in _sourceObject.GetModules(true))
                    new ModuleGraph(m, this);
            }
        }

        public AssemblyBuilder Rebuild(GraphManager translator, string fileName, bool debug = false)
        {
            //Create assembly / modules / types
            //Create members
            //Define all

            _fileName = fileName;

            CreateBuilders(translator, debug);
            DefineMembers(translator);
            DefineCode(translator);
            CreateTypes();

            _builder.SetEntryPoint((MethodInfo)translator.GetMethod(_sourceObject.EntryPoint), PEFileKinds.WindowApplication);

            //TranslateTypes(translator);

            return _builder;
        }

        internal void CreateBuilders(GraphManager translator, bool debug = false)
        {
            _builder = AssemblyBuilder.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndSave);

            foreach (var m in _modules)
                m.DeclareTypes(translator);

            foreach (var m in _modules)
                m.DeclareMembers(translator);

        }

        private void DefineMembers(GraphManager translator)
        {
            foreach (var m in _modules)
                m.DefineMembers(translator);

            foreach (var a in _customAttributes)
                _builder.SetCustomAttribute(a.CreateBuilder(translator));
        }

        private void DefineCode(GraphManager translator)
        {
            foreach (var m in _modules)
                m.DefineCode(translator);
        }

        private void CreateTypes()
        {
            foreach (var m in _modules)
                m.CreateTypes();
        }


        //internal override void TranslateTypes(AssemblyTranslator translator)
        //{
        //}

        IList<ModuleGraph> IParentObject<ModuleGraph>.ChildList { get { return _modules; } }

        public override string ToString()
        {
            return _assemblyName.ToString();
        }
    }
}
