using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Equipment;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.BlacksmithScreen")]
    public class BlacksmithScreen : Screen
    {
        [Rewrite]
        private ObjContainer m_textInfoStatContainer;
        [Rewrite]
        private ObjContainer m_textInfoStatModContainer;
        [Rewrite]
        private TextObj m_addPropertiesText;
        [Rewrite]
        private TextObj m_equipmentTitleText;
        [Rewrite]
        private int m_currentCategoryIndex;
        [Rewrite]
        private int m_currentEquipmentIndex;
        [Rewrite]
        private List<ObjContainer[]> m_masterIconArray;
        [Rewrite]
        private ObjContainer m_blacksmithUI;
        [Rewrite]
        private SpriteObj m_selectionIcon;
        [Rewrite]
        private bool m_inCategoryMenu;
        [Rewrite]
        private ObjContainer[] m_activeIconArray;
        [Rewrite]
        public PlayerObj Player { get; set; }
        [Rewrite]
        private int CurrentCategoryIndex { get { return 0; } }

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Add)]
        private void SetStatTextObject(int index, int value)
        {
            var text = this.m_textInfoStatModContainer.GetChildAt(index) as TextObj;
            if (value > 0)
            {
                text.TextureColor = index == 5 ? Color.Red : Color.Cyan;
                text.Text = "+" + value;
            }
            else if (value < 0)
            {
                text.TextureColor = index == 5 ? Color.Cyan : Color.Red;
                text.Text = value.ToString();
            }
            else
                text.Text = "";
        }

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Replace)]
        private void UpdateEquipmentDataText()
        {
            var player = this.Player;

            (this.m_textInfoStatContainer.GetChildAt(0) as TextObj).Text = player.MaxHealth.ToString();
            (this.m_textInfoStatContainer.GetChildAt(1) as TextObj).Text = player.MaxMana.ToString();
            (this.m_textInfoStatContainer.GetChildAt(2) as TextObj).Text = player.Damage.ToString();
            (this.m_textInfoStatContainer.GetChildAt(3) as TextObj).Text = player.TotalMagicDamage.ToString();
            (this.m_textInfoStatContainer.GetChildAt(4) as TextObj).Text = player.TotalArmor.ToString();
            (this.m_textInfoStatContainer.GetChildAt(5) as TextObj).Text = player.CurrentWeight + "/" + player.MaxWeight;

            int mCurrentCategoryIndex = this.m_currentCategoryIndex - 6;
            int equippedIndex = Game.PlayerStats.GetEquippedArray[mCurrentCategoryIndex];

            EquipmentBase currentData = equippedIndex == this.m_currentEquipmentIndex
                ? new EquipmentBase()
                : Game.EquipmentSystem.GetEquipmentData(mCurrentCategoryIndex, this.m_currentEquipmentIndex);

            EquipmentBase equippedData = equippedIndex >= 0
                ? Game.EquipmentSystem.GetEquipmentData(mCurrentCategoryIndex, equippedIndex)
                : new EquipmentBase();

            SetStatTextObject(0, currentData.BonusHealth - equippedData.BonusHealth);
            SetStatTextObject(1, currentData.BonusMana - equippedData.BonusMana);
            SetStatTextObject(2, currentData.BonusDamage - equippedData.BonusDamage);
            SetStatTextObject(3, currentData.BonusMagic - equippedData.BonusMagic);
            SetStatTextObject(4, currentData.BonusArmor - equippedData.BonusArmor);
            SetStatTextObject(5, currentData.Weight - equippedData.Weight);

            if (equippedIndex == this.m_currentEquipmentIndex)
                currentData = equippedData;

            Vector2[] secondaryAttribute = currentData.SecondaryAttribute;
            string secondaryString = "None";

            if (secondaryAttribute != null && secondaryAttribute.Length > 0)
            {
                var accum = "";

                foreach (var v in secondaryAttribute)
                    if (v.X > 0f)
                        accum += "+" + (v.X >= 7f ? v.Y + " " : (v.Y * 100 + "% ")) + EquipmentSecondaryDataType.ToString((int)v.X) + "\n";

                if (accum.Length > 0)
                    secondaryString = accum;
            }

            this.m_addPropertiesText.Text = secondaryString;
            this.m_equipmentTitleText.Text = currentData.ShortDisplayName;
        }

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Replace)]
        private void UpdateIconStates()
        {
            var bpArr = Game.PlayerStats.GetBlueprintArray;

            int catIx = 0, catCount = bpArr.Count;
            while (catIx < catCount)
            {
                var catArr = bpArr[catIx];
                var iconArr = this.m_masterIconArray[catIx];
                var catString = EquipmentCategoryType.ToString(catIx);

                int itemIx = 0, itemCount = catArr.Length;
                while (itemIx < itemCount)
                {
                    var item = catArr[itemIx];
                    var icon = iconArr[itemIx];

                    if (item != 0)
                    {
                        icon.ChangeSprite("BlacksmithUI_" + catString + (itemIx % 5 + 1) + "Icon_Character");
                        for (int k = 1; k < icon.NumChildren; k++)
                            icon.GetChildAt(k).Opacity = 0.2f;
                    }
                    else
                        icon.ChangeSprite("BlacksmithUI_QuestionMarkIcon_Character");

                    if (item > 2)
                    {
                        for (int l = 1; l < icon.NumChildren; l++)
                            icon.GetChildAt(l).Opacity = 1f;

                        int num1 = catIx == 0 ? 2 : 1;

                        EquipmentBase equipmentData = Game.EquipmentSystem.GetEquipmentData(catIx, itemIx);
                        icon.GetChildAt(num1).TextureColor = equipmentData.FirstColour;

                        if (catIx != 4)
                            icon.GetChildAt(num1 + 1).TextureColor = equipmentData.SecondColour;
                    }

                    itemIx++;
                }
                catIx++;
            }
        }

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Replace)]
        private void EquipmentSelectionInput()
        {
            var categoryIndex = this.m_currentCategoryIndex - 6;
            int equipmentIndex = this.m_currentEquipmentIndex;

            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                equipmentIndex = (equipmentIndex + 10) % 15;

            if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
                equipmentIndex = (equipmentIndex + 5) % 15;

            if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
                equipmentIndex += equipmentIndex % 5 == 0 ? 4 : -1;

            if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
                equipmentIndex += equipmentIndex % 5 == 4 ? -4 : 1;

            var bpArr = Game.PlayerStats.GetBlueprintArray[categoryIndex];

            if (equipmentIndex != this.m_currentEquipmentIndex)
            {
                this.m_currentEquipmentIndex = equipmentIndex;

                if (bpArr[equipmentIndex] == 1)
                    bpArr[equipmentIndex] = 2;

                this.UpdateNewIcons();
                this.UpdateIconSelectionText();
                this.m_selectionIcon.Position = this.m_activeIconArray[equipmentIndex].AbsPosition;
                SoundManager.PlaySound("ShopBSMenuMove");
            }

            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                SoundManager.PlaySound("ShopMenuCancel");
                this.m_inCategoryMenu = true;
                this.m_selectionIcon.Position = this.m_blacksmithUI.GetChildAt(this.m_currentCategoryIndex).AbsPosition;
                this.UpdateIconSelectionText();
            }
            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
            {
                var player = this.Player;
                int item = bpArr[equipmentIndex];
                var equipArray = Game.PlayerStats.GetEquippedArray;
                var currentEquipped = equipArray[categoryIndex];

                if (item < 3 && item > 0)
                {
                    EquipmentBase equipmentData = Game.EquipmentSystem.GetEquipmentData(categoryIndex, equipmentIndex);
                    if (Game.PlayerStats.Gold < equipmentData.Cost)
                    {
                        SoundManager.PlaySound("ShopMenuUnlockFail");
                    }
                    else
                    {
                        SoundManager.PlaySound("ShopMenuUnlock");
                        Game.PlayerStats.Gold -= equipmentData.Cost;
                        bpArr[equipmentIndex] = 3;
                        ObjContainer firstColour = this.m_masterIconArray[categoryIndex][equipmentIndex];

                        firstColour.ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(categoryIndex) + (equipmentIndex % 5 + 1) + "Icon_Character");

                        for (int i = 1; i < firstColour.NumChildren; i++)
                            firstColour.GetChildAt(i).Opacity = 1f;

                        int num1 = categoryIndex == 0 ? 2 : 1;

                        firstColour.GetChildAt(num1).TextureColor = equipmentData.FirstColour;
                        if (categoryIndex != 4)
                            firstColour.GetChildAt(num1 + 1).TextureColor = equipmentData.SecondColour;

                        item = 3;
                        this.UpdateIconSelectionText();
                    }
                }
                if (currentEquipped != equipmentIndex && item == 3)
                {
                    EquipmentBase equipmentDatum = Game.EquipmentSystem.GetEquipmentData(categoryIndex, equipmentIndex);
                    //int getEquippedArray1 = Game.PlayerStats.GetEquippedArray[categoryIndex];
                    int weight = 0;
                    if (currentEquipped != -1)
                        weight = Game.EquipmentSystem.GetEquipmentData(categoryIndex, currentEquipped).Weight;

                    if (equipmentDatum.Weight + player.CurrentWeight - weight > player.MaxWeight)
                    {
                        Console.WriteLine(string.Concat("cannot equip. too heavy. Weight:", equipmentDatum.Weight + player.CurrentWeight - weight));
                        return;
                    }
                    SoundManager.PlaySound("ShopBSEquip");
                    equipArray[categoryIndex] = (sbyte)equipmentIndex;
                    this.UpdateIconSelectionText();
                    Vector3 partIndices = PlayerPart.GetPartIndices(categoryIndex);

                    if (partIndices.X != -1f)
                       player.GetChildAt((int)partIndices.X).TextureColor = equipmentDatum.FirstColour;

                    if (partIndices.Y != -1f)
                        player.GetChildAt((int)partIndices.Y).TextureColor = equipmentDatum.SecondColour;

                    if (partIndices.Z != -1f)
                        player.GetChildAt((int)partIndices.Z).TextureColor = equipmentDatum.SecondColour;

                    if (categoryIndex == 2 && partIndices.X != -1f)
                        player.GetChildAt(5).TextureColor = equipmentDatum.FirstColour;

                    this.UpdateNewIcons();
                    return;
                }
                if (currentEquipped == equipmentIndex)
                {
                    equipArray[categoryIndex] = -1;
                    player.UpdateEquipmentColours();
                    this.UpdateIconSelectionText();
                    this.UpdateNewIcons();
                }
            }
        }

        [Rewrite]
        private void UpdateNewIcons() { }

        [Rewrite]
        private void UpdateIconSelectionText() { }
    }
}
