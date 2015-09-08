using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using DS2DEngine;
using RogueAPI.Classes;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.PlayerObj")]
    public class PlayerObj : PhysicsObjContainer
    {
        [Rewrite]
        public int MaxHealth { get { return 0; } }
        [Rewrite]
        public float MaxMana { get { return 0; } }
        [Rewrite]
        public int Damage { get { return 0; } }
        [Rewrite]
        public int TotalMagicDamage { get { return 0; } }
        [Rewrite]
        public float TotalArmor { get { return 0; } }
        [Rewrite]
        public int CurrentWeight { get { return 0; } }
        [Rewrite]
        public int MaxWeight { get { return 0; } }
        [Rewrite]
        public float TotalCritChance { get { return 0; } }
        [Rewrite]
        public float TotalDamageReduc { get { return 0; } }
        [Rewrite]
        public float TotalCriticalDamage { get { return 0; } }
        [Rewrite]
        public int TotalDoubleJumps { get { return 0; } }
        [Rewrite]
        public int TotalAirDashes { get { return 0; } }
        [Rewrite]
        public int TotalVampBonus { get { return 0; } }
        [Rewrite]
        public float TotalFlightTime { get { return 0; } }
        [Rewrite]
        public float ManaGain { get { return 0; } }
        [Rewrite]
        public float TotalDamageReturn { get { return 0; } }
        [Rewrite]
        public float TotalGoldBonus { get { return 0; } }
        [Rewrite]
        public float TotalMovementSpeedPercent { get { return 0; } }

        [Rewrite]
        public void UpdateEquipmentColours() { }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassDamageGivenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).PhysicalDamageMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassDamageTakenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).DamageTakenMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassMagicDamageGivenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).MagicDamageMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassMoveSpeedMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).MoveSpeedMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassTotalHPMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).HealthMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassTotalMPMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).ManaMultiplier; }

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

    }
}
