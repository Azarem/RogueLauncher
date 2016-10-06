using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SpecialItemType")]
    public class SpecialItemType
    {
        public const byte None = 0;

        public const byte FreeEntrance = 1;

        public const byte LoseCoins = 2;

        public const byte Revive = 3;

        public const byte SpikeImmunity = 4;

        public const byte GoldPerKill = 5;

        public const byte Compass = 6;

        public const byte Total = 7;

        public const byte Glasses = 8;

        public const byte EyeballToken = 9;

        public const byte SkullToken = 10;

        public const byte FireballToken = 11;

        public const byte BlobToken = 12;

        public const byte LastBossToken = 13;

        [Rewrite]
        public static string SpriteName(byte itemType) { return null; }
        [Rewrite]
        public static string ToString(byte itemType) { return null; }
    }
}
