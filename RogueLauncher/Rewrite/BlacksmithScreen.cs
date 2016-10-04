using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using RogueAPI.Equipment;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Reflection;
using Tweener;
using Tweener.Ease;

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
        private Cue m_rainSound;
        [Rewrite]
        private bool m_lockControls;
        [Rewrite]
        private KeyIconTextObj m_confirmText;
        [Rewrite]
        private KeyIconTextObj m_cancelText;
        [Rewrite]
        private KeyIconTextObj m_navigationText;
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

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        private void EquipmentSelectionInput()
        {
            var categoryIndex = m_currentCategoryIndex - 6;
            int oldIndex = m_currentEquipmentIndex;

            if (InputManager.IsNewlyPressed(InputFlags.PlayerUp1 | InputFlags.PlayerUp2))
                oldIndex = (oldIndex + 10) % 15;
            else if (InputManager.IsNewlyPressed(InputFlags.PlayerDown1 | InputFlags.PlayerDown2))
                oldIndex = (oldIndex + 5) % 15;
            
            if (InputManager.IsNewlyPressed(InputFlags.PlayerLeft1 | InputFlags.PlayerLeft2))
                oldIndex += oldIndex % 5 == 0 ? 4 : -1;

            if (InputManager.IsNewlyPressed(InputFlags.PlayerRight1 | InputFlags.PlayerRight2))
                oldIndex += oldIndex % 5 == 4 ? -4 : 1;

            var bpArr = Game.PlayerStats.GetBlueprintArray[categoryIndex];

            if (oldIndex != m_currentEquipmentIndex)
            {
                m_currentEquipmentIndex = oldIndex;

                if (bpArr[oldIndex] == 1)
                    bpArr[oldIndex] = 2;

                UpdateNewIcons();
                UpdateIconSelectionText();
                m_selectionIcon.Position = m_activeIconArray[oldIndex].AbsPosition;
                SoundManager.PlaySound("ShopBSMenuMove");
            }

            if (InputManager.IsNewlyPressed(InputFlags.MenuCancel1 | InputFlags.MenuCancel2))
            {
                SoundManager.PlaySound("ShopMenuCancel");
                m_inCategoryMenu = true;
                m_selectionIcon.Position = m_blacksmithUI.GetChildAt(m_currentCategoryIndex).AbsPosition;
                UpdateIconSelectionText();
            }

            if (InputManager.IsNewlyPressed(InputFlags.MenuConfirm1 | InputFlags.MenuConfirm2))
            {
                var player = this.Player;
                int item = bpArr[oldIndex];
                var equipArray = Game.PlayerStats.GetEquippedArray;
                var currentEquipped = equipArray[categoryIndex];

                if (item < 3 && item > 0)
                {
                    EquipmentBase equipmentData = Game.EquipmentSystem.GetEquipmentData(categoryIndex, oldIndex);
                    if (Game.PlayerStats.Gold < equipmentData.Cost)
                    {
                        SoundManager.PlaySound("ShopMenuUnlockFail");
                    }
                    else
                    {
                        SoundManager.PlaySound("ShopMenuUnlock");
                        Game.PlayerStats.Gold -= equipmentData.Cost;
                        bpArr[oldIndex] = 3;
                        ObjContainer firstColour = this.m_masterIconArray[categoryIndex][oldIndex];

                        firstColour.ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(categoryIndex) + (oldIndex % 5 + 1) + "Icon_Character");

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
                if (currentEquipped != oldIndex && item == 3)
                {
                    EquipmentBase equipmentDatum = Game.EquipmentSystem.GetEquipmentData(categoryIndex, oldIndex);
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
                    equipArray[categoryIndex] = (sbyte)oldIndex;
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
                if (currentEquipped == oldIndex)
                {
                    equipArray[categoryIndex] = -1;
                    player.UpdateEquipmentColours();
                    this.UpdateIconSelectionText();
                    this.UpdateNewIcons();
                }
            }
        }

        [Rewrite]
        private void DisplayCategory(int equipmentType) { }

        [Rewrite]
        public void EaseInMenu() { }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void OnEnter()
        {
            if (m_rainSound != null)
                m_rainSound.Dispose();

            if ((DateTime.Now.Month == 12 ? true : DateTime.Now.Month == 1))
                m_rainSound = SoundManager.PlaySound("snowloop_filtered");
            else
                m_rainSound = SoundManager.PlaySound("Rain1_Filtered");

            if (Game.PlayerStats.TotalBlueprintsFound >= 75)
                GameUtil.UnlockAchievement("FEAR_OF_THROWING_STUFF_OUT");

            m_lockControls = true;
            SoundManager.PlaySound("ShopMenuOpen");

            m_confirmText.Opacity = 0f;
            m_cancelText.Opacity = 0f;
            m_navigationText.Opacity = 0f;

            Tween.To(m_confirmText, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(m_cancelText, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(m_navigationText, 0.2f, Linear.EaseNone, "Opacity", "1");

            m_confirmText.Text = "[Input:" + (int)InputKeys.MenuConfirm1 + "]  select/equip";
            m_cancelText.Text = "[Input:" + (int)InputKeys.MenuCancel1 + "]  cancel/close menu";

            if (InputManager.IsGamepadConnected())
                m_navigationText.Text = "[Button:LeftStick] to navigate";
            else
                m_navigationText.Text = "Arrow keys to navigate";

            m_currentEquipmentIndex = 0;
            m_inCategoryMenu = true;
            m_selectionIcon.Position = m_blacksmithUI.GetChildAt(6).AbsPosition;
            m_currentCategoryIndex = 6;
            UpdateIconStates();

            DisplayCategory(0);
            EaseInMenu();

            Tween.To(this, 0.2f, Linear.EaseNone, "BackBufferOpacity", "0.5");
            UpdateIconSelectionText();

            base.OnEnter();
        }

        [Rewrite]
        private void UpdateNewIcons() { }

        [Rewrite]
        private void UpdateIconSelectionText() { }
    }
}
