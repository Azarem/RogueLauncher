using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EquipmentCategoryType")]
    public class EquipmentCategoryType
    {
        [Rewrite]
        public static string ToString(int equipmentType) { return null; }
        [Rewrite]
        public static string ToString2(int equipmentType) { return null; }
    }
}
