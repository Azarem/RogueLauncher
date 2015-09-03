using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using RogueAPI.Classes;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace RogueLauncher.Rewrite
{
    public class ClassSystem
    {
        public static void ChangeFieldCall(MethodGraph sourceGraph, MethodGraph newGraph)
        {
            sourceGraph.Attributes = MethodAttributes.Public | MethodAttributes.Static;
            sourceGraph.CallingConvention = CallingConventions.Standard;

            newGraph.Parameters[1].DeclaringObject = sourceGraph;
            newGraph.CallingConvention = CallingConventions.Standard | CallingConventions.HasThis;

            var instr = sourceGraph.InstructionList;
            instr.Locals.Clear();

            instr.RemoveAt(0);
            instr.RemoveAt(0);
            instr.RemoveAt(0);

            int ix = 0, count = instr.Count;
            while (ix < count)
            {
                var i = instr[ix++];
                if (i.ILCode == ILCode.Ldloc_0)
                    i.Replace(new ParameterInstruction() { OpCode = OpCodes.Ldarg_0 });
            }

        }


        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.PlayerObj", "get_ClassDamageGivenMultiplier", action: RewriteAction.Swap, oldName: "GetClassDamageGivenMultiplier", stubAction: StubAction.UseOld, contentHandler: "ChangeFieldCall")]
        public static float ClassDamageGivenMultiplier(byte id)
        {
            return ClassDefinition.GetById(PlayerStatsStub.get_Class()).PhysicalDamageMultiplier;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.PlayerObj", "get_ClassDamageTakenMultiplier", action: RewriteAction.Swap, oldName: "GetClassDamageTakenMultiplier", stubAction: StubAction.UseOld, contentHandler: "ChangeFieldCall")]
        public static float ClassDamageTakenMultiplier(byte id)
        {
            return ClassDefinition.GetById(PlayerStatsStub.get_Class()).DamageTakenMultiplier;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.PlayerObj", "get_ClassMagicDamageGivenMultiplier", action: RewriteAction.Swap, oldName: "GetClassMagicDamageGivenMultiplier", stubAction: StubAction.UseOld, contentHandler: "ChangeFieldCall")]
        public static float ClassMagicDamageGivenMultiplier(byte id)
        {
            return ClassDefinition.GetById(PlayerStatsStub.get_Class()).MagicDamageMultiplier;
        }
        
        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.PlayerObj", "get_ClassMoveSpeedMultiplier", action: RewriteAction.Swap, oldName: "GetClassMoveSpeedMultiplier", stubAction: StubAction.UseOld, contentHandler: "ChangeFieldCall")]
        public static float ClassMoveSpeedMultiplier(byte id)
        {
            return ClassDefinition.GetById(PlayerStatsStub.get_Class()).MoveSpeedMultiplier;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.PlayerObj", "get_ClassTotalHPMultiplier", action: RewriteAction.Swap, oldName: "GetClassTotalHPMultiplier", stubAction: StubAction.UseOld, contentHandler: "ChangeFieldCall")]
        public static float ClassTotalHPMultiplier(byte id)
        {
            return ClassDefinition.GetById(PlayerStatsStub.get_Class()).HealthMultiplier;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.PlayerObj", "get_ClassTotalMPMultiplier", action: RewriteAction.Swap, oldName: "GetClassTotalMPMultiplier", stubAction: StubAction.UseOld, contentHandler: "ChangeFieldCall")]
        public static float ClassTotalMPMultiplier(byte id)
        {
            return ClassDefinition.GetById(PlayerStatsStub.get_Class()).ManaMultiplier;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.ClassType", "Description", action: RewriteAction.Swap, newName: "_Description", stubAction: StubAction.UseOld)]
        public static string Description(byte id)
        {
            return ClassDefinition.GetById(id).Description;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.ClassType", "ProfileCardDescription", action: RewriteAction.Swap, newName: "_ProfileCardDescription", stubAction: StubAction.UseOld)]
        public static string ProfileCardDescription(byte id)
        {
            return ClassDefinition.GetById(id).ProfileCardDescription;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.ClassType", "ToString", action: RewriteAction.Swap, newName: "_ToString", stubAction: StubAction.UseOld)]
        public static string ToString(byte id, bool isFemale)
        {
            return ClassDefinition.GetById(id).GetDisplayName(isFemale);
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.ClassType", "GetSpellList", action: RewriteAction.Swap, newName: "_GetSpellList", stubAction: StubAction.UseOld)]
        public static byte[] GetSpellList(byte id)
        {
            return ClassDefinition.GetById(id).SpellByteArray;
        }

        [Rewrite("RogueCastle.Game", "PlayerStats", RewriteAction.None)]
        public static PlayerStatsStubClass PlayerStatsStub;

        public class PlayerStatsStubClass
        {
            [Rewrite("RogueCastle.PlayerStats", "get_Class", RewriteAction.None)]
            public byte get_Class() { return 0; }
        }
    }
}
