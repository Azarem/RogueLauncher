using AssemblyTranslator;
using DS2DEngine;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.PauseScreen")]
    public class PauseScreen : Screen
    {
        [Rewrite]
        private SpriteObj m_titleText;
        //[Rewrite]
        //private List<PauseScreen.PauseInfoObj> m_infoObjList;
        [Rewrite]
        private SpriteObj m_profileCard;
        [Rewrite]
        private SpriteObj m_optionsIcon;
        [Rewrite]
        private KeyIconTextObj m_profileCardKey;
        [Rewrite]
        private KeyIconTextObj m_optionsKey;
        [Rewrite]
        private float m_inputDelay;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void HandleInput()
        {
            if (m_inputDelay <= 0f)
            {
                var manager = ScreenManager as RCScreenManager;

                if (InputManager.IsNewlyPressed(InputFlags.MenuProfileCard) && Game.PlayerStats.TutorialComplete)
                    manager.DisplayScreen(17, true, null);

                if (InputManager.IsNewlyPressed(InputFlags.MenuOptions))
                    manager.DisplayScreen(4, false, new List<object>() { false });

                if (InputManager.IsNewlyPressed(InputFlags.MenuPause))
                {
                    manager.GetLevelScreen().UnpauseScreen();
                    manager.HideCurrentScreen();
                }

                base.HandleInput();
            }
        }
    }
}
