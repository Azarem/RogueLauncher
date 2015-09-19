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
            base.Camera.GraphicsDevice.SetRenderTarget(this.m_skyRenderTarget);
            base.Camera.GraphicsDevice.Clear(Color.Black);
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            this.m_sky.Draw(base.Camera);
            base.Camera.End();
            base.Camera.GraphicsDevice.SetRenderTarget(this.m_backgroundRenderTarget);
            base.Camera.GraphicsDevice.Clear(Color.Black);
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            this.m_bg1.Draw(base.Camera);
            this.m_bg2.Draw(base.Camera);
            this.m_bg3.Draw(base.Camera);
            this.m_bgOutside.Draw(base.Camera);
            this.m_ground1.Draw(base.Camera);
            this.m_ground2.Draw(base.Camera);
            this.m_ground3.Draw(base.Camera);
            this.m_border1.Draw(base.Camera);
            this.m_border2.Draw(base.Camera);
            this.m_border3.Draw(base.Camera);
            this.m_manor.Draw(base.Camera);
            this.m_prop1.Draw(base.Camera);
            this.m_prop2.Draw(base.Camera);
            this.m_prop3.Draw(base.Camera);
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
            this.m_wifeSprite.Draw(base.Camera);
            this.m_childSprite1.Draw(base.Camera);
            this.m_childSprite2.Draw(base.Camera);
            this.m_sideBorderLeft.Draw(base.Camera);
            this.m_sideBorderRight.Draw(base.Camera);
            this.m_sideBorderTop.Draw(base.Camera);
            this.m_sideBorderBottom.Draw(base.Camera);
            this.m_teddy.Draw(base.Camera);
            this.m_kenny.Draw(base.Camera);
            this.m_glauber.Draw(base.Camera);
            this.m_gordon.Draw(base.Camera);
            this.m_judson.Draw(base.Camera);
            base.Camera.End();
            base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
            base.Camera.GraphicsDevice.Textures[1] = this.m_skyRenderTarget;
            base.Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ParallaxEffect);
            base.Camera.Draw(this.m_backgroundRenderTarget, Vector2.Zero, Color.White);
            base.Camera.End();
            base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Rectangle rectangle = new Rectangle(0, 0, 1320, 720);
            foreach (TextObj mCreditsTitleList in this.m_creditsTitleList)
            {
                if (!CollisionMath.Intersects(mCreditsTitleList.Bounds, rectangle))
                {
                    continue;
                }
                mCreditsTitleList.Draw(base.Camera);
            }
            foreach (TextObj mCreditsNameList in this.m_creditsNameList)
            {
                if (!CollisionMath.Intersects(mCreditsNameList.Bounds, rectangle))
                {
                    continue;
                }
                mCreditsNameList.Draw(base.Camera);
            }
            this.m_thanksForPlayingText.Draw(base.Camera);
            this.m_totalDeaths.Draw(base.Camera);
            this.m_totalPlayTime.Draw(base.Camera);
            this.m_continueText.Draw(base.Camera);
            base.Camera.End();
            base.Draw(gametime);
        }
    }
}
