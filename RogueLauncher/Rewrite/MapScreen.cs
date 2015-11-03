using AssemblyTranslator;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.MapScreen")]
    public class MapScreen : Screen
    {
        [Rewrite]
        private MapObj m_mapDisplay;
        [Rewrite]
        private bool m_isTeleporter;
        [Rewrite]
        public bool IsTeleporter { get; set; }
        [Rewrite]
        private KeyIconTextObj m_navigationText;
        [Rewrite]
        private SpriteObj[] m_teleporterList;
        [Rewrite]
        private KeyIconTextObj m_continueText;
        [Rewrite]
        private KeyIconTextObj m_recentreText;
        [Rewrite]
        private int m_selectedTeleporter;
        [Rewrite]
        private ObjContainer m_playerIcon;
        [Rewrite]
        private SpriteObj m_titleText;
        [Rewrite]
        private TextObj m_alzheimersQuestionMarks;
        [Rewrite]
        private ObjContainer m_legend;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void OnEnter()
        {
            SoundManager.PlaySound("Map_On");
            m_mapDisplay.CentreAroundPlayer();

            var args = new RogueAPI.MapEnterEventArgs(this, IsTeleporter, false);
            RogueAPI.Event<RogueAPI.MapEnterEventArgs>.Trigger(args);

            m_mapDisplay.DrawNothing = args.DrawNothing;

            //if (IsTeleporter || Game.PlayerStats.Traits.X != 11f && Game.PlayerStats.Traits.Y != 11f)
            //    m_mapDisplay.DrawNothing = false;
            //else
            //    m_mapDisplay.DrawNothing = true;

            m_continueText.Text = string.Concat("[Input:", (byte)9, "]  to close map");
            m_recentreText.Text = string.Concat("[Input:", (byte)0, "]  to center on player");

            if (InputManager.GamePadIsConnected(PlayerIndex.One))
                m_navigationText.Text = "[Button:LeftStick] to move map";
            else
                m_navigationText.Text = "Use arrow keys to move map";

            if (IsTeleporter && m_teleporterList.Length > 0)
            {
                SpriteObj mTeleporterList = m_teleporterList[m_selectedTeleporter];
                m_playerIcon.Position = new Vector2(mTeleporterList.X + 7f, mTeleporterList.Y - 20f);
                m_mapDisplay.CentreAroundTeleporter(m_selectedTeleporter, false);
            }

            base.OnEnter();
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            m_mapDisplay.DrawRenderTargets(Camera);
            Camera.End();

            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Camera.GraphicsDevice.SetRenderTarget(Game.ScreenManager.RenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Draw((ScreenManager as RCScreenManager).GetLevelScreen().RenderTarget, Vector2.Zero, Color.White * 0.3f);
            m_mapDisplay.Draw(Camera);
            if (IsTeleporter && m_teleporterList.Length > 0)
            {
                m_titleText.Draw(Camera);
                m_playerIcon.Position = m_teleporterList[m_selectedTeleporter].Position + new Vector2(14f, -20f);
                m_playerIcon.Draw(Camera);
            }
            if (!IsTeleporter)
            {
                m_recentreText.Draw(Camera);
                m_navigationText.Draw(Camera);
            }

            var args = new RogueAPI.MapDrawEventArgs(this, Camera, gameTime, IsTeleporter);
            RogueAPI.Event<RogueAPI.MapDrawEventArgs>.Trigger(args);

            //if (!IsTeleporter && (Game.PlayerStats.Traits.X == 11f || Game.PlayerStats.Traits.Y == 11f))
            //{
            //    m_alzheimersQuestionMarks.Draw(Camera);
            //}
            m_continueText.Draw(Camera);
            m_legend.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }
    }
}
