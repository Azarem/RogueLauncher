using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private void LoadPlayer()
        {
            if (this.m_player == null)
            {
                this.m_player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, (base.Game as Game).PhysicsManager, null, base.Game as Game)
                {
                    Position = new Vector2(200f, 200f)
                };
                this.m_player.Initialize();
                RogueAPI.Game.Player.Instance = this.m_player;
            }
        }
    }
}
