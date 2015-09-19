using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Traits;
using RogueAPI.Classes;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.TraitType")]
    public class TraitType
    {
        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Description(byte traitType, bool isFemale) { return TraitDefinition.GetById(traitType).Description; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ProfileCardDescription(byte traitType) { return TraitDefinition.GetById(traitType).ProfileCardDescription; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static byte Rarity(byte traitType) { return TraitDefinition.GetById(traitType).Rarity; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ToString(byte traitType) { return TraitDefinition.GetById(traitType).DisplayName; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static bool TraitConflict(Vector2 traits)
        {
            var t1 = TraitDefinition.GetById((byte)traits.X);
            var t2 = TraitDefinition.GetById((byte)traits.Y);
            return t1.TraitConflicts.Contains(t2) || t2.TraitConflicts.Contains(t1);
        }

        [Rewrite(action: RewriteAction.Replace)]
        public static TraitDefinition[] CreateRandomTraits(ClassDefinition cls)
        {
            int traitCount = 0;

            int rand = CDGMath.RandomInt(0, 100);
            if (rand < 94)
                traitCount++;
            if (rand < 55)
                traitCount++;

            var traits = new List<TraitDefinition>(traitCount);
            while (traitCount-- > 0)
            {
                rand = CDGMath.RandomInt(0, 100);
                int rarity = 1;
                if (rand > 48)
                    rarity++;
                if (rand > 85)
                    rarity++;

                while (rarity > 0)
                {
                    foreach (var t in TraitDefinition.All.Values)
                        if (t.Rarity == rarity && !traits.Contains(t) && !t.ClassConflicts.Contains(cls))
                        {
                            traits.Add(t);
                            rarity = 0;
                            break;
                        }
                    rarity--;
                }
            }

            return traits.ToArray();
        }
    }
}
