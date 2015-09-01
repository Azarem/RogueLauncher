using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using RogueAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    public class ClassSystem
    {
        public static void Process(GraphManager manager)
        {
            var module = manager.Graph.Modules.First();

            var classType = module.TypeGraphs.First(x => x.Name == "ClassType");
            PullClassType(classType);

            var playerObj = module.TypeGraphs.First(x => x.Name == "PlayerObj");
            PullPlayerObj(playerObj);

            var cls = ClassDefinition.All;
        }

        public static ClassDefinition GetClass(byte id)
        {
            //if (id == 0)
            //    return ClassDefinition.None;

            var def = ClassDefinition.GetById(id);

            if (def == null)
                def = ClassDefinition.Register(id);

            return def;
        }

        private static void PullPlayerObj(TypeGraph playerObj)
        {
            var method = playerObj.Methods.First(x => x.Name == "get_ClassDamageGivenMultiplier");
            //var oldMethod = new MethodGraph(method.Source, playerObj);
            //oldMethod.Name = "_" + oldMethod.Name;
            //oldMethod.Source = null;

            Util.ReadSwitches<float>(method.InstructionList, (x, y, z) => GetClass(x).PhysicalDamageMultiplier = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.PhysicalDamageMultiplier == 0)) c.PhysicalDamageMultiplier = y; });
            ChangeFieldCall(method, Util.GetMethodInfo<ClassDefinition, float>(x => x.PhysicalDamageMultiplier));

            method = playerObj.Methods.First(x => x.Name == "get_ClassDamageTakenMultiplier");
            Util.ReadSwitches<float>(method.InstructionList, (x, y, z) => GetClass(x).DamageTakenMultiplier = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.DamageTakenMultiplier == 0)) c.DamageTakenMultiplier = y; });
            ChangeFieldCall(method, Util.GetMethodInfo<ClassDefinition, float>(x => x.DamageTakenMultiplier));

            method = playerObj.Methods.First(x => x.Name == "get_ClassMagicDamageGivenMultiplier");
            Util.ReadSwitches<float>(method.InstructionList, (x, y, z) => GetClass(x).MagicDamageMultiplier = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.MagicDamageMultiplier == 0)) c.MagicDamageMultiplier = y; });
            ChangeFieldCall(method, Util.GetMethodInfo<ClassDefinition, float>(x => x.MagicDamageMultiplier));

            method = playerObj.Methods.First(x => x.Name == "get_ClassMoveSpeedMultiplier");
            Util.ReadSwitches<float>(method.InstructionList, (x, y, z) => GetClass(x).MoveSpeedMultiplier = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.MoveSpeedMultiplier == 0)) c.MoveSpeedMultiplier = y; });
            ChangeFieldCall(method, Util.GetMethodInfo<ClassDefinition, float>(x => x.MoveSpeedMultiplier));

            method = playerObj.Methods.First(x => x.Name == "get_ClassTotalHPMultiplier");
            Util.ReadSwitches<float>(method.InstructionList, (x, y, z) => GetClass(x).HealthMultiplier = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.HealthMultiplier == 0)) c.HealthMultiplier = y; });
            ChangeFieldCall(method, Util.GetMethodInfo<ClassDefinition, float>(x => x.HealthMultiplier));

            method = playerObj.Methods.First(x => x.Name == "get_ClassTotalMPMultiplier");
            Util.ReadSwitches<float>(method.InstructionList, (x, y, z) => GetClass(x).ManaMultiplier = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.ManaMultiplier == 0)) c.ManaMultiplier = y; });
            ChangeFieldCall(method, Util.GetMethodInfo<ClassDefinition, float>(x => x.ManaMultiplier));
        }

        private static void ChangeFieldCall(MethodGraph graph, MethodBase info)
        {
            var l = graph.InstructionList;
            var instr = new InstructionList();
            instr.Add(l[0]);
            instr.Add(l[1]);
            instr.Add(new MethodInstruction() { OpCode = OpCodes.Call, Operand = Util.GetMethodInfo(() => ClassDefinition.GetById(0)) });
            instr.Add(new MethodInstruction() { OpCode = OpCodes.Callvirt, Operand = info });
            instr.Add(new InstructionBase() { OpCode = OpCodes.Ret });
            graph.InstructionList = instr;
        }

        private static void PullClassType(TypeGraph classType)
        {
            foreach (var field in classType.Fields.Where(x => x.ConstantValue is byte && !x.Name.StartsWith("Total")))
                GetClass((byte)field.ConstantValue).Name = field.Name;

            var spellList = new Dictionary<byte, byte[]>();

            foreach (var m in classType.Methods)
            {
                if (m.Name == "Description")
                {
                    Util.ReadSwitches<string>(m.InstructionList, (x, y, z) => GetClass(x).Description = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.Description == null)) c.Description = y; });
                    m.InstructionList = new InstructionList(new Func<byte, string>(x => ClassDefinition.GetById(x).Description));
                }
                else if (m.Name == "ProfileCardDescription")
                {
                    Util.ReadSwitches<string>(m.InstructionList, (x, y, z) => GetClass(x).ProfileCardDescription = y, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.ProfileCardDescription == null)) c.Description = y; });
                    m.InstructionList = new InstructionList(new Func<byte, string>(x => ClassDefinition.GetById(x).ProfileCardDescription));
                }
                else if (m.Name == "ToString")
                {
                    Util.ReadSwitches<string>(m.InstructionList, (x, y, z) => { if (z)  GetClass(x).FemaleDisplayName = y; else GetClass(x).DisplayName = y; }, (x, y, z) => { foreach (var c in ClassDefinition.All.Where(b => b.DisplayName == null)) c.DisplayName = y; });
                    m.InstructionList = new InstructionList(new Func<byte, bool, string>((x, y) => ClassDefinition.GetById(x).GetDisplayName(y)));
                }
                else if (m.Name == "GetSpellList")
                {
                    Util.ReadSwitches<byte[]>(m.InstructionList, (x, y, z) => { var c = GetClass(x); foreach (var b in y) c.SpellList.Add(RogueAPI.Spells.SpellDefinition.GetById(b)); }, (x, y, z) => { });
                    m.InstructionList = new InstructionList(new Func<byte, byte[]>(x => ClassDefinition.GetById(x).SpellByteArray));
                }

            }
        }

    }
}
