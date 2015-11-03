using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.RCScreenManager")]
    public class RCScreenManager : ScreenManager
    {
        [Rewrite]
        private PlayerObj m_player;
        [Rewrite]
        public PlayerObj Player { get { return null; } }
        [Rewrite]
        public RenderTarget2D RenderTarget { get { return null; } }

        [Rewrite]
        public RCScreenManager(Game Game) : base(Game) { }

        [Rewrite]
        public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null) { }
        [Rewrite]
        public void AddIconsToMap(List<RoomObj> roomList) { }

        [Rewrite(action: RewriteAction.Replace)]
        private void LoadPlayer()
        {
            if (m_player == null)
            {
                m_player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, (Game as Game).PhysicsManager, null, Game as Game)
                {
                    Position = new Vector2(200f, 200f)
                };
                m_player.Initialize();
                RogueAPI.Game.Player.Instance = new RogueAPI.Game.Player(m_player);
            }
        }

        [Rewrite]
        public ProceduralLevelScreen GetLevelScreen() { return null; }
    }
}
