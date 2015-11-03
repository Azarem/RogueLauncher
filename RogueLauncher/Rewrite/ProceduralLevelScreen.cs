using AssemblyTranslator;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProceduralLevelScreen")]
    public class ProceduralLevelScreen : Screen
    {
        [Rewrite]
        private PlayerObj m_player;
        [Rewrite]
        protected MapObj m_miniMapDisplay;
        [Rewrite]
        private List<RoomObj> m_roomList;
        [Rewrite]
        private RenderTarget2D m_bgRenderTarget;
        [Rewrite]
        private BackgroundObj m_backgroundSprite;
        [Rewrite]
        private InputMap m_inputMap;
        [Rewrite]
        private RenderTarget2D m_finalRenderTarget;
        [Rewrite]
        private bool m_enemiesPaused;
        [Rewrite]
        private RenderTarget2D m_skyRenderTarget;
        [Rewrite]
        private RenderTarget2D m_fgRenderTarget;
        [Rewrite]
        protected RoomObj m_currentRoom;
        [Rewrite]
        protected ProjectileManager m_projectileManager;
        [Rewrite]
        private RenderTarget2D m_traitAuraRenderTarget;
        [Rewrite]
        public TextObj DebugTextObj;
        [Rewrite]
        protected ItemDropManager m_itemDropManager;
        [Rewrite]
        private ImpactEffectPool m_impactEffectPool;
        [Rewrite]
        protected TextManager m_textManager;
        [Rewrite]
        private BackgroundObj m_gardenParallaxFG;
        [Rewrite]
        private SpriteObj m_whiteBG;
        [Rewrite]
        private RenderTarget2D m_lightSourceRenderTarget;
        [Rewrite]
        private RenderTarget2D m_shadowRenderTarget;
        [Rewrite]
        private ProjectileIconPool m_projectileIconPool;
        [Rewrite]
        private PlayerHUDObj m_playerHUD;
        [Rewrite]
        private EnemyHUDObj m_enemyHUD;
        [Rewrite]
        private EnemyObj m_lastEnemyHit;
        [Rewrite]
        private float m_enemyHUDCounter;
        [Rewrite]
        private SpriteObj m_mapBG;
        [Rewrite]
        private SpriteObj m_compassBG;
        [Rewrite]
        private SpriteObj m_compass;
        [Rewrite]
        private ObjContainer m_objectivePlate;
        [Rewrite]
        private TextObj m_roomEnteringTitle;
        [Rewrite]
        private TextObj m_roomTitle;
        [Rewrite]
        private SpriteObj m_filmGrain;
        [Rewrite]
        private SpriteObj m_blackBorder1;
        [Rewrite]
        private SpriteObj m_blackBorder2;
        [Rewrite]
        private TextObj m_creditsText;
        [Rewrite]
        private TextObj m_creditsTitleText;

        [Rewrite]
        public PlayerObj Player { get; set; }
        [Rewrite]
        public RoomObj CurrentRoom { get { return null; } }
        [Rewrite]
        public TextManager TextManager { get { return null; } }
        [Rewrite]
        public ProjectileManager ProjectileManager { get { return null; } }
        [Rewrite]
        public ImpactEffectPool ImpactEffectPool { get { return null; } }
        [Rewrite]
        public ItemDropManager ItemDropManager { get { return null; } }
        [Rewrite]
        public float ShoutMagnitude { get; set; }
        [Rewrite]
        public List<RoomObj> RoomList { get { return null; } }
        [Rewrite]
        public float BackBufferOpacity { get; set; }
        [Rewrite]
        public GameTypes.LevelType CurrentLevelType { get { return 0; } }
        [Rewrite]
        public RenderTarget2D RenderTarget { get { return null; } }
        [Rewrite]
        public PhysicsManager PhysicsManager { get { return null; } }

        [Rewrite]
        public void UpdatePlayerSpellIcon() { }
        [Rewrite]
        public void CastTimeStop(float duration) { }
        [Rewrite]
        public void StopTimeStop() { }
        [Rewrite]
        public void AddEnemyToCurrentRoom(EnemyObj enemy) { }
        [Rewrite]
        public void DisplayMap(bool isTeleporterScreen) { }
        [Rewrite]
        private void CheckForRoomTransition() { }
        [Rewrite]
        public void UpdateCamera() { }
        [Rewrite]
        public void UpdatePlayerHUDAbilities() { }
        [Rewrite]
        public void DrawRenderTargets() { }
        
        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void OnEnter()
        {
            (ScreenManager.Game as Game).SaveManager.ResetAutosave();
            m_player.DisableAllWeight = false;
            m_player.StopAllSpells();
            m_player.Scale = new Vector2(2f, 2f);
            ShoutMagnitude = 3f;

            //if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
            //    m_player.Scale *= 1.5f;
            //else if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
            //    m_player.Scale = new Vector2(1.35f, 1.35f);

            //if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
            //{
            //    m_player.ScaleX *= 0.825f;
            //    m_player.ScaleY *= 1.15f;
            //}
            //else if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
            //{
            //    m_player.ScaleX *= 1.25f;
            //    m_player.ScaleY *= 1.175f;
            //}

            m_player.CurrentHealth = Game.PlayerStats.CurrentHealth;
            m_player.CurrentMana = Game.PlayerStats.CurrentMana;
            if (LevelEV.RUN_TESTROOM)
            {
                Game.ScreenManager.Player.CurrentHealth = Game.ScreenManager.Player.MaxHealth;
                Game.ScreenManager.Player.CurrentMana = Game.ScreenManager.Player.MaxMana;
            }

            BuildRenderChain();

            var args = new RogueAPI.LevelEnterEventArgs(this, m_player, _renderChain);
            RogueAPI.Event<RogueAPI.LevelEnterEventArgs>.Trigger(args);

            m_player.UpdateInternalScale();
            CheckForRoomTransition();
            UpdateCamera();
            UpdatePlayerHUDAbilities();
            m_player.UpdateEquipmentColours();
            m_player.StopAllSpells();

            if (Game.PlayerStats.Class == 13)
            {
                m_miniMapDisplay.AddAllIcons(RoomList);
                (ScreenManager as RCScreenManager).AddIconsToMap(RoomList);
            }

            if (Game.PlayerStats.EyeballBossBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_EYES");

            if (Game.PlayerStats.FairyBossBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_GHOSTS");

            if (Game.PlayerStats.BlobBossBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_SLIME");

            if (Game.PlayerStats.FireballBossBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_FIRE");

            if (Game.PlayerStats.LastbossBeaten || Game.PlayerStats.TimesCastleBeaten > 0)
                GameUtil.UnlockAchievement("FEAR_OF_FATHERS");

            if (Game.PlayerStats.TimesCastleBeaten > 1)
                GameUtil.UnlockAchievement("FEAR_OF_TWINS");

            if (Game.PlayerStats.ChallengeEyeballBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_BLINDNESS");

            if (Game.PlayerStats.ChallengeSkullBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_BONES");

            if (Game.PlayerStats.ChallengeFireballBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_CHEMICALS");

            if (Game.PlayerStats.ChallengeBlobBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_SPACE");

            if (Game.PlayerStats.ChallengeLastBossBeaten)
                GameUtil.UnlockAchievement("FEAR_OF_RELATIVES");

            if (Game.PlayerStats.EnemiesKilledList[5].W > 0f &&
                Game.PlayerStats.EnemiesKilledList[12].W > 0f &&
                Game.PlayerStats.EnemiesKilledList[15].W > 0f &&
                Game.PlayerStats.EnemiesKilledList[22].W > 0f &&
                Game.PlayerStats.EnemiesKilledList[32].W > 0f)
                GameUtil.UnlockAchievement("FEAR_OF_ANIMALS");

            if (Game.PlayerStats.TotalHoursPlayed + Game.PlaySessionLength >= 20f)
                GameUtil.UnlockAchievement("FEAR_OF_SLEEP");

            if (Game.PlayerStats.TotalRunesFound > 10)
                GameUtil.UnlockAchievement("LOVE_OF_MAGIC");


            base.OnEnter();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void OnExit()
        {
            if (m_currentRoom != null)
                m_currentRoom.OnExit();

            SoundManager.StopAllSounds("Default");
            SoundManager.StopAllSounds("Pauseable");

            var args = new RogueAPI.LevelExitEventArgs(this, m_player);
            RogueAPI.Event<RogueAPI.LevelExitEventArgs>.Trigger(args);

            base.OnExit();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep1(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            m_backgroundSprite.Draw(camera);
            if (CurrentRoom == null || camera.Zoom != 1f || m_inputMap.Pressed(2) && (!m_inputMap.Pressed(2) || !LevelEV.RUN_DEMO_VERSION && !LevelEV.CREATE_RETAIL_VERSION))
            {
                foreach (RoomObj mRoomList in m_roomList)
                    mRoomList.DrawBGObjs(camera);
            }
            else
                CurrentRoom.DrawBGObjs(camera);
        }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        //public void DrawStep2Pre(Camera2D camera)
        //{
        //    camera.GraphicsDevice.Textures[1] = m_skyRenderTarget;
        //    camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
        //}

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep2(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            if (!m_enemiesPaused)
                camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep3(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            camera.Draw(CurrentRoom.BGRender, camera.TopLeftCorner, Color.White);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep4(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            CurrentRoom.Draw(camera);

            if (LevelEV.SHOW_ENEMY_RADII)
                foreach (var enemyList in m_currentRoom.EnemyList)
                    enemyList.DrawDetectionRadii(camera);

            m_projectileManager.Draw(camera);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep5(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            camera.Draw(m_finalRenderTarget, Vector2.Zero, Color.White);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep6(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep7(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            camera.Draw(Game.GenericTexture, new Rectangle((int)camera.TopLeftCorner.X, (int)camera.TopLeftCorner.Y, 1320, 720), Color.Black * BackBufferOpacity);

            if (!m_player.IsKilled)
                m_player.Draw(camera);

            if (!LevelEV.CREATE_RETAIL_VERSION)
            {
                DebugTextObj.Position = new Vector2(camera.X, camera.Y - 300f);
                DebugTextObj.Draw(camera);
            }

            m_itemDropManager.Draw(camera);
            m_impactEffectPool.Draw(camera);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep8(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            m_textManager.Draw(camera);

            if (CurrentRoom.LevelType == GameTypes.LevelType.TOWER)
                m_gardenParallaxFG.Draw(camera);

            m_whiteBG.Draw(camera);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep9(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            if (!LevelEV.SAVE_FRAMES)
                camera.Draw(m_shadowRenderTarget, Vector2.Zero, Color.White);
            else
                camera.Draw(m_shadowRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), SpriteEffects.None, 1f);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep10(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            m_projectileIconPool.Draw(camera);
            m_playerHUD.Draw(camera);

            if (m_lastEnemyHit != null && m_enemyHUDCounter > 0f)
                m_enemyHUD.Draw(camera);

            if (m_enemyHUDCounter > 0f)
            {
                ProceduralLevelScreen mEnemyHUDCounter = this;
                mEnemyHUDCounter.m_enemyHUDCounter = mEnemyHUDCounter.m_enemyHUDCounter - (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (CurrentRoom.Name != "Start" && CurrentRoom.Name != "Boss" && CurrentRoom.Name != "ChallengeBoss" && m_miniMapDisplay.Visible)
            {
                m_mapBG.Draw(camera);
                m_miniMapDisplay.Draw(camera);
            }

            if (CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Ending")
            {
                m_compassBG.Draw(camera);
                m_compass.Draw(camera);
            }

            m_objectivePlate.Draw(camera);
            m_roomEnteringTitle.Draw(camera);
            m_roomTitle.Draw(camera);

            if (CurrentRoom.Name != "Ending" && (!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
                m_filmGrain.Draw(camera);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void DrawStep11(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            m_blackBorder1.Draw(camera);
            m_blackBorder2.Draw(camera);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void PrepareHUD(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            if (CurrentRoom.Name != "Ending")
            {
                if ((Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f) && Game.PlayerStats.SpecialItem != 8)
                {
                    Game.GaussianBlur.InvertMask = true;
                    Game.GaussianBlur.Draw(m_finalRenderTarget, camera, m_traitAuraRenderTarget);
                }
                else if ((Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f) && Game.PlayerStats.SpecialItem != 8)
                {
                    Game.GaussianBlur.InvertMask = false;
                    Game.GaussianBlur.Draw(m_finalRenderTarget, camera, m_traitAuraRenderTarget);
                }
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void SetCameraTransform(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            step.TransformMatrix = camera.GetTransformation();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void PreparePausedEnemies(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            if (!(step.Skip = !m_enemiesPaused))
            {
                if (step.Effect == Game.HSVEffect)
                {
                    step.Effect.Parameters["Saturation"].SetValue(0);
                    step.Effect.Parameters["UseMask"].SetValue(true);
                }
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void PrepareShadowEffect(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            step.Skip = !((CurrentLevelType == GameTypes.LevelType.DUNGEON || Game.PlayerStats.Traits.X == 35f || Game.PlayerStats.Traits.Y == 35f) && (Game.PlayerStats.Class != 13 || Game.PlayerStats.Class == 13 && !Player.LightOn));
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void PrepareClearColor(Camera2D camera, RenderStep step, GameTime gameTime)
        {
            step.ClearColor = m_enemiesPaused ? Color.White : Color.Black;
        }

        [Rewrite(action: RewriteAction.Add)]
        private RogueAPI.ObjectChain<LevelRenderStep, RenderStep> _renderChain;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public void BuildRenderChain()
        {
            _renderChain = new RogueAPI.ObjectChain<LevelRenderStep, RenderStep>();

            _renderChain.Push(LevelRenderStep.RoomBgToBack, new RenderStep(m_bgRenderTarget, samplerState: SamplerState.PointWrap, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { SetCameraTransform }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep1 }));
            _renderChain.Push(LevelRenderStep.SkyBackToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.LinearClamp, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { PrepareClearColor }, effect: Game.ParallaxEffect, shaderSteps: new[] { new RogueAPI.Game.ShaderStep(1, m_skyRenderTarget, SamplerState.LinearClamp) }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep2 }));
            _renderChain.Push(LevelRenderStep.RoomBgToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone, effect: Game.BWMaskEffect, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { SetCameraTransform }, shaderSteps: new[] { new RogueAPI.Game.ShaderStep(1, m_fgRenderTarget, SamplerState.PointClamp) }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep3 }));
            _renderChain.Push(LevelRenderStep.RoomFgToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.PointWrap, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { SetCameraTransform }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep4 }));
            _renderChain.Push(LevelRenderStep.InvertFrontToBack, new RenderStep(m_bgRenderTarget, samplerState: SamplerState.LinearClamp, effect: Game.InvertShader, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { PreparePausedEnemies }, shaderSteps: new[] { new RogueAPI.Game.ShaderStep(1, m_traitAuraRenderTarget) }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep5 }));
            _renderChain.Push(LevelRenderStep.SaturateBackToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.LinearClamp, effect: Game.HSVEffect, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { PreparePausedEnemies }, shaderSteps: new[] { new RogueAPI.Game.ShaderStep(1, m_traitAuraRenderTarget) }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep6 }));
            _renderChain.Push(LevelRenderStep.PlayerObjectsToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.PointClamp, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { SetCameraTransform }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep7 }));
            _renderChain.Push(LevelRenderStep.TextToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.LinearWrap, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { SetCameraTransform }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep8 }));
            _renderChain.Push(LevelRenderStep.DarknessToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.LinearClamp, effect: Game.ShadowEffect, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { PrepareShadowEffect }, shaderSteps: new[] { new RogueAPI.Game.ShaderStep(1, m_lightSourceRenderTarget, SamplerState.LinearClamp) }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep9 }));
            _renderChain.Push(LevelRenderStep.HUDToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.LinearWrap, preSteps: new Action<Camera2D, RenderStep, GameTime>[] { PrepareHUD }, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep10 }));
            _renderChain.Push(LevelRenderStep.BorderToFront, new RenderStep(m_finalRenderTarget, samplerState: SamplerState.PointClamp, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep11 }));
            _renderChain.Push(LevelRenderStep.ExtraFrontToBack, new RenderStep(m_bgRenderTarget, samplerState: SamplerState.LinearClamp, drawSteps: new Action<Camera2D, RenderStep, GameTime>[] { DrawStep5 }));
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Draw(GameTime gameTime)
        {
            DrawRenderTargets();

            var step = _renderChain.First;
            while (step != null)
            {
                step.Value.Draw(Camera, gameTime);
                step = step.Next;
            }

            //List<RogueAPI.Game.RenderStep> steps = new List<RogueAPI.Game.RenderStep>(new[] {
            //    new RogueAPI.Game.RenderStep(m_bgRenderTarget, samplerState: SamplerState.PointWrap, transformMatrix: Camera.GetTransformation(), drawSteps: new Action<Camera2D>[] { DrawStep1 }),
            //    new RogueAPI.Game.RenderStep(m_finalRenderTarget, samplerState: SamplerState.LinearClamp, clearColor: m_enemiesPaused ? Color.White : Color.Black, effect: Game.ParallaxEffect, shaderSteps: new [] { new RogueAPI.Game.ShaderStep(1, m_skyRenderTarget, SamplerState.LinearClamp) }, drawSteps: new Action<Camera2D>[] { DrawStep2 }),
            //    new RogueAPI.Game.RenderStep(m_finalRenderTarget, samplerState: SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone, effect: Game.BWMaskEffect, transformMatrix: Camera.GetTransformation(), shaderSteps: new [] { new RogueAPI.Game.ShaderStep(1, m_fgRenderTarget, SamplerState.PointClamp) }, drawSteps: new Action<Camera2D>[] { DrawStep3 })
            //});

            //foreach (var step in steps)
            //    step.Draw(Camera);

            //STEP1 - Draw background textures to BG

            //Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
            //Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.GetTransformation());
            //m_backgroundSprite.Draw(Camera);
            //if (CurrentRoom == null || Camera.Zoom != 1f || m_inputMap.Pressed(2) && (!m_inputMap.Pressed(2) || !LevelEV.RUN_DEMO_VERSION && !LevelEV.CREATE_RETAIL_VERSION))
            //{
            //    foreach (RoomObj mRoomList in m_roomList)
            //        mRoomList.DrawBGObjs(Camera);
            //}
            //else
            //    CurrentRoom.DrawBGObjs(Camera);

            //Camera.End();

            //STEP2 - Clear background, draw sky
            //Camera.GraphicsDevice.SetRenderTarget(m_finalRenderTarget);
            //Camera.GraphicsDevice.Clear(Color.Black);

            //if (m_enemiesPaused)
            //    Camera.GraphicsDevice.Clear(Color.White);

            //Camera.GraphicsDevice.Textures[1] = m_skyRenderTarget;
            //Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            //Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.ParallaxEffect);

            //if (!m_enemiesPaused)
            //    Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);

            //Camera.End();

            //STEP3 - Draw room BG
            //Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, Game.BWMaskEffect, Camera.GetTransformation());
            //Camera.GraphicsDevice.Textures[1] = m_fgRenderTarget;
            //Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            //Camera.Draw(CurrentRoom.BGRender, Camera.TopLeftCorner, Color.White);
            //Camera.End();

            //if (LevelEV.SHOW_ENEMY_RADII)
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.GetTransformation());
            //else
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.GetTransformation());

            //CurrentRoom.Draw(Camera);
            //if (LevelEV.SHOW_ENEMY_RADII)
            //{
            //    foreach (EnemyObj enemyList in m_currentRoom.EnemyList)
            //        enemyList.DrawDetectionRadii(Camera);
            //}

            //m_projectileManager.Draw(Camera);
            //Camera.End();

            //if (m_enemiesPaused)
            //{
            //    Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
            //    Camera.GraphicsDevice.Textures[1] = m_traitAuraRenderTarget;
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.InvertShader);
            //    Camera.Draw(m_finalRenderTarget, Vector2.Zero, Color.White);
            //    Camera.End();

            //    Game.HSVEffect.Parameters["Saturation"].SetValue(0);
            //    Game.HSVEffect.Parameters["UseMask"].SetValue(true);
            //    Camera.GraphicsDevice.SetRenderTarget(m_finalRenderTarget);
            //    Camera.GraphicsDevice.Textures[1] = m_traitAuraRenderTarget;
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
            //    Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
            //    Camera.End();
            //}


            //Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.GetTransformation());
            //Camera.Draw(Game.GenericTexture, new Rectangle((int)Camera.TopLeftCorner.X, (int)Camera.TopLeftCorner.Y, 1320, 720), Color.Black * BackBufferOpacity);

            //if (!m_player.IsKilled)
            //    m_player.Draw(Camera);

            //if (!LevelEV.CREATE_RETAIL_VERSION)
            //{
            //    DebugTextObj.Position = new Vector2(Camera.X, Camera.Y - 300f);
            //    DebugTextObj.Draw(Camera);
            //}

            //m_itemDropManager.Draw(Camera);
            //m_impactEffectPool.Draw(Camera);
            //Camera.End();



            //Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Camera.GetTransformation());
            //m_textManager.Draw(Camera);

            //if (CurrentRoom.LevelType == GameTypes.LevelType.TOWER)
            //    m_gardenParallaxFG.Draw(Camera);

            //m_whiteBG.Draw(Camera);
            //Camera.End();

            //if ((CurrentLevelType == GameTypes.LevelType.DUNGEON || Game.PlayerStats.Traits.X == 35f || Game.PlayerStats.Traits.Y == 35f) && (Game.PlayerStats.Class != 13 || Game.PlayerStats.Class == 13 && !Player.LightOn))
            //{
            //    Camera.GraphicsDevice.Textures[1] = m_lightSourceRenderTarget;
            //    Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.ShadowEffect);

            //    if (!LevelEV.SAVE_FRAMES)
            //        Camera.Draw(m_shadowRenderTarget, Vector2.Zero, Color.White);
            //    else
            //        Camera.Draw(m_shadowRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), SpriteEffects.None, 1f);

            //    Camera.End();
            //}
            //if (CurrentRoom.Name != "Ending")
            //{
            //    if ((Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f) && Game.PlayerStats.SpecialItem != 8)
            //    {
            //        Game.GaussianBlur.InvertMask = true;
            //        Game.GaussianBlur.Draw(m_finalRenderTarget, Camera, m_traitAuraRenderTarget);
            //    }
            //    else if ((Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f) && Game.PlayerStats.SpecialItem != 8)
            //    {
            //        Game.GaussianBlur.InvertMask = false;
            //        Game.GaussianBlur.Draw(m_finalRenderTarget, Camera, m_traitAuraRenderTarget);
            //    }
            //}
            //Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            //m_projectileIconPool.Draw(Camera);
            //m_playerHUD.Draw(Camera);

            //if (m_lastEnemyHit != null && m_enemyHUDCounter > 0f)
            //    m_enemyHUD.Draw(Camera);

            //if (m_enemyHUDCounter > 0f)
            //{
            //    ProceduralLevelScreen mEnemyHUDCounter = this;
            //    mEnemyHUDCounter.m_enemyHUDCounter = mEnemyHUDCounter.m_enemyHUDCounter - (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}

            //if (CurrentRoom.Name != "Start" && CurrentRoom.Name != "Boss" && CurrentRoom.Name != "ChallengeBoss" && m_miniMapDisplay.Visible)
            //{
            //    m_mapBG.Draw(Camera);
            //    m_miniMapDisplay.Draw(Camera);
            //}

            //if (CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Ending")
            //{
            //    m_compassBG.Draw(Camera);
            //    m_compass.Draw(Camera);
            //}

            //m_objectivePlate.Draw(Camera);
            //m_roomEnteringTitle.Draw(Camera);
            //m_roomTitle.Draw(Camera);

            //if (CurrentRoom.Name != "Ending" && (!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
            //    m_filmGrain.Draw(Camera);

            //Camera.End();

            //Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            //m_blackBorder1.Draw(Camera);
            //m_blackBorder2.Draw(Camera);
            //Camera.End();

            //Shout ripple
            //Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);

            //if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
            //{
            //    Vector2 position = m_player.Position - Camera.TopLeftCorner;
            //    Game.RippleEffect.Parameters["width"].SetValue(ShoutMagnitude);
            //    Game.RippleEffect.Parameters["xcenter"].SetValue(position.X / 1320f);
            //    Game.RippleEffect.Parameters["ycenter"].SetValue(position.Y / 720f);

            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.RippleEffect);
            //}
            //else
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);

            //Camera.Draw(m_finalRenderTarget, Vector2.Zero, Color.White);
            //Camera.End();


            //Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);

            //if (CurrentRoom.Name == "Ending")
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            //else if ((Game.PlayerStats.Traits.X == 1f || Game.PlayerStats.Traits.Y == 1f) && Game.PlayerStats.SpecialItem != 8)
            //{
            //    Game.HSVEffect.Parameters["Saturation"].SetValue(0);
            //    Game.HSVEffect.Parameters["Brightness"].SetValue(0);
            //    Game.HSVEffect.Parameters["Contrast"].SetValue(0);
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
            //}
            //else if ((!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
            //{
            //    Camera.GraphicsDevice.SetRenderTarget(m_finalRenderTarget);
            //    Game.HSVEffect.Parameters["Saturation"].SetValue(0.2f);
            //    Game.HSVEffect.Parameters["Brightness"].SetValue(0.1f);

            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
            //    Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
            //    Camera.End();

            //    Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            //    Camera.Draw(m_finalRenderTarget, Vector2.Zero, new Color(180, 150, 80));
            //    m_creditsText.Draw(Camera);
            //    m_creditsTitleText.Draw(Camera);
            //    Camera.End();

            //    Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            //}
            //else
            //    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);

            
            if (CurrentRoom.Name != "Ending" && (!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
            {
                Game.HSVEffect.Parameters["Saturation"].SetValue(0.2f);
                Game.HSVEffect.Parameters["Brightness"].SetValue(0.1f);

                Camera.GraphicsDevice.SetRenderTarget(m_finalRenderTarget);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
                Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
                Camera.End();

                Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
                Camera.Draw(m_finalRenderTarget, Vector2.Zero, new Color(180, 150, 80));
                m_creditsText.Draw(Camera);
                m_creditsTitleText.Draw(Camera);
                Camera.End();
            }

            Effect effect = null;

            if (CurrentRoom.Name != "Ending" && (Game.PlayerStats.Traits.X == 1f || Game.PlayerStats.Traits.Y == 1f) && Game.PlayerStats.SpecialItem != 8)
            {
                effect = Game.HSVEffect;
                effect.Parameters["Saturation"].SetValue(0);
                effect.Parameters["Brightness"].SetValue(0);
                effect.Parameters["Contrast"].SetValue(0);
            }

            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);

            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, effect);

            Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
            Camera.End();

            base.Draw(gameTime);
        }
    }
}
