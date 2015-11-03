using AssemblyTranslator;
using RogueAPI.Equipment;
using System.Collections.Generic;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EquipmentSystem")]
    public class EquipmentSystem
    {

        [Rewrite]
        private List<EquipmentBase[]> m_equipmentDataArray;

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Add)]
        public List<EquipmentBase[]> EquipmentDataArray { get { return m_equipmentDataArray; } }

        [Rewrite]
        public EquipmentBase GetEquipmentData(int category, int index) { return null; }

        //[Rewrite("RogueCastle.EquipmentData", action: RewriteAction.None)]
        //public class EquipmentDataStub
        //{
        //    [Rewrite("RogueCastle.EquipmentData", "BonusDamage", RewriteAction.None)]
        //    public int BonusDamage;

        //    [Rewrite("RogueCastle.EquipmentData", "BonusMagic", RewriteAction.None)]
        //    public int BonusMagic;

        //    [Rewrite("RogueCastle.EquipmentData", "Weight", RewriteAction.None)]
        //    public int Weight;

        //    [Rewrite("RogueCastle.EquipmentData", "BonusMana", RewriteAction.None)]
        //    public int BonusMana;

        //    [Rewrite("RogueCastle.EquipmentData", "BonusHealth", RewriteAction.None)]
        //    public int BonusHealth;

        //    [Rewrite("RogueCastle.EquipmentData", "BonusArmor", RewriteAction.None)]
        //    public int BonusArmor;

        //    [Rewrite("RogueCastle.EquipmentData", "Cost", RewriteAction.None)]
        //    public int Cost;

        //    [Rewrite("RogueCastle.EquipmentData", "FirstColour", RewriteAction.None)]
        //    public Color FirstColour;

        //    [Rewrite("RogueCastle.EquipmentData", "SecondColour", RewriteAction.None)]
        //    public Color SecondColour;

        //    [Rewrite("RogueCastle.EquipmentData", "SecondaryAttribute", RewriteAction.None)]
        //    public Vector2[] SecondaryAttribute;

        //    [Rewrite("RogueCastle.EquipmentData", "ChestColourRequirement", RewriteAction.None)]
        //    public byte ChestColourRequirement;

        //    [Rewrite("RogueCastle.EquipmentData", "LevelRequirement", RewriteAction.None)]
        //    public byte LevelRequirement;
        //}
    }
}
