using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EnchantressScreen")]
    public class EnchantressScreen
    {
        [Rewrite]
        private TextObj m_equipmentTitleText;
        [Rewrite]
        private TextObj m_descriptionText;
        [Rewrite]
        private int m_currentEquipmentIndex;
        [Rewrite]
        private TextObj m_instructionsTitleText;
        [Rewrite]
        private KeyIconTextObj m_instructionsText;
        [Rewrite]
        private int m_currentCategoryIndex;
        [Rewrite]
        private ObjContainer m_enchantressUI;

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Replace)]
        private void UpdateEquipmentDataText()
        {
            this.m_equipmentTitleText.Text = EquipmentAbilityType.ToString(this.m_currentEquipmentIndex) + " Rune\n(" + EquipmentCategoryType.ToString2(this.m_currentCategoryIndex - 6) + ")";
            this.m_descriptionText.Text = EquipmentAbilityType.Description(this.m_currentEquipmentIndex);
            this.m_descriptionText.WordWrap(195);
            this.m_descriptionText.Y = this.m_equipmentTitleText.Y + 60f;
            this.m_instructionsTitleText.Position = new Vector2(this.m_enchantressUI.X + 140f, this.m_descriptionText.Bounds.Bottom + 20);
            this.m_instructionsText.Text = EquipmentAbilityType.Instructions(this.m_currentEquipmentIndex);
            this.m_instructionsText.WordWrap(200);
            this.m_instructionsText.Position = new Vector2(this.m_instructionsTitleText.X, this.m_instructionsTitleText.Bounds.Bottom);
        }
    }
}
