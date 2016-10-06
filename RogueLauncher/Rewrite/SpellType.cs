using AssemblyTranslator;
using Microsoft.Xna.Framework;
using RogueAPI.Spells;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SpellType")]
    public class SpellType
    {
        public const byte None = 0;

        public const byte Dagger = 1;

        public const byte Axe = 2;

        public const byte TimeBomb = 3;

        public const byte TimeStop = 4;

        public const byte Nuke = 5;

        public const byte Translocator = 6;

        public const byte Displacer = 7;

        public const byte Boomerang = 8;

        public const byte DualBlades = 9;

        public const byte Close = 10;

        public const byte DamageShield = 11;

        public const byte Bounce = 12;

        public const byte DragonFire = 13;

        public const byte RapidDagger = 14;

        public const byte DragonFireNeo = 15;

        public const byte Total = 16;

        public const byte Laser = 100;

        public const byte Shout = 20;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ToString(byte id) { return SpellDefinition.GetById(id).DisplayName; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Description(byte id) { return SpellDefinition.GetById(id).Description; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Icon(byte id) { return SpellDefinition.GetById(id).Icon; }

        [Rewrite]
        public static Vector3 GetNext3Spells() { return Vector3.Zero; }
    }
}
