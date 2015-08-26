using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using RogueAPI.Plugins;
using System;
using System.Linq;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    internal class Game
    {
        public static void Process(GraphManager manager)
        {
            var module = manager.Graph.Modules.First();
            var gameGraph = module.TypeGraphs.First(x => x.Name == "Game");

            var initMethod = gameGraph.Methods.First(x => x.Name == "Initialize");

            var newMethod = new MethodGraph(Util.GetMethodInfo<Game>(x => x.NewInitialize()), initMethod.DeclaringObject);

            newMethod.Name = "Initialize";
            newMethod.Source = initMethod.Source;
            newMethod.Attributes = initMethod.Attributes;

            var instr = newMethod.InstructionList;

            var stub = Util.GetMethodInfo(() => GetProjDataStub(0));
            var initStub = Util.GetMethodInfo<Game>(x => x.OldInitialize());

            foreach (var i in instr)
            {
                if (i is MethodInstruction)
                {
                    var mi = i as MethodInstruction;
                    if (mi.Operand == stub)
                        mi.GraphOperand = module.TypeGraphs.First(x => x.Name == "SpellEV").Methods.First(x => x.Name == "_GetProjData");
                    else if (mi.Operand == initStub)
                        mi.GraphOperand = initMethod;
                }
            }

            initMethod.Name = "_Initialize";
            initMethod.Attributes &= ~MethodAttributes.Virtual;
            initMethod.Source = null;
        }

        protected virtual void OldInitialize() { }

        private static RogueAPI.Projectiles.ProjectileInstance GetProjDataStub(byte id) { return null; }

        [Obfuscation(Exclude = true)]
        protected virtual void NewInitialize()
        {
            foreach (var s in RogueAPI.Spells.SpellDefinition.All)
                s.Projectile = GetProjDataStub(s.SpellId);

            PluginInitializer.Initialize();

            OldInitialize();
        }
    }

}
