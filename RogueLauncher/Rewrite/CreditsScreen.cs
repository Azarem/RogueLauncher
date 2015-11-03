using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.CreditsScreen")]
    public class CreditsScreen : Screen
    {
        [Rewrite]
        private RenderTarget2D m_skyRenderTarget;
        [Rewrite]
        private SkyObj m_sky;
        [Rewrite]
        private RenderTarget2D m_backgroundRenderTarget;
        [Rewrite]
        private SpriteObj m_bg1;
        [Rewrite]
        private SpriteObj m_bg2;
        [Rewrite]
        private SpriteObj m_bg3;
        [Rewrite]
        private SpriteObj m_bgOutside;
        [Rewrite]
        private SpriteObj m_ground1;
        [Rewrite]
        private SpriteObj m_ground2;
        [Rewrite]
        private SpriteObj m_ground3;
        [Rewrite]
        private SpriteObj m_border1;
        [Rewrite]
        private SpriteObj m_border2;
        [Rewrite]
        private SpriteObj m_border3;
        [Rewrite]
        private SpriteObj m_prop1;
        [Rewrite]
        private ObjContainer m_prop2;
        [Rewrite]
        private ObjContainer m_prop3;
        [Rewrite]
        private ObjContainer m_playerSprite;
        [Rewrite]
        private ObjContainer m_wifeSprite;
        [Rewrite]
        private ObjContainer m_childSprite1;
        [Rewrite]
        private ObjContainer m_childSprite2;
        [Rewrite]
        private SpriteObj m_sideBorderLeft;
        [Rewrite]
        private SpriteObj m_sideBorderRight;
        [Rewrite]
        private SpriteObj m_sideBorderTop;
        [Rewrite]
        private SpriteObj m_sideBorderBottom;
        [Rewrite]
        private ObjContainer m_manor;
        [Rewrite]
        private SpriteObj m_glauber;
        [Rewrite]
        private SpriteObj m_teddy;
        [Rewrite]
        private SpriteObj m_kenny;
        [Rewrite]
        private SpriteObj m_gordon;
        [Rewrite]
        private SpriteObj m_judson;
        [Rewrite]
        private TextObj m_thanksForPlayingText;
        [Rewrite]
        private TextObj m_totalPlayTime;
        [Rewrite]
        private TextObj m_totalDeaths;
        [Rewrite]
        private KeyIconTextObj m_continueText;
        [Rewrite]
        private List<TextObj> m_creditsNameList;
        [Rewrite]
        private List<TextObj> m_creditsTitleList;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Draw(GameTime gametime)
        {
            base.Camera.GraphicsDevice.SetRenderTarget(m_skyRenderTarget);
            base.Camera.GraphicsDevice.Clear(Color.Black);
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            m_sky.Draw(base.Camera);
            base.Camera.End();
            base.Camera.GraphicsDevice.SetRenderTarget(m_backgroundRenderTarget);
            base.Camera.GraphicsDevice.Clear(Color.Black);
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            m_bg1.Draw(base.Camera);
            m_bg2.Draw(base.Camera);
            m_bg3.Draw(base.Camera);
            m_bgOutside.Draw(base.Camera);
            m_ground1.Draw(base.Camera);
            m_ground2.Draw(base.Camera);
            m_ground3.Draw(base.Camera);
            m_border1.Draw(base.Camera);
            m_border2.Draw(base.Camera);
            m_border3.Draw(base.Camera);
            m_manor.Draw(base.Camera);
            m_prop1.Draw(base.Camera);
            m_prop2.Draw(base.Camera);
            m_prop3.Draw(base.Camera);
            m_playerSprite.Draw(base.Camera);
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
            base.Camera.End();
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader);
            m_playerSprite.GetChildAt(12).Draw(base.Camera);
            base.Camera.End();
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            if (Game.PlayerStats.IsFemale)
            {
                m_playerSprite.GetChildAt(13).Draw(base.Camera);
            }
            m_playerSprite.GetChildAt(15).Draw(base.Camera);
            m_wifeSprite.Draw(base.Camera);
            m_childSprite1.Draw(base.Camera);
            m_childSprite2.Draw(base.Camera);
            m_sideBorderLeft.Draw(base.Camera);
            m_sideBorderRight.Draw(base.Camera);
            m_sideBorderTop.Draw(base.Camera);
            m_sideBorderBottom.Draw(base.Camera);
            m_teddy.Draw(base.Camera);
            m_kenny.Draw(base.Camera);
            m_glauber.Draw(base.Camera);
            m_gordon.Draw(base.Camera);
            m_judson.Draw(base.Camera);
            base.Camera.End();
            base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
            base.Camera.GraphicsDevice.Textures[1] = m_skyRenderTarget;
            base.Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ParallaxEffect);
            base.Camera.Draw(m_backgroundRenderTarget, Vector2.Zero, Color.White);
            base.Camera.End();
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Rectangle rectangle = new Rectangle(0, 0, 1320, 720);
            foreach (TextObj mCreditsTitleList in m_creditsTitleList)
            {
                if (!CollisionMath.Intersects(mCreditsTitleList.Bounds, rectangle))
                {
                    continue;
                }
                mCreditsTitleList.Draw(base.Camera);
            }
            foreach (TextObj mCreditsNameList in m_creditsNameList)
            {
                if (!CollisionMath.Intersects(mCreditsNameList.Bounds, rectangle))
                {
                    continue;
                }
                mCreditsNameList.Draw(base.Camera);
            }
            m_thanksForPlayingText.Draw(base.Camera);
            m_totalDeaths.Draw(base.Camera);
            m_totalPlayTime.Draw(base.Camera);
            m_continueText.Draw(base.Camera);
            base.Camera.End();
            base.Draw(gametime);
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


            //m_playerSprite.ChangeSprite("Player" + animationType + "_Character");
            //PlayerObj player = (base.ScreenManager as RCScreenManager).Player;

            //for (int i = 0; i < m_playerSprite.NumChildren; i++)
            //{
            //    m_playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
            //    m_playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
            //}

            //m_playerSprite.GetChildAt(16).Visible = false;
            //m_playerSprite.Scale = player.Scale;
            //if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
            //{
            //    m_playerSprite.GetChildAt(7).Visible = false;
            //}
            //m_playerSprite.GetChildAt(14).Visible = false;
            //if (Game.PlayerStats.SpecialItem == 8)
            //{
            //    m_playerSprite.GetChildAt(14).Visible = true;
            //}
            //if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
            //{
            //    m_playerSprite.GetChildAt(15).Visible = true;
            //    m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Shield_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
            //{
            //    m_playerSprite.GetChildAt(15).Visible = true;
            //    m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Lamp_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
            //{
            //    m_playerSprite.GetChildAt(15).Visible = true;
            //    m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Beard_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
            //{
            //    m_playerSprite.GetChildAt(15).Visible = true;
            //    m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Headband_Sprite"));
            //}
            //else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
            //{
            //    m_playerSprite.GetChildAt(15).Visible = true;
            //    m_playerSprite.GetChildAt(15).ChangeSprite(string.Concat("Player", animationType, "Horns_Sprite"));
            //}
            //else
            //{
            //    m_playerSprite.GetChildAt(15).Visible = false;
            //}
            //m_playerSprite.GetChildAt(0).Visible = false;
            //if (Game.PlayerStats.Class == 16)
            //{
            //    m_playerSprite.GetChildAt(0).Visible = true;
            //    GameObj childAt = m_playerSprite.GetChildAt(12);
            //    object[] objArray = new object[] { "Player", animationType, "Head", 6, "_Sprite" };
            //    childAt.ChangeSprite(string.Concat(objArray));
            //}
            //if (Game.PlayerStats.Class == 17)
            //{
            //    GameObj gameObj = m_playerSprite.GetChildAt(12);
            //    object[] objArray1 = new object[] { "Player", animationType, "Head", 7, "_Sprite" };
            //    gameObj.ChangeSprite(string.Concat(objArray1));
            //}
            //if (Game.PlayerStats.IsFemale)
            //{
            //    m_playerSprite.GetChildAt(5).Visible = true;
            //    m_playerSprite.GetChildAt(13).Visible = true;
            //}
            //else
            //{
            //    m_playerSprite.GetChildAt(5).Visible = false;
            //    m_playerSprite.GetChildAt(13).Visible = false;
            //}
            //if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
            //{
            //    m_playerSprite.Scale = new Vector2(3f, 3f);
            //}
            //if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
            //{
            //    m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
            //}
            //if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
            //{
            //    ObjContainer mPlayerSprite = m_playerSprite;
            //    mPlayerSprite.ScaleX = mPlayerSprite.ScaleX * 0.825f;
            //    ObjContainer scaleY = m_playerSprite;
            //    scaleY.ScaleY = scaleY.ScaleY * 1.25f;
            //}
            //if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
            //{
            //    ObjContainer scaleX = m_playerSprite;
            //    scaleX.ScaleX = scaleX.ScaleX * 1.25f;
            //    ObjContainer objContainer = m_playerSprite;
            //    objContainer.ScaleY = objContainer.ScaleY * 1.175f;
            //}
            //if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
            //{
            //    m_playerSprite.OutlineColour = Color.White;
            //    m_playerSprite.GetChildAt(10).Visible = false;
            //    m_playerSprite.GetChildAt(11).Visible = false;
            //}
            //else
            //{
            //    m_playerSprite.OutlineColour = Color.Black;
            //    m_playerSprite.GetChildAt(10).Visible = true;
            //    m_playerSprite.GetChildAt(11).Visible = true;
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

            m_playerSprite.PlayAnimation(true);
            m_playerSprite.CalculateBounds();
            m_playerSprite.Y = 435f - (m_playerSprite.Bounds.Bottom - m_playerSprite.Y);
        }
    }
}
