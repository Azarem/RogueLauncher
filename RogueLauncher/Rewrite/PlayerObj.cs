using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueAPI.Classes;
using RogueAPI.Game;
using RogueAPI.Projectiles;
using RogueAPI.Spells;
using RogueAPI.Stats;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Tweener;
using Tweener.Ease;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.PlayerObj")]
    public class PlayerObj : CharacterObj
    {
        [Rewrite]
        private float m_spellCastDelay;
        [Rewrite]
        private bool m_damageShieldCast;
        [Rewrite]
        private bool m_timeStopCast;
        [Rewrite]
        private ObjContainer m_translocatorSprite;
        [Rewrite]
        private bool m_megaDamageShieldCast;
        [Rewrite]
        private SpriteObj m_swearBubble;
        [Rewrite]
        private TextObj m_flightDurationText;
        [Rewrite]
        private float m_flightCounter;
        [Rewrite]
        private SpriteObj m_playerHead;
        [Rewrite]
        private float m_dropThroughGroundTimer;
        [Rewrite]
        private float m_ninjaTeleportDelay;
        [Rewrite]
        private float m_rapidSpellCastDelay;
        [Rewrite]
        private float m_ambilevousTimer = 0.5f;
        [Rewrite]
        private float m_wizardSparkleCounter = 0.2f;
        [Rewrite]
        private float m_assassinSmokeTimer = 0.5f;
        [Rewrite]
        private float m_swearBubbleCounter;
        [Rewrite]
        private float m_blockInvincibleCounter;
        [Rewrite]
        private bool m_isFlying;
        [Rewrite]
        private LogicSet m_externalLS;
        [Rewrite]
        private FrameSoundObj m_walkUpSound;
        [Rewrite]
        private FrameSoundObj m_walkUpSoundLow;
        [Rewrite]
        private FrameSoundObj m_walkUpSoundHigh;
        [Rewrite]
        private FrameSoundObj m_walkDownSound;
        [Rewrite]
        private FrameSoundObj m_walkDownSoundLow;
        [Rewrite]
        private FrameSoundObj m_walkDownSoundHigh;
        [Rewrite]
        private float m_attackCounter;
        [Rewrite]
        private int m_attackNumber;
        [Rewrite]
        private LogicSet m_currentLogicSet;
        [Rewrite]
        private int m_dashCounter;
        [Rewrite]
        private int m_dashCooldownCounter;
        [Rewrite]
        private int m_invincibleCounter;
        [Rewrite]
        private bool m_assassinSpecialActive;
        [Rewrite]
        private LogicSet m_airAttackLS;
        [Rewrite]
        private float JumpDeceleration;
        [Rewrite]
        private float m_dragonManaRechargeCounter;
        [Rewrite]
        private float m_assassinDrainCounter;
        [Rewrite]
        private float m_timeStopDrainCounter;
        [Rewrite]
        private float m_damageShieldDrainCounter;
        [Rewrite]
        private bool m_lightOn;
        [Rewrite]
        private float m_lightDrainCounter;
        [Rewrite]
        private float m_tanookiDrainCounter;
        [Rewrite]
        private bool m_collidingLeft;
        [Rewrite]
        private bool m_collidingRight;
        [Rewrite]
        private byte m_numSequentialAttacks;
        [Rewrite]
        private LogicSet m_standingAttack3LogicSet;
        [Rewrite]
        private SpriteObj m_playerLegs;
        [Rewrite]
        private bool m_isJumping;
        [Rewrite]
        private byte m_doubleJumpCount;
        [Rewrite]
        private float ComboDelay = 1f;
        [Rewrite]
        private byte m_airDashCount;
        [Rewrite]
        private float DashCoolDown;
        [Rewrite]
        private float DashTime;
        [Rewrite]
        private float DashSpeed;
        [Rewrite]
        private float m_startingAnimationDelay;
        [Rewrite]
        private float m_currentMana;
        [Rewrite]
        private IPhysicsObj m_closestGround;
        [Rewrite]
        private bool m_collidingLeftOnly;
        [Rewrite]
        private bool m_collidingRightOnly;
        [Rewrite]
        private TeleporterObj m_lastTouchedTeleporter;
        [Rewrite]
        private Vector2 m_enemyKnockBack = Vector2.Zero;
        [Rewrite]
        public int MaxHealth { get { return 0; } }
        [Rewrite]
        public float MaxMana
        {
            get
            {

                return Player.Instance.GetStatObject(ManaStat.Id).MaxValue;

                //if (Game.PlayerStats.Traits.X != 12f && Game.PlayerStats.Traits.Y != 12f)
                //{
                //    int baseMana = (int)((this.BaseMana + (float)this.GetEquipmentMana() + (float)(this.ManaGainPerLevel * Game.PlayerStats.CurrentLevel) + (float)(Game.PlayerStats.BonusMana * 5) + SkillSystem.GetSkill(SkillType.Mana_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Mana_Up_Final).ModifierAmount) * this.ClassTotalMPMultiplier) + Game.PlayerStats.LichMana;
                //    if (baseMana < 1)
                //    {
                //        baseMana = 1;
                //    }
                //    return (float)baseMana;
                //}
                //int num = (int)Math.Round((double)(((float)(this.BaseHealth + this.GetEquipmentHealth() + this.HealthGainPerLevel * Game.PlayerStats.CurrentLevel + Game.PlayerStats.BonusHealth * 5) + SkillSystem.GetSkill(SkillType.Health_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Health_Up_Final).ModifierAmount) * this.ClassTotalHPMultiplier * Game.PlayerStats.LichHealthMod), MidpointRounding.AwayFromZero) + Game.PlayerStats.LichHealth;
                //if (num < 1)
                //{
                //    num = 1;
                //}
                //return (float)num;
            }
        }
        [Rewrite]
        public int Damage { get { return 0; } }
        [Rewrite]
        public int TotalMagicDamage { get { return 0; } }
        [Rewrite]
        public float TotalArmor { get { return 0; } }
        [Rewrite]
        public int CurrentWeight { get { return 0; } }
        [Rewrite]
        public int MaxWeight { get { return 0; } }
        [Rewrite]
        public float TotalCritChance { get { return 0; } }
        [Rewrite]
        public float TotalDamageReduc { get { return 0; } }
        [Rewrite]
        public float TotalCriticalDamage { get { return 0; } }
        [Rewrite]
        public int TotalDoubleJumps { get { return 0; } }
        [Rewrite]
        public int TotalAirDashes { get { return 0; } }
        [Rewrite]
        public int TotalVampBonus { get { return 0; } }
        [Rewrite]
        public float TotalFlightTime { get { return 0; } }
        [Rewrite]
        public float ManaGain { get { return 0; } }
        [Rewrite]
        public float TotalDamageReturn { get { return 0; } }
        [Rewrite]
        public float TotalGoldBonus { get { return 0; } }
        [Rewrite]
        public bool InvincibleToSpikes { get; set; }
        [Rewrite]
        public float ClassDamageTakenMultiplier { get { return 1f; } }
        [Rewrite]
        public float TotalMovementSpeedPercent { get { return 0; } }
        [Rewrite(action: RewriteAction.Replace)]
        public float CurrentMana
        {
            get { return m_currentMana; }
            set
            {
                m_currentMana = value;

                if (m_currentMana < 0f)
                    m_currentMana = 0f;

                if (m_currentMana > MaxMana)
                    m_currentMana = MaxMana;
            }
        }
        [Rewrite]
        public float InvincibilityTime { get; }
        [Rewrite]
        public bool ForceInvincible { get; set; }
        [Rewrite]
        public ProceduralLevelScreen AttachedLevel { get { return null; } }
        [Rewrite]
        public bool IsFlying { get { return m_isFlying; } }
        [Rewrite]
        public bool IsAirAttacking { get; set; }
        [Rewrite]
        public bool CanBlock { get { return Game.PlayerStats.Class == 8; } }
        [Rewrite]
        public bool CanAirAttackDownward { get { return true; } }
        [Rewrite]
        public bool LightOn { get { return false; } }
        [Rewrite]
        public float ProjectileLifeSpan { get; internal set; }

        [Rewrite]
        public float TotalMovementSpeed
        {
            get
            {
                //float flightSpeedMod = 0f;
                //if (base.State == 7 || base.State == 9)
                //{
                //    flightSpeedMod = this.FlightSpeedMod;
                //}
                //return base.Speed * (this.TotalMovementSpeedPercent + flightSpeedMod);
                return 0;
            }
        }
        [Rewrite]
        public bool CanAirDash
        {
            get
            {
                return TotalAirDashes > 0;
            }
        }
        [Rewrite]
        public bool CanFly
        {
            get
            {
                if (Game.PlayerStats.Class == 16)
                {
                    return true;
                }
                return TotalFlightTime > 0f;
            }
        }

        [Rewrite]
        public byte NumSequentialAttacks { get { return m_numSequentialAttacks; } set { m_numSequentialAttacks = value; } }
        [Rewrite]
        public int NumAirBounces { get; set; }
        [Rewrite]
        public float AirAttackKnockBack { get; internal set; }
        [Rewrite]
        private Vector2 StrongEnemyKnockBack { get; set; }
        [Rewrite]
        public Vector2 EnemyKnockBack { get { return (m_currentLogicSet == m_standingAttack3LogicSet) ? StrongEnemyKnockBack : m_enemyKnockBack; } set { m_enemyKnockBack = value; } }

        [Rewrite]
        private float BlockInvincibleTime { get; set; }

        [Rewrite]
        public float BlockManaDrain { get; set; }

        [Rewrite]
        public bool ControlsLocked { get; }

        [Rewrite]
        public void UpdateEquipmentColours() { }


        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        //public static float get_ClassDamageGivenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).PhysicalDamageMultiplier; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        //public static float get_ClassDamageTakenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).DamageTakenMultiplier; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        //public static float get_ClassMagicDamageGivenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).MagicDamageMultiplier; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        //public static float get_ClassMoveSpeedMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).MoveSpeedMultiplier; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        //public static float get_ClassTotalHPMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).HealthMultiplier; }

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        //public static float get_ClassTotalMPMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).ManaMultiplier; }

        //public static void ChangeFieldCall(MethodGraph sourceGraph, MethodGraph newGraph)
        //{
        //    sourceGraph.Attributes = MethodAttributes.Public | MethodAttributes.Static;
        //    sourceGraph.CallingConvention = CallingConventions.Standard;

        //    newGraph.Parameters[1].DeclaringObject = sourceGraph;
        //    newGraph.CallingConvention = CallingConventions.Standard | CallingConventions.HasThis;

        //    var instr = sourceGraph.InstructionList;
        //    instr.Locals.Clear();

        //    instr.RemoveAt(0);
        //    instr.RemoveAt(0);
        //    instr.RemoveAt(0);

        //    int ix = 0, count = instr.Count;
        //    while (ix < count)
        //    {
        //        var i = instr[ix++];
        //        if (i.ILCode == ILCode.Ldloc_0)
        //            i.Replace(new ParameterInstruction() { OpCode = OpCodes.Ldarg_0 });
        //    }
        //}

        [Rewrite]
        public PlayerObj(string spriteName, PlayerIndex playerIndex, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, Game game)
            : base(spriteName, physicsManager, levelToAttachTo)
        {
        }

        [Rewrite]
        public void Initialize() { }

        [Rewrite]
        public void UpdateInternalScale() { }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void CastSpell(bool activateSecondary, bool megaSpell = false)
        {
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void StopAllSpells()
        {
            var spell = RogueAPI.Game.Player.Spell;
            if (spell != null)
                spell.Deactivate(true);

            if (base.State == 8)
            {
                this.DeactivateTanooki();
            }
            if (this.m_damageShieldCast)
            {
                this.m_damageShieldCast = false;
                this.m_megaDamageShieldCast = false;
            }
            if (this.m_timeStopCast)
            {
                this.AttachedLevel.StopTimeStop();
                this.m_timeStopCast = false;
            }
            if (this.m_assassinSpecialActive)
            {
                this.DisableAssassinAbility();
            }
            this.m_lightOn = false;
            //this.m_translocatorSprite.Visible = false;
            if (base.State == 9)
            {
                base.State = 2;
                base.DisableGravity = false;
                this.m_isFlying = false;
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Draw(Camera2D camera)
        {
            var evt = new RogueAPI.PlayerDrawEventArgs(this, camera, true);
            RogueAPI.Event.Trigger(evt);

            m_swearBubble.Scale = new Vector2(ScaleX * 1.2f, ScaleY * 1.2f);
            m_swearBubble.Position = new Vector2(base.X - 30f * ScaleX, base.Y - 35f * ScaleX);
            m_swearBubble.Draw(camera);
            //m_translocatorSprite.Draw(camera);

            base.Draw(camera);

            evt.IsPreDraw = false;
            RogueAPI.Event.Trigger(evt);

            if (IsFlying && State != 9)
            {
                m_flightDurationText.Text = string.Format("{0:F1}", m_flightCounter);
                m_flightDurationText.Position = new Vector2(X, TerrainBounds.Top - 70);
                m_flightDurationText.Draw(camera);
            }

            camera.End();

            Game.ColourSwapShader.Parameters["desiredTint"].SetValue(m_playerHead.TextureColor.ToVector4());

            var args = RogueAPI.Game.Player.PipeSkinShaderArgs(m_levelScreen, this);
            Game.ColourSwapShader.Parameters["Opacity"].SetValue(args.Target.Opacity);
            Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(args.Target.ColorSwappedOut1.ToVector4());
            Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(args.Target.ColorSwappedIn1.ToVector4());
            Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(args.Target.ColorSwappedOut2.ToVector4());
            Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(args.Target.ColorSwappedIn2.ToVector4());

            //if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
            //{
            //    Game.ColourSwapShader.Parameters["Opacity"].SetValue(base.Opacity);
            //    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_lichColour1.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_lichColour2.ToVector4());
            //}
            //else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
            //{
            //    Game.ColourSwapShader.Parameters["Opacity"].SetValue(base.Opacity);
            //    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
            //}
            //else
            //{
            //    Game.ColourSwapShader.Parameters["Opacity"].SetValue(base.Opacity);
            //    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_skinColour1.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
            //    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_skinColour2.ToVector4());
            //}
            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader, camera.GetTransformation());
            m_playerHead.Draw(camera);
            camera.End();

            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
            if (Game.PlayerStats.IsFemale)
            {
                _objectList[13].Draw(camera);
            }
            _objectList[14].Draw(camera);
            _objectList[15].Draw(camera);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Update(GameTime gameTime)
        {
            var evt = new RogueAPI.PlayerUpdateEvent(gameTime, !(m_levelScreen.CurrentRoom is EndingRoomObj) && ScaleX > 0.1f);
            RogueAPI.Event.Trigger(evt);

            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (m_dropThroughGroundTimer > 0f)
                m_dropThroughGroundTimer -= totalSeconds;

            if (m_ninjaTeleportDelay > 0f)
                m_ninjaTeleportDelay -= totalSeconds;

            if (m_rapidSpellCastDelay > 0f)
                m_rapidSpellCastDelay -= totalSeconds;


            //if (!(m_levelScreen.CurrentRoom is EndingRoomObj) && ScaleX > 0.1f)
            //{
            //    RogueAPI.Game.Player.UpdatePlayerEffects(this, gameTime);
            //    //if ((Game.PlayerStats.Traits.Y == 22f || Game.PlayerStats.Traits.X == 22f) && base.CurrentSpeed == 0f && this.m_ambilevousTimer > 0f)
            //    //{
            //    //    this.m_ambilevousTimer -= totalSeconds;
            //    //    if (this.m_ambilevousTimer <= 0f)
            //    //    {
            //    //        this.m_ambilevousTimer = 0.4f;
            //    //        this.m_levelScreen.ImpactEffectPool.DisplayQuestionMark(new Vector2(base.X, this.Bounds.Top));
            //    //    }
            //    //}

            //    //if ((Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14) && this.m_wizardSparkleCounter > 0f)
            //    //{
            //    //    this.m_wizardSparkleCounter -= totalSeconds;
            //    //    if (this.m_wizardSparkleCounter <= 0f)
            //    //    {
            //    //        this.m_wizardSparkleCounter = 0.2f;
            //    //        this.m_levelScreen.ImpactEffectPool.DisplayChestSparkleEffect(base.Position);
            //    //        this.m_levelScreen.ImpactEffectPool.DisplayChestSparkleEffect(base.Position);
            //    //    }
            //    //}

            //    //if ((Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11) && this.m_assassinSmokeTimer > 0f)
            //    //{
            //    //    this.m_assassinSmokeTimer -= totalSeconds;
            //    //    if (this.m_assassinSmokeTimer <= 0f)
            //    //    {
            //    //        this.m_assassinSmokeTimer = base.CurrentSpeed > 0f ? 0.05f : 0.15f;
            //    //        this.m_levelScreen.ImpactEffectPool.BlackSmokeEffect(this);
            //    //    }
            //    //}
            //}

            if (m_swearBubbleCounter > 0f)
            {
                m_swearBubbleCounter -= totalSeconds;
                if (m_swearBubbleCounter <= 0f)
                    m_swearBubble.Visible = false;
            }

            if (m_blockInvincibleCounter > 0f)
                m_blockInvincibleCounter -= totalSeconds;

            if (IsFlying && State != 9)
            {
                m_flightCounter -= totalSeconds;

                if (m_flightCounter <= 0f)
                {
                    State = 2;
                    base.DisableGravity = false;
                    m_isFlying = false;
                }
            }

            if (AccelerationX < 0f)
                AccelerationX += 200f * totalSeconds;
            else if (AccelerationX > 0f)
                AccelerationX -= 200f * totalSeconds;

            if (Math.Abs(AccelerationX) < 3.6f)
                base.AccelerationX = 0f;

            X += Heading.X * (CurrentSpeed * totalSeconds);

            if (State == 1)
            {
                if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
                {
                    m_walkDownSoundLow.Update();
                    m_walkUpSoundLow.Update();
                }
                else if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
                {
                    m_walkDownSoundHigh.Update();
                    m_walkUpSoundHigh.Update();
                }
                else
                {
                    m_walkDownSound.Update();
                    m_walkUpSound.Update();
                }
            }

            if (m_externalLS.IsActive)
            {
                if (m_externalLS.IsActive)
                    m_externalLS.Update(gameTime);

                return;
            }

            if (m_attackCounter <= 0f)
                m_attackNumber = 0;
            else
                m_attackCounter -= totalSeconds;

            if (m_currentLogicSet.IsActive)
                m_currentLogicSet.Update(gameTime);

            if (m_dashCooldownCounter > 0)
                m_dashCooldownCounter -= gameTime.ElapsedGameTime.Milliseconds;

            if (m_dashCounter > 0)
            {
                m_dashCounter -= gameTime.ElapsedGameTime.Milliseconds;
                if (m_dashCounter <= 0 && State != 3)
                {
                    UnlockControls();
                    base.AccelerationYEnabled = true;
                }
            }

            if (m_invincibleCounter > 0)
            {
                m_invincibleCounter -= gameTime.ElapsedGameTime.Milliseconds;

                if (!m_assassinSpecialActive && Opacity != 0.6f)
                    base.Opacity = 0.6f;
            }
            else if (!m_assassinSpecialActive && base.Opacity == 0.6f)
                base.Opacity = 1f;

            if (!base.IsPaused && (m_currentLogicSet == null || !m_currentLogicSet.IsActive))
                UpdateAnimationState();

            CheckGroundCollision();

            if (State != 3 && (!InputManager.IsPressed(InputFlags.PlayerJump1 | InputFlags.PlayerJump2) || m_currentLogicSet == m_airAttackLS && m_currentLogicSet.IsActive && !IsAirAttacking) && !m_isTouchingGround && AccelerationY < 0f)
                AccelerationY += JumpDeceleration * totalSeconds;

            if (Game.PlayerStats.Class == 16 && CurrentMana < MaxMana)
            {
                m_dragonManaRechargeCounter += totalSeconds;
                if (m_dragonManaRechargeCounter >= 0.33f)
                {
                    m_dragonManaRechargeCounter = 0f;
                    CurrentMana += 4f;
                }
            }

            if (m_assassinSpecialActive)
            {
                m_assassinDrainCounter += totalSeconds;
                if (m_assassinDrainCounter >= 0.33f)
                {
                    m_assassinDrainCounter = 0f;
                    CurrentMana -= 7f;
                    if (CurrentMana <= 0f)
                        DisableAssassinAbility();
                }
            }

            if (m_timeStopCast)
            {
                m_timeStopDrainCounter += totalSeconds;
                if (m_timeStopDrainCounter >= 0.33f)
                {
                    m_timeStopDrainCounter = 0f;
                    CurrentMana -= 8f;
                    if (CurrentMana <= 0f)
                    {
                        AttachedLevel.StopTimeStop();
                        m_timeStopCast = false;
                    }
                }
            }

            if (m_damageShieldCast)
            {
                m_damageShieldDrainCounter += totalSeconds;
                if (m_damageShieldDrainCounter >= 0.33f)
                {
                    m_damageShieldDrainCounter = 0f;
                    CurrentMana -= m_megaDamageShieldCast ? 12f : 6f;
                    if (CurrentMana <= 0f)
                    {
                        m_damageShieldCast = false;
                        m_megaDamageShieldCast = false;
                    }
                }
            }

            if (m_lightOn)
            {
                m_lightDrainCounter += totalSeconds;
                if (m_lightDrainCounter >= 1f)
                {
                    m_lightDrainCounter = 0f;
                    CurrentMana -= 0f;
                    if (CurrentMana <= 0f)
                    {
                        m_lightOn = false;
                        _objectList[16].Visible = false;
                    }
                }
            }

            if (State == 8)
            {
                m_tanookiDrainCounter += totalSeconds;
                if (m_tanookiDrainCounter >= 0.33f)
                {
                    m_tanookiDrainCounter = 0f;
                    CurrentMana -= 6f;
                    if (CurrentMana <= 0f)
                        DeactivateTanooki();
                }
            }

            if (m_spellCastDelay > 0f)
                m_spellCastDelay -= totalSeconds;

            base.Update(gameTime);
        }

        [Rewrite]
        public override void HandleInput() { }


        [Rewrite(action: RewriteAction.Replace)]
        private void InputControls()
        {
            if (!LevelEV.CREATE_RETAIL_VERSION)
            {
                //if (InputManager.JustPressed(Keys.T, null))
                //{
                //    SoundManager.PlaySound(new string[] { "Fart1", "Fart2", "Fart3" });
                //    m_levelScreen.ImpactEffectPool.DisplayFartEffect(this);
                //}
            }

            if (RogueAPI.Game.InputManager.IsNewlyPressed(InputFlags.MenuMap) && Game.PlayerStats.TutorialComplete && m_levelScreen.CurrentRoom.Name != "Start" && m_levelScreen.CurrentRoom.Name != "Boss" && m_levelScreen.CurrentRoom.Name != "ChallengeBoss")
            {
                m_levelScreen.DisplayMap(false);
            }

            //Blocking
            if (State != (int)RogueAPI.Game.PlayerState.Tanuki)
            {
                if (InputManager.IsPressed(InputKeys.PlayerBlock) && CanBlock && !m_currentLogicSet.IsActive)
                {
                    if (CurrentMana >= 25f)
                    {
                        if (m_isTouchingGround)
                            CurrentSpeed = 0f;

                        if (State == (int)RogueAPI.Game.PlayerState.Flying)
                        {
                            CurrentSpeed = 0f;
                            AccelerationX = 0f;
                            AccelerationY = 0f;
                        }

                        State = (int)RogueAPI.Game.PlayerState.Blocking;

                        if (InputManager.IsNewlyPressed(InputKeys.PlayerBlock))
                            SoundManager.PlaySound("Player_Block_Action");
                    }
                    else if (InputManager.IsNewlyPressed(InputKeys.PlayerBlock))
                        SoundManager.PlaySound("Error_Spell");

                }
                else if (m_isTouchingGround)
                    State = (int)RogueAPI.Game.PlayerState.Idle;
                else if (!IsFlying)
                    State = (int)RogueAPI.Game.PlayerState.Jumping;
                else if (State != (int)RogueAPI.Game.PlayerState.Dragon)
                    State = (int)RogueAPI.Game.PlayerState.Flying;
                else
                    State = (int)RogueAPI.Game.PlayerState.Dragon;
            }

            if (State != (int)RogueAPI.Game.PlayerState.Blocking && State != (int)RogueAPI.Game.PlayerState.Tanuki)
            {
                if (InputManager.IsPressed(InputFlags.PlayerLeft1 | InputFlags.PlayerLeft2 | InputFlags.PlayerRight1 | InputFlags.PlayerRight2))
                {
                    if (m_isTouchingGround)
                        State = (int)RogueAPI.Game.PlayerState.Walking;

                    if (InputManager.IsPressed(InputFlags.PlayerRight1 | InputFlags.PlayerRight2) && (!m_collidingRight || m_isTouchingGround))
                    {
                        HeadingX = 1f;
                        CurrentSpeed = TotalMovementSpeed;
                    }
                    else if (InputManager.IsPressed(InputFlags.PlayerLeft1 | InputFlags.PlayerLeft2) && !InputManager.IsPressed(InputFlags.PlayerRight1 | InputFlags.PlayerRight2) && (!m_collidingLeft || m_isTouchingGround))
                    {
                        HeadingX = -1f;
                        CurrentSpeed = TotalMovementSpeed;
                    }
                    else
                        CurrentSpeed = 0f;

                    //if (!LevelEV.RUN_DEMO_VERSION && !LevelEV.CREATE_RETAIL_VERSION && (InputManager.Pressed(Keys.LeftShift, PlayerIndex.One) || InputManager.Pressed(Buttons.LeftShoulder, PlayerIndex.One)) && this.CanRun && this.m_isTouchingGround)
                    //{
                    //    PlayerObj currentSpeed = this;
                    //    currentSpeed.CurrentSpeed = currentSpeed.CurrentSpeed * this.RunSpeedMultiplier;
                    //}
                    if (!m_currentLogicSet.IsActive || m_currentLogicSet.IsActive && (Game.PlayerStats.Traits.X == 27f || Game.PlayerStats.Traits.Y == 27f))
                    {
                        if (InputManager.IsPressed(InputFlags.PlayerRight1 | InputFlags.PlayerRight2))
                            Flip = SpriteEffects.None;
                        else if (InputManager.IsPressed(InputKeys.PlayerLeft1 | InputKeys.PlayerLeft2))
                            Flip = SpriteEffects.FlipHorizontally;
                    }
                    if (m_isTouchingGround && m_currentLogicSet == m_standingAttack3LogicSet && m_currentLogicSet.IsActive && m_playerLegs.SpriteName != "PlayerWalkingLegs_Sprite")
                    {
                        m_playerLegs.ChangeSprite("PlayerWalkingLegs_Sprite");
                        m_playerLegs.PlayAnimation(base.CurrentFrame, base.TotalFrames, false);
                        SpriteObj mPlayerLegs = m_playerLegs;
                        mPlayerLegs.Y = mPlayerLegs.Y + 4f;
                        m_playerLegs.OverrideParentAnimationDelay = true;
                        m_playerLegs.AnimationDelay = 0.1f;
                    }
                }
                else
                {
                    if (m_isTouchingGround)
                        State = (int)RogueAPI.Game.PlayerState.Idle;

                    CurrentSpeed = 0f;
                }
            }


            bool startedJump = false;
            if (State != (int)RogueAPI.Game.PlayerState.Blocking && State != (int)RogueAPI.Game.PlayerState.Flying && State != (int)RogueAPI.Game.PlayerState.Tanuki && Game.PlayerStats.Class != 16)
            {
                if (InputManager.IsNewlyPressed(InputFlags.PlayerJump1 | InputFlags.PlayerJump2) && m_isTouchingGround && m_dropThroughGroundTimer <= 0f)
                {
                    State = (int)RogueAPI.Game.PlayerState.Jumping;
                    AccelerationY = -base.JumpHeight;
                    m_isJumping = true;
                    if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
                    {
                        SoundManager.PlaySound("Player_Jump_04_Low");
                        SoundManager.PlaySound("Player_WalkUp01_Low");
                    }
                    if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
                    {
                        SoundManager.PlaySound("Player_Jump_04_High");
                        SoundManager.PlaySound("Player_WalkUp01_High");
                    }
                    else
                    {
                        SoundManager.PlaySound("Player_Jump_04");
                        SoundManager.PlaySound("Player_WalkUp01");
                    }
                    if ((Game.PlayerStats.Traits.X == 19f || Game.PlayerStats.Traits.Y == 19f) && CDGMath.RandomInt(0, 100) >= 91)
                    {
                        SoundManager.PlaySound("Fart1", "Fart2", "Fart3");
                        m_levelScreen.ImpactEffectPool.DisplayDustEffect(this);
                    }
                    startedJump = true;
                }
                else if (InputManager.IsNewlyPressed(InputFlags.PlayerJump1 | InputFlags.PlayerJump2) && !m_isTouchingGround && m_doubleJumpCount < TotalDoubleJumps && m_dropThroughGroundTimer <= 0f)
                {
                    State = (int)RogueAPI.Game.PlayerState.Jumping;
                    AccelerationY = -base.DoubleJumpHeight;
                    m_isJumping = true;
                    m_levelScreen.ImpactEffectPool.DisplayDoubleJumpEffect(new Vector2(X, (float)(Bounds.Bottom + 10)));
                    m_doubleJumpCount++;
                    SoundManager.PlaySound("Player_DoubleJump");
                    if ((Game.PlayerStats.Traits.X == 19f || Game.PlayerStats.Traits.Y == 19f) && CDGMath.RandomInt(0, 100) >= 91)
                    {
                        SoundManager.PlaySound("Fart1", "Fart2", "Fart3");
                        m_levelScreen.ImpactEffectPool.DisplayDustEffect(this);
                    }
                    startedJump = true;
                }

                if (!m_isTouchingGround)
                {
                    if (m_currentLogicSet == m_standingAttack3LogicSet && m_currentLogicSet.IsActive)
                    {
                        if (AccelerationY > 0f && m_playerLegs.SpriteName != "PlayerAttackFallingLegs_Sprite")
                            m_playerLegs.ChangeSprite("PlayerAttackFallingLegs_Sprite");
                        else if (AccelerationY < 0f && m_playerLegs.SpriteName != "PlayerAttackJumpingLegs_Sprite")
                            m_playerLegs.ChangeSprite("PlayerAttackJumpingLegs_Sprite");
                    }

                    if (State != (int)RogueAPI.Game.PlayerState.Flying)
                        State = (int)RogueAPI.Game.PlayerState.Jumping;
                }
            }
            if (!m_currentLogicSet.IsActive && State != (int)RogueAPI.Game.PlayerState.Blocking && State != (int)RogueAPI.Game.PlayerState.Tanuki && Game.PlayerStats.Class != 16)
            {
                if (InputManager.IsNewlyPressed(InputFlags.PlayerDown1 | InputFlags.PlayerDown2) && CanAirAttackDownward && Game.GameConfig.QuickDrop && State == (int)RogueAPI.Game.PlayerState.Jumping && m_dropThroughGroundTimer <= 0f)
                {
                    m_currentLogicSet = m_airAttackLS;
                    if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                        FadeSword();

                    if (m_assassinSpecialActive)
                        DisableAssassinAbility();

                    m_currentLogicSet.Execute();
                }
                else if (InputManager.IsNewlyPressed(InputFlags.PlayerAttack))
                {
                    if (State != (int)RogueAPI.Game.PlayerState.Jumping)
                    {
                        if (!m_isTouchingGround)
                            CurrentSpeed = 0f;

                        if (m_attackCounter > 0f)
                        {
                            PlayerObj mAttackNumber = this;
                            mAttackNumber.m_attackNumber = mAttackNumber.m_attackNumber + 1;
                        }

                        m_attackCounter = ComboDelay;

                        if (m_attackNumber != 0)
                        {
                            m_currentLogicSet = m_standingAttack3LogicSet;
                            m_attackNumber = 0;
                            m_attackCounter = 0f;
                        }
                        else
                            m_currentLogicSet = m_standingAttack3LogicSet;

                        if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                            FadeSword();

                        if (m_assassinSpecialActive)
                            DisableAssassinAbility();

                        m_playerLegs.OverrideParentAnimationDelay = false;
                        m_currentLogicSet.Execute();
                    }
                    else
                    {
                        if (InputManager.IsPressed(InputFlags.PlayerDown1 | InputFlags.PlayerDown2) && CanAirAttackDownward)
                            m_currentLogicSet = m_airAttackLS;
                        else
                            m_currentLogicSet = m_standingAttack3LogicSet;

                        if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                            FadeSword();

                        if (m_assassinSpecialActive)
                            DisableAssassinAbility();

                        m_currentLogicSet.Execute();
                    }
                }
            }
            if (Game.PlayerStats.TutorialComplete)
            {
                var evt = new RogueAPI.InputEventHandler();
                RogueAPI.Event.Trigger(evt);

                //bool flag1 = false;
                //if (Game.PlayerStats.Spell == 15 && (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerAttack) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerSpell1)) && m_rapidSpellCastDelay <= 0f)
                //{
                //    m_rapidSpellCastDelay = 0.2f;
                //    CastSpell(false, false);
                //    flag1 = true;
                //}

                //if ((m_spellCastDelay <= 0f || Game.PlayerStats.Class == 16) && (Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerSpell1) || Game.PlayerStats.Class == 16 && Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerAttack)) && (Game.PlayerStats.Class != 16 || !flag1))
                //    CastSpell(false, false);

                //if (Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerBlock))
                //{
                //    RoomObj currentRoom = m_levelScreen.CurrentRoom;
                //    if (currentRoom is CarnivalShoot1BonusRoom || currentRoom is CarnivalShoot2BonusRoom || currentRoom is ChestBonusRoomObj)
                //    {
                //        if (State == (int)RogueAPI.Game.PlayerState.Tanuki)
                //            DeactivateTanooki();
                //    }
                //    else
                //    {
                //        RogueAPI.Event<RogueAPI.KeyPressEventArgs>.Trigger(new RogueAPI.KeyPressEventArgs(RogueAPI.Game.InputKeys.PlayerBlock));

                //        if (Game.PlayerStats.Class == 14 && m_spellCastDelay <= 0f)
                //            CastSpell(false, true);
                //        else if (Game.PlayerStats.Class == 15)
                //            ConvertHPtoMP();
                //        else if (Game.PlayerStats.Class == 11 && CurrentMana > 0f)
                //        {
                //            if (m_assassinSpecialActive)
                //                DisableAssassinAbility();
                //            else
                //                ActivateAssassinAbility();
                //        }
                //        else if (Game.PlayerStats.Class == 9)
                //            SwapSpells();
                //        else if (Game.PlayerStats.Class == 12)
                //            NinjaTeleport();
                //        else if (Game.PlayerStats.Class == 8)
                //        {
                //            if (State == (int)RogueAPI.Game.PlayerState.Tanuki)
                //                DeactivateTanooki();
                //            else if (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerDown1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerDown2))
                //                ActivateTanooki();
                //        }
                //        //else if (Game.PlayerStats.Class == 10)
                //        //    CastFuhRohDah();
                //        else if (Game.PlayerStats.Class == 17 && CurrentMana >= 30f && m_spellCastDelay <= 0f)
                //        {
                //            PlayerObj currentMana = this;
                //            currentMana.CurrentMana = currentMana.CurrentMana - 30f;
                //            m_spellCastDelay = 0.5f;
                //            ThrowAxeProjectiles();
                //        }

                //        if (Game.PlayerStats.Class == 16)
                //        {
                //            if (State == (int)RogueAPI.Game.PlayerState.Dragon)
                //            {
                //                State = (int)RogueAPI.Game.PlayerState.Jumping;
                //                base.DisableGravity = false;
                //                m_isFlying = false;
                //            }
                //            else
                //            {
                //                State = (int)RogueAPI.Game.PlayerState.Dragon;
                //                base.DisableGravity = true;
                //                m_isFlying = true;
                //                AccelerationY = 0f;
                //            }
                //        }
                //        else if (Game.PlayerStats.Class == 13)
                //        {
                //            if (!m_lightOn)
                //            {
                //                SoundManager.PlaySound("HeadLampOn");
                //                m_lightOn = true;
                //                _objectList[16].Visible = true;
                //            }
                //            else
                //            {
                //                SoundManager.PlaySound("HeadLampOff");
                //                m_lightOn = false;
                //                _objectList[16].Visible = false;
                //            }
                //        }
                //    }
                //}

                if (Game.PlayerStats.Class == 16 && InputManager.IsNewlyPressed(InputFlags.PlayerJump1 | InputFlags.PlayerJump2))
                {
                    if (State == (int)RogueAPI.Game.PlayerState.Dragon)
                    {
                        State = (int)RogueAPI.Game.PlayerState.Jumping;
                        base.DisableGravity = false;
                        m_isFlying = false;
                    }
                    else
                    {
                        State = (int)RogueAPI.Game.PlayerState.Dragon;
                        base.DisableGravity = true;
                        m_isFlying = true;
                        AccelerationY = 0f;
                    }
                }
            }

            //Air Dash
            if (InputManager.IsNewlyPressed(InputFlags.PlayerDashLeft | InputFlags.PlayerDashRight) && CanAirDash &&
                m_dashCooldownCounter <= 0 && (m_isTouchingGround || !m_isTouchingGround && m_airDashCount < TotalAirDashes) &&
                State != (int)RogueAPI.Game.PlayerState.Blocking && State != (int)RogueAPI.Game.PlayerState.Tanuki)
            {
                m_airDashCount++;
                base.AnimationDelay = m_startingAnimationDelay;
                State = (int)RogueAPI.Game.PlayerState.Dashing;
                base.AccelerationYEnabled = false;
                m_dashCooldownCounter = (int)(DashCoolDown * 1000f);
                m_dashCounter = (int)(DashTime * 1000f);
                LockControls();
                CurrentSpeed = DashSpeed;
                base.HeadingX = InputManager.IsNewlyPressed(InputFlags.PlayerDashLeft) ? -1f : 1f;
                AccelerationY = 0f;
                if (m_currentLogicSet.IsActive)
                    m_currentLogicSet.Stop();

                m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(X, (float)TerrainBounds.Bottom), true);
                SoundManager.PlaySound("Player_Dash");
                if ((Game.PlayerStats.Traits.X == 19f || Game.PlayerStats.Traits.Y == 19f) && CDGMath.RandomInt(0, 100) >= 91)
                {
                    m_levelScreen.ImpactEffectPool.DisplayDustEffect(this);
                    SoundManager.PlaySound("Fart1", "Fart2", "Fart3");
                }
            }

            //Flying
            if (State == (int)RogueAPI.Game.PlayerState.Flying || State == (int)RogueAPI.Game.PlayerState.Dragon)
            {
                if (InputManager.IsPressed(InputFlags.PlayerUp1 | InputFlags.PlayerUp2) || InputSystem.InputManager.Pressed(Buttons.LeftThumbstickUp, PlayerIndex.One))
                    AccelerationY = -TotalMovementSpeed;
                else if (InputManager.IsPressed(InputFlags.PlayerDown1 | InputFlags.PlayerDown2) || InputSystem.InputManager.Pressed(Buttons.LeftThumbstickDown, PlayerIndex.One))
                    AccelerationY = TotalMovementSpeed;
                else
                    AccelerationY = 0f;

                if (!m_isTouchingGround && m_currentLogicSet == m_standingAttack3LogicSet && m_currentLogicSet.IsActive)
                {
                    if (AccelerationY > 0f && m_playerLegs.SpriteName != "PlayerAttackFallingLegs_Sprite")
                        m_playerLegs.ChangeSprite("PlayerAttackFallingLegs_Sprite");
                    else if (AccelerationY <= 0f && m_playerLegs.SpriteName != "PlayerAttackJumpingLegs_Sprite")
                        m_playerLegs.ChangeSprite("PlayerAttackJumpingLegs_Sprite");
                }

                if (InputManager.IsNewlyPressed(InputFlags.PlayerJump1 | InputFlags.PlayerJump2) && State != (int)RogueAPI.Game.PlayerState.Dragon)
                {
                    State = (int)RogueAPI.Game.PlayerState.Jumping;
                    base.DisableGravity = false;
                    m_isFlying = false;
                    //return;
                }
            }
            else if (InputManager.IsNewlyPressed(InputFlags.PlayerJump1 | InputFlags.PlayerJump2) && !m_isTouchingGround && !startedJump && m_doubleJumpCount >= TotalDoubleJumps && m_dropThroughGroundTimer <= 0f && CanFly && m_flightCounter > 0f &&
                State != (int)RogueAPI.Game.PlayerState.Blocking && State != (int)RogueAPI.Game.PlayerState.Flying && State != (int)RogueAPI.Game.PlayerState.Tanuki && State != (int)RogueAPI.Game.PlayerState.Dragon)
            {
                AccelerationY = 0f;
                State = (int)RogueAPI.Game.PlayerState.Flying;
                base.DisableGravity = true;
                m_isFlying = true;
            }
        }


        [Rewrite(action: RewriteAction.Replace)]
        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            IPhysicsObj absParent = otherBox.AbsParent as IPhysicsObj;

            //Activate teleporter
            TeleporterObj teleporter = otherBox.Parent as TeleporterObj;
            if (teleporter != null && !ControlsLocked && IsTouchingGround && InputManager.IsNewlyPressed(InputKeys.PlayerUp1 | InputKeys.PlayerUp2))
            {
                StopAllSpells();
                LockControls();
                m_lastTouchedTeleporter = teleporter;
                Tween.RunFunction(0f, AttachedLevel, "DisplayMap", true);
            }

            //Activate boss door
            DoorObj doorObj = otherBox.Parent as DoorObj;
            if (doorObj != null && !ControlsLocked && IsTouchingGround && doorObj.IsBossDoor && !doorObj.Locked && InputManager.IsNewlyPressed(InputFlags.PlayerUp1 | InputFlags.PlayerUp2))
            {
                if (doorObj.Name != "FinalBossDoor")
                {
                    RoomObj linkedRoom = doorObj.Room.LinkedRoom;
                    if (linkedRoom != null)
                    {
                        foreach (DoorObj door in linkedRoom.DoorList)
                        {
                            if (!door.IsBossDoor)
                                continue;

                            if (linkedRoom is LastBossChallengeRoom)
                                linkedRoom.LinkedRoom = AttachedLevel.CurrentRoom;

                            StopAllSpells();
                            CurrentSpeed = 0f;
                            LockControls();
                            (m_levelScreen.ScreenManager as RCScreenManager).StartWipeTransition();
                            Tween.RunFunction(0.2f, this, "EnterBossRoom", new Vector2(door.Width / 2f, door.Bounds.Bottom - (Bounds.Bottom - Y)));
                            Tween.RunFunction(0.2f, m_levelScreen.ScreenManager, "EndWipeTransition");
                            break;
                        }
                    }
                }
                else
                    Game.ScreenManager.DisplayScreen(ScreenType.Ending, true, null);
            }


            BreakableObj breakableObj = absParent as BreakableObj;

            //Down-attack hit response
            if (breakableObj != null && IsAirAttacking && thisBox.Type == Consts.WEAPON_HITBOX)
            {
                IsAirAttacking = false;
                AccelerationY = -AirAttackKnockBack;
                NumAirBounces++;
            }

            //Break furniture
            if (Game.PlayerStats.Traits.X == TraitType.NoFurniture || Game.PlayerStats.Traits.Y == TraitType.NoFurniture)
            {
                if (breakableObj != null && !breakableObj.Broken)
                    breakableObj.Break();

                if (absParent.GetType() == typeof(PhysicsObj) && (absParent as PhysicsObj).SpriteName != "CastleEntranceGate_Sprite")
                    return;
            }

            if (collisionResponseType == Consts.COLLISIONRESPONSE_TERRAIN && (absParent.CollisionTypeTag == GameTypes.CollisionType_WALL || absParent.CollisionTypeTag == GameTypes.CollisionType_WALL_FOR_PLAYER || absParent.CollisionTypeTag == GameTypes.CollisionType_ENEMYWALL || absParent.CollisionTypeTag == GameTypes.CollisionType_GLOBAL_DAMAGE_WALL))
            {
                //float accelerationY = AccelerationY;

                bool shouldCollide = true;

                if (m_dropThroughGroundTimer > 0f && !absParent.CollidesBottom && absParent.CollidesTop)
                    shouldCollide = false;

                if (m_isTouchingGround && !absParent.CollidesBottom && absParent.CollidesTop && absParent.TerrainBounds.Top < TerrainBounds.Bottom - 10)
                    shouldCollide = false;

                if (!absParent.CollidesBottom && Bounds.Bottom > absParent.TerrainBounds.Top + 10 && !m_isTouchingGround)
                    shouldCollide = false;

                if (!absParent.CollidesBottom && absParent.CollidesTop && (State == (int)PlayerState.Flying || State == (int)PlayerState.Dragon))
                    shouldCollide = false;

                Vector2 vector21 = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);
                if ((m_collidingLeftOnly || m_collidingRightOnly) && Math.Abs(vector21.X) < 10f && !m_isTouchingGround && !(absParent is HazardObj))
                    shouldCollide = false;

                //X adjustment
                if (!absParent.CollidesLeft && !absParent.CollidesRight && absParent.CollidesTop && absParent.CollidesBottom && !(absParent is HazardObj))
                {
                    if (Game.PlayerStats.Traits.X == TraitType.Dwarfism || Game.PlayerStats.Traits.Y == TraitType.Dwarfism)
                        shouldCollide = false;
                    else if (X >= absParent.TerrainBounds.Center.X)
                        X += absParent.TerrainBounds.Right - TerrainBounds.Left;
                    else
                        X -= TerrainBounds.Right - absParent.TerrainBounds.Left;
                }

                //Y adjustment
                if (m_isTouchingGround && m_closestGround == absParent)
                {
                    shouldCollide = false;
                    if (!(absParent is HazardObj) || absParent.Rotation != -90f)
                        Y += m_closestGround.TerrainBounds.Top - TerrainBounds.Bottom;
                    else
                        Y += m_closestGround.Bounds.Top - TerrainBounds.Bottom + 15;
                    AccelerationY = 0f;
                }

                if (shouldCollide)
                    base.CollisionResponse(thisBox, otherBox, collisionResponseType);

                Vector2 intersect = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                if (intersect.Y != 0f && otherBox.AbsRotation != 0f)
                    X -= intersect.X;
            }

            if (thisBox.Type == Consts.BODY_HITBOX && otherBox.Type == Consts.WEAPON_HITBOX && (absParent.CollisionTypeTag == GameTypes.CollisionType_ENEMY || absParent.CollisionTypeTag == GameTypes.CollisionType_ENEMYWALL || absParent.CollisionTypeTag == GameTypes.CollisionType_GLOBAL_DAMAGE_WALL) && State != (int)PlayerState.Hurt && m_invincibleCounter <= 0)
            {
                EnemyObj enemyObj = absParent as EnemyObj;
                if (enemyObj != null && enemyObj.IsDemented)
                    return;

                ProjectileObj projectileObj = absParent as ProjectileObj;
                if (projectileObj != null && projectileObj.IsDemented)
                    return;

                if (!LevelEV.ENABLE_PLAYER_DEBUG)
                {
                    if (State == (int)PlayerState.Blocking && (CurrentMana > 0f || m_blockInvincibleCounter > 0f) && (projectileObj == null || projectileObj != null && projectileObj.Spell != SpellType.Boomerang && projectileObj.Spell != SpellType.Bounce))
                    {
                        if (CanBeKnockedBack)
                        {
                            Point center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                            Vector2 position = new Vector2(center.X, center.Y);
                            if (position == Vector2.Zero)
                                position = Position;

                            m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(position, Vector2.One);
                            CurrentSpeed = 0f;

                            AccelerationX = (otherBox.AbsParent.Bounds.Left + otherBox.AbsParent.Bounds.Width / 2 > X)
                                ? -KnockBack.X
                                : KnockBack.X;

                            AccelerationY = -KnockBack.Y;
                            Blink(Color.LightBlue, 0.1f);
                        }

                        if (m_blockInvincibleCounter <= 0f)
                        {
                            CurrentMana -= BlockManaDrain;
                            m_blockInvincibleCounter = BlockInvincibleTime;
                            m_levelScreen.TextManager.DisplayNumberStringText(-25, "mp", Color.SkyBlue, new Vector2(X, Bounds.Top));
                        }

                        SoundManager.PlaySound("Player_Block");
                    }
                    else if (m_invincibleCounter <= 0)
                        HitPlayer(otherBox.AbsParent);

                    ProjectileObj otherParent = otherBox.AbsParent as ProjectileObj;
                    if (otherParent != null && otherParent.DestroysWithEnemy && !m_assassinSpecialActive)
                        otherParent.RunDestroyAnimation(true);

                }
            }

            //Pick up item
            ItemDropObj itemDropObj = absParent as ItemDropObj;
            if (itemDropObj != null && itemDropObj.IsCollectable)
            {
                itemDropObj.GiveReward(this, m_levelScreen.TextManager);
                itemDropObj.IsCollidable = false;
                itemDropObj.IsWeighted = false;
                itemDropObj.AnimationDelay = 0.0166666675f;
                itemDropObj.AccelerationY = 0f;
                itemDropObj.AccelerationX = 0f;
                Tween.By(itemDropObj, 0.4f, Quad.EaseOut, "Y", "-120");
                Tween.To(itemDropObj, 0.1f, Linear.EaseNone, "delay", "0.6", "Opacity", "0");
                Tween.AddEndHandlerToLastTween(m_levelScreen.ItemDropManager, "DestroyItemDrop", itemDropObj);
                SoundManager.PlaySound("CoinDrop1", "CoinDrop2", "CoinDrop3", "CoinDrop4", "CoinDrop5");
            }

            //Open chest
            ChestObj chestObj = absParent as ChestObj;
            if (chestObj != null && !ControlsLocked && m_isTouchingGround && InputManager.IsNewlyPressed(InputFlags.PlayerUp1 | InputFlags.PlayerUp2) && !chestObj.IsOpen)
                chestObj.OpenChest(m_levelScreen.ItemDropManager, this);
        }

        [Rewrite(action: RewriteAction.Replace)]
        public void HitPlayer(GameObj obj)
        {
            bool shouldHit = true;

            if (obj is HazardObj && (Game.PlayerStats.SpecialItem == SpecialItemType.SpikeImmunity && obj.Bounds.Top > Y || InvincibleToSpikes))
                shouldHit = false;

            ProjectileObj projectileObj = obj as ProjectileObj;
            EnemyObj projectileSource = null;

            if (projectileObj != null)
            {
                if (projectileObj.IsDemented)
                    shouldHit = false;
                else if (projectileObj.Spell == SpellType.Bounce || projectileObj.Spell == SpellType.Boomerang)
                {
                    shouldHit = false;
                    projectileObj.KillProjectile();
                    m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, CDGMath.AngleBetweenPts(Position, projectileObj.Position), false);
                }

                projectileSource = projectileObj.Source as EnemyObj;
                if (projectileSource != null && (projectileSource.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || projectileSource is EnemyObj_LastBoss) && projectileSource.CurrentHealth <= 0)
                    shouldHit = false;
            }

            EnemyObj enemyObj = obj as EnemyObj;
            if (enemyObj != null && enemyObj.IsDemented)
                shouldHit = false;

            if (enemyObj != null && (enemyObj.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || enemyObj is EnemyObj_LastBoss) && enemyObj.CurrentHealth <= 0)
                shouldHit = false;

            if (shouldHit && (!ForceInvincible || ForceInvincible && obj is HazardObj))
            {
                Blink(Color.Red, 0.1f);
                m_levelScreen.ImpactEffectPool.DisplayPlayerImpactEffect(Position);
                AccelerationYEnabled = true;
                UnlockControls();

                int damage = 0;
                if (projectileObj != null)
                    damage = projectileObj.Damage;
                else if (obj is IDealsDamageObj)
                    damage = ((IDealsDamageObj)obj).Damage;

                damage = (int)((damage - damage * TotalDamageReduc) * ClassDamageTakenMultiplier);

                if (damage < 0)
                    damage = 0;

                if (!Game.PlayerStats.TutorialComplete)
                    damage = 0;

                CurrentHealth -= damage;

                //Apply damage return to enemy
                if (enemyObj != null && CurrentHealth > 0)
                {
                    int totalDamageReturn = (int)(damage * TotalDamageReturn);
                    if (totalDamageReturn > 0)
                        enemyObj.HitEnemy(totalDamageReturn, enemyObj.Position, true);
                }

                //Apply damage return to projectile source
                if (projectileObj != null && projectileObj.CollisionTypeTag == GameTypes.CollisionType_ENEMY)
                {
                    if (projectileSource != null && !projectileSource.IsKilled && !projectileSource.IsDemented && CurrentHealth > 0)
                    {
                        int projDamageReturn = (int)(damage * TotalDamageReturn);
                        if (projDamageReturn > 0)
                            projectileSource.HitEnemy(projDamageReturn, projectileSource.Position, true);
                    }
                }

                m_isJumping = false;
                m_isFlying = false;
                DisableGravity = false;

                if (!CanBeKnockedBack)
                    m_invincibleCounter = (int)(InvincibilityTime * 1000f);
                else
                {
                    if (Game.PlayerStats.Traits.X == TraitType.Tourettes || Game.PlayerStats.Traits.Y == TraitType.Tourettes)
                    {
                        m_swearBubble.ChangeSprite("SwearBubble" + CDGMath.RandomInt(1, 4) + "_Sprite");
                        m_swearBubble.Visible = true;
                        m_swearBubbleCounter = 1f;
                    }

                    State = (int)PlayerState.Hurt;
                    UpdateAnimationState();

                    if (m_currentLogicSet.IsActive)
                        m_currentLogicSet.Stop();

                    IsAirAttacking = false;
                    AnimationDelay = m_startingAnimationDelay;
                    CurrentSpeed = 0f;

                    float knockbackInertia = 1f;
                    if (Game.PlayerStats.Traits.X == TraitType.Ectomorph || Game.PlayerStats.Traits.Y == TraitType.Ectomorph)
                        knockbackInertia = 1.85f;

                    if (Game.PlayerStats.Traits.X == TraitType.Endomorph || Game.PlayerStats.Traits.Y == TraitType.Endomorph)
                        knockbackInertia = 0.5f;

                    if ((obj.Bounds.Left + obj.Bounds.Width / 2) <= X)
                        AccelerationX = KnockBack.X * knockbackInertia;
                    else
                        AccelerationX = -KnockBack.X * knockbackInertia;

                    AccelerationY = -KnockBack.Y * knockbackInertia;
                }

                if (CurrentHealth <= 0)
                {
                    if (Game.PlayerStats.SpecialItem == SpecialItemType.Revive)
                    {
                        CurrentHealth = (int)(MaxHealth * 0.25f);
                        Game.PlayerStats.SpecialItem = 0;
                        (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerHUDSpecialItem();
                        m_invincibleCounter = (int)(InvincibilityTime * 1000f);
                        (m_levelScreen.ScreenManager as RCScreenManager).DisplayScreen(ScreenType.DeathDefy, true, null);
                    }
                    else if (CDGMath.RandomInt(1, 100) <= SkillSystem.GetSkill(SkillType.Death_Dodge).ModifierAmount * 100f)
                    {
                        CurrentHealth = (int)(MaxHealth * 0.1f);
                        m_invincibleCounter = (int)(InvincibilityTime * 1000f);
                        (m_levelScreen.ScreenManager as RCScreenManager).DisplayScreen(ScreenType.DeathDefy, true, null);
                    }
                    else
                    {
                        ChallengeBossRoomObj currentRoom = this.AttachedLevel.CurrentRoom as ChallengeBossRoomObj;
                        if (currentRoom == null)
                        {
                            AttachedLevel.SetObjectKilledPlayer(obj);
                            Kill(true);
                        }
                        else
                            currentRoom.KickPlayerOut();
                    }
                }

                //Display damage text
                if (!m_levelScreen.IsDisposed)
                {
                    if (Game.PlayerStats.Traits.X == TraitType.Hypochondriac || Game.PlayerStats.Traits.Y == TraitType.Hypochondriac)
                        m_levelScreen.TextManager.DisplayNumberText(damage * 100 + CDGMath.RandomInt(1, 99), Color.Red, new Vector2(X, Bounds.Top));
                    else
                        m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(X, Bounds.Top));
                }

                //Drop coins when carrying special item LoseCoins
                if (Game.PlayerStats.SpecialItem == SpecialItemType.LoseCoins)
                {
                    int gold = (int)(Game.PlayerStats.Gold * 0.25f / 10f);
                    if (gold > 50)
                        gold = 50;

                    if (gold > 0 && AttachedLevel.ItemDropManager.AvailableItems > gold)
                    {
                        float goldMultiplier = 1f;
                        if (Game.PlayerStats.HasArchitectFee)
                            goldMultiplier = 0.6f;

                        int totalGoldLost = (int)((gold * 10) * (1f + TotalGoldBonus) * goldMultiplier);
                        Game.PlayerStats.Gold -= totalGoldLost;

                        for (int i = 0; i < gold; i++)
                            m_levelScreen.ItemDropManager.DropItemWide(Position, 1, 10f);

                        if (totalGoldLost > 0)
                            AttachedLevel.TextManager.DisplayNumberStringText(-totalGoldLost, "gold", Color.Yellow, new Vector2(X, Bounds.Top));
                    }
                }

                if (Game.PlayerStats.IsFemale)
                    SoundManager.PlaySound("Player_Female_Damage_03", "Player_Female_Damage_04", "Player_Female_Damage_05", "Player_Female_Damage_06", "Player_Female_Damage_07");
                else
                    SoundManager.PlaySound("Player_Male_Injury_01", "Player_Male_Injury_02", "Player_Male_Injury_03", "Player_Male_Injury_04", "Player_Male_Injury_05", "Player_Male_Injury_06", "Player_Male_Injury_07", "Player_Male_Injury_08", "Player_Male_Injury_09", "Player_Male_Injury_10");

                SoundManager.PlaySound("EnemyHit1", "EnemyHit2", "EnemyHit3", "EnemyHit4", "EnemyHit5", "EnemyHit6");
            }
        }

        [Rewrite]
        public override void Kill(bool giveXP = true) { }

        [Rewrite]
        public void LockControls()
        {
            //this.m_lockControls = true;
        }
        [Rewrite]
        public void UnlockControls()
        {
            //this.m_lockControls = false;
        }

        [Rewrite]
        public void ConvertHPtoMP() { }

        [Rewrite]
        public void ActivateTanooki() { }

        [Rewrite]
        public void DeactivateTanooki() { }

        [Rewrite]
        public void DisableAssassinAbility() { }

        [Rewrite]
        private void CheckGroundCollision() { }

        [Rewrite]
        private void UpdateAnimationState() { }

        [Rewrite]
        public void Translocate(Vector2 position) { }

        [Rewrite]
        private void ThrowDaggerProjectiles() { }

        [Rewrite]
        public void FadeSword() { }

        [Rewrite]
        public void ActivateAssassinAbility() { }

        [Rewrite]
        public void SwapSpells() { }

        [Rewrite]
        public void NinjaTeleport() { }

        [Rewrite]
        public void CastFuhRohDah() { }

        [Rewrite]
        private void ThrowAxeProjectiles() { }
    }
}
