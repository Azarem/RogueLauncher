using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EquipmentBaseType")]
    public class EquipmentBaseType
    {
        [Rewrite]
        public static string ToString(int equipmentBaseType) { return null; }
    }
}
