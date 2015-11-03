using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EquipmentAbilityType")]
    public class EquipmentAbilityType
    {
        [Rewrite]
        public static string ShortDescription(int type, float amount) { return null; }
        [Rewrite]
        public static string ToString(int type) { return null; }
        [Rewrite]
        public static string Instructions(int type) { return null; }
        [Rewrite]
        public static string Description(int type) { return null; }
    }
}
