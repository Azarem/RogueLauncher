using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;

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

            int ix = 0, count = m_dataList1.Count;
            while (ix < count)
            {
                obj1 = m_dataList1[ix];
                obj2 = m_dataList2[ix];

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
            int posY = (int)m_equipmentTitle.Y + 40;
            while (ix < count)
            {
                int id = getEquippedArray[ix];
                obj1 = m_equipmentList[ix];

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
            count = m_runeBackTitleList.Count;
            posY = (int)m_runesTitle.Y + 40;
            while (ix < count)
            {
                obj1 = m_runeBackTitleList[ix];
                obj2 = m_runeBackDescriptionList[ix];

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

            (m_backCard.GetChildAt(3) as TextObj).Text = Game.PlayerStats.PlayerName;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Draw(GameTime gametime)
        {
            m_playerHUD.SetPosition(new Vector2(m_frontCard.X + 46f, m_frontCard.Y + 64f));
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            m_frontCard.Draw(Camera);
            m_backCard.Draw(Camera);
            m_cancelText.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            if (!Game.PlayerStats.IsDead)
            {
                if (!m_playerInAir)
                {
                    ObjContainer mPlayerSprite = m_playerSprite;
                    Rectangle bounds = m_playerSprite.Bounds;
                    mPlayerSprite.Position = new Vector2(m_frontCard.X + 160f, m_frontCard.Y + 280f - ((float)bounds.Bottom - m_playerSprite.Y));
                }
                else
                {
                    m_playerSprite.Position = new Vector2(m_frontCard.X + 180f, m_frontCard.Y + 202f);
                }
                m_playerSprite.Draw(Camera);
                EffectParameter item = Game.ColourSwapShader.Parameters["desiredTint"];
                Color textureColor = m_playerSprite.GetChildAt(12).TextureColor;
                item.SetValue(textureColor.ToVector4());

                var args = RogueAPI.Game.Player.PipeSkinShaderArgs(this, m_playerSprite);
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
                Camera.End();
                Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader);
                m_playerSprite.GetChildAt(12).Draw(Camera);
                Camera.End();
                Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
                if (Game.PlayerStats.IsFemale)
                {
                    m_playerSprite.GetChildAt(13).Draw(Camera);
                }
                m_playerSprite.GetChildAt(15).Draw(Camera);
            }
            else
            {
                m_tombStoneSprite.Position = new Vector2(m_frontCard.X + 240f, m_frontCard.Y + 280f);
                m_tombStoneSprite.Draw(Camera);
            }
            m_spellIcon.Position = new Vector2(m_frontCard.X + 380f, m_frontCard.Y + 320f);
            m_spellIcon.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        private void ChangeParts(PlayerObj player)
        {
            string[] strArrays = (m_playerInAir = CDGMath.RandomInt(0, 1) == 0)
                ? new[] { "Jumping", "Falling", "AirAttack" }
                : new[] { "Idle", "Walking", "LevelUp", "Dash", "FrontDash", "Attacking3" };

            int max = strArrays.Length - 1;

            if (Game.PlayerStats.Class == 16)
                max--;

            string animationType = strArrays[CDGMath.RandomInt(0, max)];

            //SetPlayerStyle(strArrays[CDGMath.RandomInt(0, strArrays.Length - 1)]);

            m_playerSprite.ChangeSprite("Player" + animationType + "_Character");
            m_playerSprite.Scale = player.Scale;
            m_playerSprite.OutlineColour = player.OutlineColour;

            for (int i = 0; i < m_playerSprite.NumChildren; i++)
            {
                var src = player.GetChildAt(i);
                var dst = m_playerSprite.GetChildAt(i);

                dst.TextureColor = src.TextureColor;
                dst.Visible = src.Visible;
            }

            m_playerSprite.CalculateBounds();
            m_playerSprite.Y = 435f - (m_playerSprite.Bounds.Bottom - m_playerSprite.Y);

            //for (int i = 0; i < player.NumChildren; i++)
            //{
            //    SpriteObj childAt = player.GetChildAt(i) as SpriteObj;
            //    SpriteObj textureColor = m_playerSprite.GetChildAt(i) as SpriteObj;
            //    textureColor.TextureColor = childAt.TextureColor;
            //}


            //string spriteName = (m_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
            //int num = spriteName.IndexOf("_") - 1;
            //spriteName = spriteName.Remove(num, 1);
            //if (Game.PlayerStats.Class != 16)
            //{
            //    spriteName = (Game.PlayerStats.Class != 17 ? spriteName.Replace("_", string.Concat(Game.PlayerStats.HeadPiece, "_")) : spriteName.Replace("_", string.Concat(7, "_")));
            //}
            //else
            //{
            //    spriteName = spriteName.Replace("_", string.Concat(6, "_"));
            //}
            //m_playerSprite.GetChildAt(12).ChangeSprite(spriteName);

            //string str = (m_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
            //num = str.IndexOf("_") - 1;
            //str = str.Remove(num, 1);
            //str = str.Replace("_", string.Concat(Game.PlayerStats.ChestPiece, "_"));
            //m_playerSprite.GetChildAt(4).ChangeSprite(str);

            //string spriteName1 = (m_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
            //num = spriteName1.IndexOf("_") - 1;
            //spriteName1 = spriteName1.Remove(num, 1);
            //spriteName1 = spriteName1.Replace("_", string.Concat(Game.PlayerStats.ShoulderPiece, "_"));
            //m_playerSprite.GetChildAt(9).ChangeSprite(spriteName1);

            //string str1 = (m_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
            //num = str1.IndexOf("_") - 1;
            //str1 = str1.Remove(num, 1);
            //str1 = str1.Replace("_", string.Concat(Game.PlayerStats.ShoulderPiece, "_"));
            //m_playerSprite.GetChildAt(3).ChangeSprite(str1);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void SetPlayerStyle(string animationType)
        {
            m_playerSprite.ChangeSprite("Player" + animationType + "_Character");
            PlayerObj player = (ScreenManager as RCScreenManager).Player;

            for (int i = 0; i < m_playerSprite.NumChildren; i++)
            {
                var src = player.GetChildAt(i);
                var dst = m_playerSprite.GetChildAt(i);

                dst.TextureColor = src.TextureColor;
                dst.Visible = src.Visible;
            }
            m_playerSprite.Scale = player.Scale;
            m_playerSprite.OutlineColour = player.OutlineColour;

            //this.m_playerSprite.ChangeSprite(string.Concat("Player", animationType, "_Character"));
            //PlayerObj player = (base.ScreenManager as RCScreenManager).Player;
            //for (int i = 0; i < this.m_playerSprite.NumChildren; i++)
            //{
            //    this.m_playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
            //    this.m_playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
            //}
            //this.m_playerSprite.GetChildAt(16).Visible = false;
            //this.m_playerSprite.Scale = player.Scale;
            //if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
            //{
            //    this.m_playerSprite.GetChildAt(7).Visible = false;
            //}
            //this.m_playerSprite.GetChildAt(14).Visible = false;
            //if (Game.PlayerStats.SpecialItem == 8)
            //{
            //    this.m_playerSprite.GetChildAt(14).Visible = true;
            //}
            //if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
            //{
            //    this.m_playerSprite.GetChildAt(15).Visible = true;
            //    this.m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Shield_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
            //{
            //    this.m_playerSprite.GetChildAt(15).Visible = true;
            //    this.m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Lamp_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
            //{
            //    this.m_playerSprite.GetChildAt(15).Visible = true;
            //    this.m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Beard_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
            //{
            //    this.m_playerSprite.GetChildAt(15).Visible = true;
            //    this.m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Headband_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
            //{
            //    this.m_playerSprite.GetChildAt(15).Visible = true;
            //    this.m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Horns_Sprite"));
            //}
            //else
            //{
            //    this.m_playerSprite.GetChildAt(15).Visible = false;
            //}
            //this.m_playerSprite.GetChildAt(0).Visible = false;
            //if (Game.PlayerStats.Class == 16)
            //{
            //    this.m_playerSprite.GetChildAt(0).Visible = true;
            //    GameObj childAt = this.m_playerSprite.GetChildAt(12);
            //    object[] objArray = new object[] { "Player", animationType, "Head", 6, "_Sprite" };
            //    childAt.ChangeSprite(string.Concat(objArray));
            //}
            //if (Game.PlayerStats.IsFemale)
            //{
            //    this.m_playerSprite.GetChildAt(5).Visible = true;
            //    this.m_playerSprite.GetChildAt(13).Visible = true;
            //}
            //else
            //{
            //    this.m_playerSprite.GetChildAt(5).Visible = false;
            //    this.m_playerSprite.GetChildAt(13).Visible = false;
            //}
            //if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
            //{
            //    this.m_playerSprite.Scale = new Vector2(3f, 3f);
            //}
            //if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
            //{
            //    this.m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
            //}
            //if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
            //{
            //    ObjContainer mPlayerSprite = this.m_playerSprite;
            //    mPlayerSprite.ScaleX = mPlayerSprite.ScaleX * 0.825f;
            //    ObjContainer scaleY = this.m_playerSprite;
            //    scaleY.ScaleY = scaleY.ScaleY * 1.25f;
            //}
            //if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
            //{
            //    ObjContainer scaleX = this.m_playerSprite;
            //    scaleX.ScaleX = scaleX.ScaleX * 1.25f;
            //    ObjContainer objContainer = this.m_playerSprite;
            //    objContainer.ScaleY = objContainer.ScaleY * 1.175f;
            //}
            //if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
            //{
            //    this.m_playerSprite.OutlineColour = Color.White;
            //}
            //else
            //{
            //    this.m_playerSprite.OutlineColour = Color.Black;
            //}

            m_playerSprite.CalculateBounds();
            m_playerSprite.Y = 435f - (m_playerSprite.Bounds.Bottom - m_playerSprite.Y);
        }
    }


}
