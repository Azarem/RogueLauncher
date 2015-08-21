using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using RogueAPI.Plugins;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace RogueLauncher.Rewrite
{
    internal class Game
    {
        public static void Rewrite(GraphManager manager)
        {
            var module = manager.Graph.Modules.First();
            var gameGraph = module.TypeGraphs.First(x => x.Name == "Game");

            var initMethod = gameGraph.Methods.First(x => x.Name == "Initialize");

            var newMethod = new MethodGraph(initMethod.Source, initMethod.DeclaringObject);


            InstructionList instr = new InstructionList(Util.GetMethodInfo(() => NewInitialize()));
            instr.Last(x => x.ILCode == ILCode.Ret).Replace(new InstructionBase() { OpCode = OpCodes.Ldarg_0 });
            instr.Add(new MethodInstruction() { OpCode = OpCodes.Call, GraphOperand = initMethod });
            instr.Add(new InstructionBase() { OpCode = OpCodes.Ret });


            newMethod.InstructionList = instr;

            initMethod.Name = "_Initialize";
            initMethod.Attributes &= ~MethodAttributes.Virtual;
            initMethod.Source = null;
        }


        private static void NewInitialize()
        {
            PluginInitializer.Initialize();
        }
    }

}
