using AssemblyTranslator;
using System;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SkillSystem")]
    public class SkillSystem
    {
        [Rewrite]
        private static List<SkillObj> m_skillArray;
        [Rewrite]
        private static SkillObj m_blankTrait;

        [Rewrite]
        public static SkillObj GetSkill(SkillType skillType)
        {
            SkillObj skillObj;
            List<SkillObj>.Enumerator enumerator = SkillSystem.m_skillArray.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    SkillObj current = enumerator.Current;
                    if (current.TraitType != skillType)
                    {
                        continue;
                    }
                    skillObj = current;
                    return skillObj;
                }
                return SkillSystem.m_blankTrait;
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            return skillObj;
        }
    }
}
