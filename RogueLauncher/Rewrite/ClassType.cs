using AssemblyTranslator;
using DS2DEngine;
using RogueAPI.Classes;
using System.Collections.Generic;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ClassType")]
    public class ClassType
    {
        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Description(byte id) { return ClassDefinition.GetById(id).Description; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ProfileCardDescription(byte id) { return ClassDefinition.GetById(id).GetProfileCardDescription(); }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ToString(byte id, bool isFemale) { return ClassDefinition.GetById(id).GetDisplayName(isFemale); }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static byte[] GetSpellList(byte id) { return ClassDefinition.GetById(id).SpellByteArray; }

        [Rewrite(action: RewriteAction.Replace)]
        public static byte GetRandomClass()
        {
            List<byte> nums = new List<byte>() { 0, 1, 2, 3 };

            if (SkillSystem.GetSkill(SkillType.Ninja_Unlock).ModifierAmount > 0f)
                nums.Add(4);

            if (SkillSystem.GetSkill(SkillType.Banker_Unlock).ModifierAmount > 0f)
                nums.Add(5);

            if (SkillSystem.GetSkill(SkillType.Spellsword_Unlock).ModifierAmount > 0f)
                nums.Add(6);

            if (SkillSystem.GetSkill(SkillType.Lich_Unlock).ModifierAmount > 0f)
                nums.Add(7);

            if (SkillSystem.GetSkill(SkillType.SuperSecret).ModifierAmount > 0f)
                nums.Add(16);

            if (Game.PlayerStats.ChallengeLastBossBeaten)
                nums.Add(17);

            byte item = nums[CDGMath.RandomInt(0, nums.Count - 1)];
            if (ClassType.Upgraded(item))
                item += 8;

            return item;
        }

        [Rewrite(action: RewriteAction.Replace)]
        public static bool Upgraded(byte classType)
        {
            switch (classType)
            {
                case 0: return SkillSystem.GetSkill(SkillType.Knight_Up).ModifierAmount > 0f;
                case 1: return SkillSystem.GetSkill(SkillType.Mage_Up).ModifierAmount > 0f;
                case 2: return SkillSystem.GetSkill(SkillType.Barbarian_Up).ModifierAmount > 0f;
                case 3: return SkillSystem.GetSkill(SkillType.Assassin_Up).ModifierAmount > 0f;
                case 4: return SkillSystem.GetSkill(SkillType.Ninja_Up).ModifierAmount > 0f;
                case 5: return SkillSystem.GetSkill(SkillType.Banker_Up).ModifierAmount > 0f;
                case 6: return SkillSystem.GetSkill(SkillType.SpellSword_Up).ModifierAmount > 0f;
                case 7: return SkillSystem.GetSkill(SkillType.Lich_Up).ModifierAmount > 0f;
            }
            return false;
        }
    }
}
