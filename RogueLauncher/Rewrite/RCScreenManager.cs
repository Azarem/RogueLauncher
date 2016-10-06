using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.RCScreenManager")]
    public class RCScreenManager : ScreenManager
    {
        //[Rewrite]
        //private GameOverScreen m_gameOverScreen;
        //[Rewrite]
        //private RogueCastle.SkillScreen m_traitScreen;
        [Rewrite]
        private EnchantressScreen m_enchantressScreen;
        [Rewrite]
        private BlacksmithScreen m_blacksmithScreen;
        [Rewrite]
        private GetItemScreen m_getItemScreen;
        //[Rewrite]
        //private RogueCastle.DialogueScreen m_dialogueScreen;
        [Rewrite]
        private MapScreen m_mapScreen;
        [Rewrite]
        private PauseScreen m_pauseScreen;
        [Rewrite]
        private OptionsScreen m_optionsScreen;
        [Rewrite]
        private ProfileCardScreen m_profileCardScreen;
        [Rewrite]
        private CreditsScreen m_creditsScreen;
        //[Rewrite]
        //private SkillUnlockScreen m_skillUnlockScreen;
        //[Rewrite]
        //private DiaryEntryScreen m_diaryEntryScreen;
        //[Rewrite]
        //private DeathDefiedScreen m_deathDefyScreen;
        //[Rewrite]
        //private DiaryFlashbackScreen m_flashbackScreen;
        //[Rewrite]
        //private TextScreen m_textScreen;
        //[Rewrite]
        //private GameOverBossScreen m_gameOverBossScreen;
        //[Rewrite]
        //private ProfileSelectScreen m_profileSelectScreen;
        //[Rewrite]
        //private VirtualScreen m_virtualScreen;
        [Rewrite]
        private bool m_isTransitioning;
        [Rewrite]
        private bool m_inventoryVisible;
        [Rewrite]
        private PlayerObj m_player;
        [Rewrite]
        private SpriteObj m_blackTransitionIn;
        [Rewrite]
        private SpriteObj m_blackScreen;
        [Rewrite]
        private SpriteObj m_blackTransitionOut;
        [Rewrite]
        private bool m_isWipeTransitioning;


        [Rewrite]
        public PlayerObj Player { get; }
        [Rewrite]
        public RenderTarget2D RenderTarget { get; }
        [Rewrite]
        public DialogueScreen DialogueScreen { get; }


        [Rewrite]
        public RCScreenManager(Game Game) : base(Game) { }


        [Rewrite]
        public void ActivateMapScreenTeleporter() { }

        [Rewrite]
        public void AddIconsToMap(List<RoomObj> roomList) { }

        [Rewrite]
        public void AddRoomsToMap(List<RoomObj> roomList) { }

        [Rewrite]
        public void AttachMap(ProceduralLevelScreen level) { }

        [Rewrite]
        public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null) { }

        [Rewrite]
        public override void Draw(GameTime gameTime) { }

        [Rewrite]
        public void EndWipeTransition() { }

        [Rewrite]
        public void ForceResolutionChangeCheck() { }

        [Rewrite]
        public ProceduralLevelScreen GetLevelScreen() { return null; }

        [Rewrite]
        public void HideCurrentScreen() { }

        [Rewrite]
        public override void Initialize() { }

        [Rewrite]
        public void InitializeScreens() { }

        [Rewrite]
        public override void LoadContent() { }

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
        private void LoadScreen(byte screenType, bool wipeTransition) { }

        [Rewrite]
        public void PauseGame(object sender, EventArgs e) { }

        [Rewrite]
        public void RefreshMapScreenChestIcons(RoomObj room) { }

        [Rewrite]
        public void ReinitializeCamera(GraphicsDevice graphicsDevice) { }

        [Rewrite]
        public void ReinitializeContent(object sender, EventArgs e) { }

        [Rewrite]
        public override void RemoveScreen(DS2DEngine.Screen screen, bool disposeScreen) { }

        [Rewrite]
        public void StartWipeTransition() { }

        [Rewrite]
        public override void Update(GameTime gameTime) { }

        [Rewrite]
        public void UpdatePauseScreenIcons() { }

        [Rewrite]
        private void Window_ClientSizeChanged(object sender, EventArgs e) { }
    }
}
