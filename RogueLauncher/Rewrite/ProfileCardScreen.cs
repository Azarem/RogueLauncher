using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProfileCardScreen")]
    public class ProfileCardScreen : Screen
    {
        [Rewrite]
        private List<TextObj> m_dataList1;
        [Rewrite]
        private List<TextObj> m_dataList2;
        [Rewrite]
        private List<TextObj> m_equipmentList;
        [Rewrite]
        private TextObj m_equipmentTitle;
        [Rewrite]
        private TextObj m_runesTitle;
        [Rewrite]
        private List<TextObj> m_runeBackTitleList;
        [Rewrite]
        private List<TextObj> m_runeBackDescriptionList;
        [Rewrite]
        private ObjContainer m_backCard;
        [Rewrite]
        private ObjContainer m_playerSprite;
        [Rewrite]
        private ObjContainer m_frontCard;
        [Rewrite]
        private PlayerHUDObj m_playerHUD;
        [Rewrite]
        private KeyIconTextObj m_cancelText;
        [Rewrite]
        private bool m_playerInAir;
        [Rewrite]
        private SpriteObj m_tombStoneSprite;
        [Rewrite]
        private SpriteObj m_spellIcon;
        [Rewrite]
        public float BackBufferOpacity { get; set; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        private void LoadBackCardStats(PlayerObj player)
        {
            TextObj obj1, obj2;

            int ix = 0, count = this.m_dataList1.Count;
            while (ix < count)
            {
                obj1 = this.m_dataList1[ix];
                obj2 = this.m_dataList2[ix];

                switch (ix)
                {
                    case 0:
                        obj1.Text = player.MaxHealth.ToString();
                        obj2.Text = player.Damage.ToString();
                        break;

                    case 1:
                        obj1.Text = player.MaxMana.ToString();
                        obj2.Text = player.TotalMagicDamage.ToString();
                        break;

                    case 2:
                        obj1.Text = player.TotalArmor + "(" + (int)(player.TotalDamageReduc * 100f) + "%)";
                        obj2.Text = (int)Math.Round(player.TotalCritChance * 100f, MidpointRounding.AwayFromZero) + "%";
                        break;

                    case 3:
                        obj1.Text = player.CurrentWeight + "/" + player.MaxWeight;
                        obj2.Text = (int)(player.TotalCriticalDamage * 100f) + "%";
                        break;
                }

                ix++;
            }

            var getEquippedArray = Game.PlayerStats.GetEquippedArray;
            ix = 0;
            count = getEquippedArray.Length;
            int posY = (int)this.m_equipmentTitle.Y + 40;
            while (ix < count)
            {
                int id = getEquippedArray[ix];
                obj1 = this.m_equipmentList[ix];

                if (id >= 0)
                {
                    obj1.Y = posY;
                    obj1.Visible = true;
                    obj1.Text = Game.EquipmentSystem.EquipmentDataArray[ix][id].DisplayName;
                    posY += 20;
                }
                else
                    obj1.Visible = false;

                ix++;
            }

            ix = 0;
            count = this.m_runeBackTitleList.Count;
            posY = (int)this.m_runesTitle.Y + 40;
            while (ix < count)
            {
                obj1 = this.m_runeBackTitleList[ix];
                obj2 = this.m_runeBackDescriptionList[ix];

                float value = 0f;
                switch (ix)
                {
                    case 0: value = player.TotalDoubleJumps; break;
                    case 1: value = player.TotalAirDashes; break;
                    case 2: value = player.TotalVampBonus; break;
                    case 3: value = player.TotalFlightTime; break;
                    case 4: value = player.ManaGain; break;
                    case 5: value = player.TotalDamageReturn * 100f; break;
                    case 6: value = player.TotalGoldBonus * 100f; break;
                    case 7: value = player.TotalMovementSpeedPercent * 100f - 100f; break;
                    case 8: value = Game.PlayerStats.GetNumberOfEquippedRunes(8) * 8; break;
                    case 9: value = Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f; break;
                    case 10: value = Game.PlayerStats.HasArchitectFee ? 1f : 0f; break;
                    case 11: value = Game.PlayerStats.TimesCastleBeaten * 50; break;
                }

                if (value > 0f)
                {
                    obj2.Text = "(" + EquipmentAbilityType.ShortDescription(ix + (ix > 9 ? 10 : 0), value) + ")";
                    obj1.Visible = true;
                    obj2.Visible = true;
                    obj1.Y = posY;
                    obj2.Y = posY;
                    posY += 20;
                }
                else
                {
                    obj1.Visible = false;
                    obj2.Visible = false;
                }

                ix++;
            }

            (this.m_backCard.GetChildAt(3) as TextObj).Text = Game.PlayerStats.PlayerName;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Draw(GameTime gametime)
        {
            this.m_playerHUD.SetPosition(new Vector2(this.m_frontCard.X + 46f, this.m_frontCard.Y + 64f));
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
            this.m_frontCard.Draw(base.Camera);
            this.m_backCard.Draw(base.Camera);
            this.m_cancelText.Draw(base.Camera);
            base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            if (!Game.PlayerStats.IsDead)
            {
                if (!this.m_playerInAir)
                {
                    ObjContainer mPlayerSprite = this.m_playerSprite;
                    Rectangle bounds = this.m_playerSprite.Bounds;
                    mPlayerSprite.Position = new Vector2(this.m_frontCard.X + 160f, this.m_frontCard.Y + 280f - ((float)bounds.Bottom - this.m_playerSprite.Y));
                }
                else
                {
                    this.m_playerSprite.Position = new Vector2(this.m_frontCard.X + 180f, this.m_frontCard.Y + 202f);
                }
                this.m_playerSprite.Draw(base.Camera);
                EffectParameter item = Game.ColourSwapShader.Parameters["desiredTint"];
                Color textureColor = this.m_playerSprite.GetChildAt(12).TextureColor;
                item.SetValue(textureColor.ToVector4());

                var args = RogueAPI.Game.Player.PipeSkinShaderArgs(this, this.m_playerSprite);
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(args.Target.Opacity);
                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(args.Target.ColorSwappedOut1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(args.Target.ColorSwappedIn1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(args.Target.ColorSwappedOut2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(args.Target.ColorSwappedIn2.ToVector4());

                //if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
                //{
                //    Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
                //    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_lichColour1.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_lichColour2.ToVector4());
                //}
                //else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
                //{
                //    Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
                //    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
                //}
                //else
                //{
                //    Game.ColourSwapShader.Parameters["Opacity"].SetValue(1);
                //    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_skinColour1.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
                //    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_skinColour2.ToVector4());
                //}
                base.Camera.End();
                base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader);
                this.m_playerSprite.GetChildAt(12).Draw(base.Camera);
                base.Camera.End();
                base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
                if (Game.PlayerStats.IsFemale)
                {
                    this.m_playerSprite.GetChildAt(13).Draw(base.Camera);
                }
                this.m_playerSprite.GetChildAt(15).Draw(base.Camera);
            }
            else
            {
                this.m_tombStoneSprite.Position = new Vector2(this.m_frontCard.X + 240f, this.m_frontCard.Y + 280f);
                this.m_tombStoneSprite.Draw(base.Camera);
            }
            this.m_spellIcon.Position = new Vector2(this.m_frontCard.X + 380f, this.m_frontCard.Y + 320f);
            this.m_spellIcon.Draw(base.Camera);
            base.Camera.End();
            base.Draw(gametime);
        }
    }


}
