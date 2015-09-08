using System;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Spells;
using Tweener;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.GetItemScreen")]
    public class GetItemScreen : Screen
    {
        [Rewrite]
        private SpriteObj m_tripStat1;
        [Rewrite]
        private SpriteObj m_tripStat2;
        [Rewrite]
        private Vector2 m_itemInfo;
        [Rewrite]
        private byte m_itemType;
        [Rewrite]
        private Vector2 m_itemStartPos;
        [Rewrite]
        private Vector2 m_itemEndPos;
        [Rewrite]
        private SpriteObj m_itemSprite;
        [Rewrite]
        private string m_songName;
        [Rewrite]
        private float m_storedMusicVolume;
        [Rewrite]
        private bool m_lockControls;
        [Rewrite]
        private KeyIconTextObj m_continueText;
        [Rewrite]
        private TextObj m_itemFoundText;
        [Rewrite]
        private TextObj m_tripStat1FoundText;
        [Rewrite]
        private TextObj m_tripStat2FoundText;
        [Rewrite]
        private bool m_itemSpinning;
        [Rewrite]
        private SpriteObj m_itemFoundSprite;
        [Rewrite]
        private Vector2 m_tripStatData;

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Replace)]
        public virtual void OnEnter()
        {
            this.m_tripStat1.Visible = false;
            this.m_tripStat2.Visible = false;
            this.m_tripStat1.Scale = Vector2.One;
            this.m_tripStat2.Scale = Vector2.One;
            if (this.m_itemType != 7)
                (base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[] { SaveType.PlayerData, SaveType.UpgradeData });

            this.m_itemSprite.Rotation = 0f;
            this.m_itemSprite.Scale = Vector2.One;
            this.m_itemStartPos.X = this.m_itemStartPos.X - base.Camera.TopLeftCorner.X;
            this.m_itemStartPos.Y = this.m_itemStartPos.Y - base.Camera.TopLeftCorner.Y;
            this.m_storedMusicVolume = SoundManager.GlobalMusicVolume;
            this.m_songName = SoundManager.GetCurrentMusicName();
            this.m_lockControls = true;
            this.m_continueText.Opacity = 0f;
            this.m_continueText.Text = "[Input:0]  to continue";
            this.m_itemFoundText.Position = this.m_itemEndPos;
            this.m_itemFoundText.Y += 70f;
            this.m_itemFoundText.Scale = Vector2.Zero;
            this.m_tripStat1FoundText.Position = this.m_itemFoundText.Position;
            this.m_tripStat2FoundText.Position = this.m_itemFoundText.Position;
            this.m_tripStat1FoundText.Scale = Vector2.Zero;
            this.m_tripStat2FoundText.Scale = Vector2.Zero;
            this.m_tripStat1FoundText.Visible = false;
            this.m_tripStat2FoundText.Visible = false;
            switch (this.m_itemType)
            {
                case 1:
                    this.m_itemSpinning = true;
                    this.m_itemSprite.ChangeSprite("BlueprintIcon_Sprite");
                    this.m_itemFoundSprite.ChangeSprite("BlueprintFoundText_Sprite");
                    this.m_itemFoundText.Text = Game.EquipmentSystem.GetEquipmentData((int)this.m_itemInfo.X, (int)this.m_itemInfo.Y).DisplayName;// string.Concat(EquipmentBaseType.ToString((int)this.m_itemInfo.Y), " ", EquipmentCategoryType.ToString2((int)this.m_itemInfo.X));
                    break;

                case 2:
                    this.m_itemSpinning = true;
                    this.m_itemSprite.ChangeSprite("RuneIcon_Sprite");
                    this.m_itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
                    this.m_itemFoundText.Text = EquipmentAbilityType.ToString((int)this.m_itemInfo.Y) + " Rune (" + EquipmentCategoryType.ToString2((int)this.m_itemInfo.X) + ")";
                    this.m_itemSprite.AnimationDelay = 0.05f;
                    GameUtil.UnlockAchievement("LOVE_OF_MAGIC");
                    break;

                case 3:
                case 6:
                    this.m_itemSprite.ChangeSprite(this.GetStatSpriteName((int)this.m_itemInfo.X));
                    this.m_itemFoundText.Text = this.GetStatText((int)this.m_itemInfo.X);
                    this.m_itemSprite.AnimationDelay = 0.05f;
                    this.m_itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
                    if (this.m_itemType == 6)
                    {
                        this.m_tripStat1FoundText.Visible = true;
                        this.m_tripStat2FoundText.Visible = true;
                        this.m_tripStat1.ChangeSprite(this.GetStatSpriteName((int)this.m_tripStatData.X));
                        this.m_tripStat2.ChangeSprite(this.GetStatSpriteName((int)this.m_tripStatData.Y));
                        this.m_tripStat1.Visible = true;
                        this.m_tripStat2.Visible = true;
                        this.m_tripStat1.AnimationDelay = 0.05f;
                        this.m_tripStat2.AnimationDelay = 0.05f;
                        Tween.RunFunction(0.1f, this.m_tripStat1, "PlayAnimation", new object[] { true });
                        Tween.RunFunction(0.2f, this.m_tripStat2, "PlayAnimation", new object[] { true });
                        this.m_tripStat1FoundText.Text = this.GetStatText((int)this.m_tripStatData.X);
                        this.m_tripStat2FoundText.Text = this.GetStatText((int)this.m_tripStatData.Y);
                        this.m_itemFoundText.Y += 50f;
                        this.m_tripStat1FoundText.Y = this.m_itemFoundText.Y + 50f;
                    }
                    break;

                case 4:
                    this.m_itemSprite.ChangeSprite(SpellType.Icon((byte)this.m_itemInfo.X));
                    this.m_itemFoundSprite.ChangeSprite("SpellFoundText_Sprite");
                    this.m_itemFoundText.Text = SpellDefinition.GetById((byte)this.m_itemInfo.X).DisplayName;
                    break;

                case 5:
                    this.m_itemSprite.ChangeSprite(SpecialItemType.SpriteName((byte)this.m_itemInfo.X));
                    this.m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    this.m_itemFoundText.Text = SpecialItemType.ToString((byte)this.m_itemInfo.X);
                    break;

                case 7:
                    this.m_itemSprite.ChangeSprite(this.GetMedallionImage((int)this.m_itemInfo.X));
                    this.m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    if (this.m_itemInfo.X != 19f)
                        this.m_itemFoundText.Text = "You've collected a medallion piece!";
                    else
                        this.m_itemFoundText.Text = "Medallion completed!";

                    break;
            }
            this.m_itemSprite.PlayAnimation(true);
            this.ItemSpinAnimation();
            base.OnEnter();
        }

        [Rewrite]
        private string GetStatSpriteName(int type) { return null; }

        [Rewrite]
        private void ItemSpinAnimation() { }

        [Rewrite]
        private string GetMedallionImage(int medallionType) { return null; }

        [Rewrite]
        private string GetStatText(int type) { return null; }
    }
}
