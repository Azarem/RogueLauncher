using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EquipmentSecondaryDataType")]
    public class EquipmentSecondaryDataType
    {
        [Rewrite]
        public static string ToString(int equipmentSecondaryDataType) { return null; }
    }
}
