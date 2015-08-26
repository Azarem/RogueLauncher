using System;
using System.Linq;
using AssemblyTranslator.IL;
using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    public class Program
    {
        public static void Process(GraphManager manager)
        {
            var graph = manager.Graph.Modules.SelectMany(x => x.TypeGraphs.Where(y => y.FullName == "RogueCastle.Program")).Single();
            var main = graph.Methods.Single(x => x.Name == "Main");

            //Pull out Steam app ID
            var instr = main.InstructionList;
            int ix = 0;
            int count = instr.Count;
            while(ix < count)
            {
                var i = instr[ix] as MethodInstruction;
                if(i != null && i.Operand.DeclaringType.Name == "SWManager" && i.Operand.Name == "init")
                {
                    RogueLauncher.Program.SteamAppId = (int)instr[ix - 1].RawOperand;
                    PInvoke.SetEnvironmentVariable("SteamAppId", RogueLauncher.Program.SteamAppId.ToString());
                    break;
                }
                ix++;
            }

            //main.InstructionList[5].OpCode = OpCodes.Ldc_I4_1;

            //var inst = main.InstructionList.First(x => x.OpCode == OpCodes.Ldsfld && x is FieldInstruction && ((FieldInstruction)x).Operand.Name == "CREATE_RETAIL_VERSION");
            //inst.Replace(new InstructionBase() { OpCode = OpCodes.Ldc_I4_0 });
        }
    }
}
