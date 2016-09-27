using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using DS2DEngine;
using RogueAPI.Spells;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SpellEV")]
    public class SpellEV
    {
        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "RewriteGetProjData")]
        //public static RogueAPI.Projectiles.ProjectileInstance GetProjData(byte id, GameObj source) { return SpellDefinition.GetById(id).GetProjectileInstance(source); }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        //public static float GetDamageMultiplier(byte id) { return SpellDefinition.GetById(id).DamageMultiplier; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        //public static int GetRarity(byte id) { return SpellDefinition.GetById(id).Rarity; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        //public static int GetManaCost(byte id) { return SpellDefinition.GetById(id).ManaCost; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        //public static float GetXValue(byte id) { return SpellDefinition.GetById(id).MiscValue1; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        //public static float GetYValue(byte id) { return SpellDefinition.GetById(id).MiscValue2; }


        //public static void RewriteGetProjData(MethodGraph projMethod, MethodGraph newMethod)
        //{
        //    var newCon = Util.GetMethodInfo(() => new RogueAPI.Projectiles.ProjectileDefinition());

        //    //Replace the old constructor and remove usage of the Target field. The Type replacer will handle the rest.
        //    var instr = projMethod.InstructionList;
        //    var ix = 0;
        //    var count = instr.Count;
        //    while (ix < count)
        //    {
        //        var i = instr[ix];
        //        if (i is MethodInstruction)
        //        {
        //            var mi = i as MethodInstruction;
        //            if (mi.Operand.IsConstructor && mi.Operand.DeclaringType.Name == "ProjectileData")
        //            {
        //                mi.Operand = newCon;
        //                instr.RemoveAt(ix - 1);
        //                count--;
        //                continue;
        //            }
        //        }
        //        else if (i is FieldInstruction)
        //        {
        //            var fi = i as FieldInstruction;
        //            if (fi.ILCode == ILCode.Stfld && fi.Operand.Name == "Target")
        //            {
        //                instr.RemoveAt(ix--);
        //                instr.RemoveAt(ix--);
        //                instr.RemoveAt(ix);
        //                count -= 3;
        //                continue;
        //            }
        //        }
        //        ix++;
        //    }

        //    //Change return type
        //    projMethod.Parameters[0].ParameterType = typeof(RogueAPI.Projectiles.ProjectileDefinition);
        //    //Remove GameObj parameter
        //    //projMethod.Parameters.RemoveAt(2);

        //    //Change local types
        //    foreach (var l in instr.Locals)
        //        if (l.Type.Name == "ProjectileData")
        //            l.Type = typeof(RogueAPI.Projectiles.ProjectileDefinition);

        //}
    }
}
