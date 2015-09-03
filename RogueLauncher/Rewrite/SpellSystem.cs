using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using DS2DEngine;
using RogueAPI.Spells;
using System;
using System.Linq;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    internal static class SpellSystem
    {

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellEV", "GetProjData", action: RewriteAction.Swap, newName: "_GetProjData", stubAction: StubAction.UseOld, contentHandler: "RewriteGetProjData")]
        public static RogueAPI.Projectiles.ProjectileInstance GetProjData(byte id, GameObj source)
        {
            return SpellDefinition.GetById(id).GetProjectileInstance(source);
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellEV", "GetDamageMultiplier", action: RewriteAction.Swap, newName: "_GetDamageMultiplier", stubAction: StubAction.UseOld)]
        public static float GetDamageMultiplier(byte id)
        {
            return SpellDefinition.GetById(id).DamageMultiplier;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellEV", "GetRarity", action: RewriteAction.Swap, newName: "_GetRarity", stubAction: StubAction.UseOld)]
        public static int GetRarity(byte id)
        {
            return SpellDefinition.GetById(id).Rarity;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellEV", "GetManaCost", action: RewriteAction.Swap, newName: "_GetManaCost", stubAction: StubAction.UseOld)]
        public static int GetManaCost(byte id)
        {
            return SpellDefinition.GetById(id).ManaCost;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellEV", "GetXValue", action: RewriteAction.Swap, newName: "_GetXValue", stubAction: StubAction.UseOld)]
        public static float GetXValue(byte id)
        {
            return SpellDefinition.GetById(id).MiscValue1;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellEV", "GetYValue", action: RewriteAction.Swap, newName: "_GetYValue", stubAction: StubAction.UseOld)]
        public static float GetYValue(byte id)
        {
            return SpellDefinition.GetById(id).MiscValue2;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellType", "ToString", action: RewriteAction.Swap, newName: "_ToString", stubAction: StubAction.UseOld)]
        public static string ToString(byte id)
        {
            return SpellDefinition.GetById(id).DisplayName;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellType", "ToString", action: RewriteAction.Swap, newName: "_Description", stubAction: StubAction.UseOld)]
        public static string Description(byte id)
        {
            return SpellDefinition.GetById(id).Description;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.SpellType", "Icon", action: RewriteAction.Swap, newName: "_Icon", stubAction: StubAction.UseOld)]
        public static string Icon(byte id)
        {
            return SpellDefinition.GetById(id).Icon;
        }

        public static void RewriteGetProjData(MethodGraph projMethod, MethodGraph newMethod)
        {
            var newCon = Util.GetMethodInfo(() => new RogueAPI.Projectiles.ProjectileDefinition());

            //Replace the old constructor and remove usage of the Target field. The Type replacer will handle the rest.
            var instr = projMethod.InstructionList;
            var ix = 0;
            var count = instr.Count;
            while (ix < count)
            {
                var i = instr[ix];
                if (i is MethodInstruction)
                {
                    var mi = i as MethodInstruction;
                    if (mi.Operand.IsConstructor && mi.Operand.DeclaringType.Name == "ProjectileData")
                    {
                        mi.Operand = newCon;
                        instr.RemoveAt(ix - 1);
                        count--;
                        continue;
                    }
                }
                else if (i is FieldInstruction)
                {
                    var fi = i as FieldInstruction;
                    if (fi.ILCode == ILCode.Stfld && fi.Operand.Name == "Target")
                    {
                        instr.RemoveAt(ix--);
                        instr.RemoveAt(ix--);
                        instr.RemoveAt(ix);
                        count -= 3;
                        continue;
                    }
                }
                ix++;
            }

            //Change return type
            projMethod.Parameters[0].ParameterType = typeof(RogueAPI.Projectiles.ProjectileDefinition);
            //Remove GameObj parameter
            //projMethod.Parameters.RemoveAt(2);

            //Change local types
            foreach (var l in instr.Locals)
                if (l.Type.Name == "ProjectileData")
                    l.Type = typeof(RogueAPI.Projectiles.ProjectileDefinition);

        }

    }
}
