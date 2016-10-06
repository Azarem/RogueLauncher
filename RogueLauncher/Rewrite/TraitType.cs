using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Classes;
using RogueAPI.Traits;
using System.Collections.Generic;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.TraitType")]
    public class TraitType
    {
        public const byte None = 0;

        public const byte ColorBlind = 1;

        public const byte Gay = 2;

        public const byte NearSighted = 3;

        public const byte FarSighted = 4;

        public const byte Dyslexia = 5;

        public const byte Gigantism = 6;

        public const byte Dwarfism = 7;

        public const byte Baldness = 8;

        public const byte Endomorph = 9;

        public const byte Ectomorph = 10;

        public const byte Alzheimers = 11;

        public const byte Dextrocardia = 12;

        public const byte Tourettes = 13;

        public const byte Hyperactive = 14;

        public const byte OCD = 15;

        public const byte Hypergonadism = 16;

        public const byte Hypogonadism = 17;

        public const byte StereoBlind = 18;

        public const byte IBS = 19;

        public const byte Vertigo = 20;

        public const byte TunnelVision = 21;

        public const byte Ambilevous = 22;

        public const byte PAD = 23;

        public const byte Alektorophobia = 24;

        public const byte Hypochondriac = 25;

        public const byte Dementia = 26;

        public const byte Hypermobility = 27;

        public const byte EideticMemory = 28;

        public const byte Nostalgic = 29;

        public const byte CIP = 30;

        public const byte Savant = 31;

        public const byte TheOne = 32;

        public const byte NoFurniture = 33;

        public const byte PlatformsOpen = 34;

        public const byte Glaucoma = 35;

        public const byte Total = 36;

        public const byte Adopted = 100;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Description(byte traitType, bool isFemale) { return TraitDefinition.GetById(traitType).GetDescription(Game.PlayerStats.IsFemale); }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ProfileCardDescription(byte traitType) { return TraitDefinition.GetById(traitType).GetProfileDescription(Game.PlayerStats.IsFemale); }

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
