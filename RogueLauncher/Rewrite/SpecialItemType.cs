using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SpecialItemType")]
    public class SpecialItemType
    {
        [Rewrite]
        public static string SpriteName(byte itemType) { return null; }
        [Rewrite]
        public static string ToString(byte itemType) { return null; }
    }
}
