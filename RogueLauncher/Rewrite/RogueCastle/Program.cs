using System;
using System.Linq;
using System.Reflection.Emit;
using AssemblyTranslator.IL;
using AssemblyTranslator.Graphs;

namespace RogueLauncher.Rewrite.RogueCastle
{
    public class Program
    {
        public static void Rewrite(TypeGraph graph)
        {
            var main = graph.Methods.Single(x => x.Name == "Main");

            var inst = main.InstructionList.First(x => x.OpCode == OpCodes.Ldsfld && x is FieldInstruction && ((FieldInstruction)x).Operand.Name == "CREATE_RETAIL_VERSION");
            inst.Replace(new InstructionBase() { OpCode = OpCodes.Ldc_I4_0 });
        }
    }
}
