﻿using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using DS2DEngine;
using InputSystem;
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
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (m_dropThroughGroundTimer > 0f)
                m_dropThroughGroundTimer -= totalSeconds;

            if (m_ninjaTeleportDelay > 0f)
                m_ninjaTeleportDelay -= totalSeconds;

            if (m_rapidSpellCastDelay > 0f)
                m_rapidSpellCastDelay -= totalSeconds;


            if (!(m_levelScreen.CurrentRoom is EndingRoomObj) && ScaleX > 0.1f)
            {
                RogueAPI.Game.Player.UpdatePlayerEffects(this, gameTime);
                //if ((Game.PlayerStats.Traits.Y == 22f || Game.PlayerStats.Traits.X == 22f) && base.CurrentSpeed == 0f && this.m_ambilevousTimer > 0f)
                //{
                //    this.m_ambilevousTimer -= totalSeconds;
                //    if (this.m_ambilevousTimer <= 0f)
                //    {
                //        this.m_ambilevousTimer = 0.4f;
                //        this.m_levelScreen.ImpactEffectPool.DisplayQuestionMark(new Vector2(base.X, this.Bounds.Top));
                //    }
                //}

                //if ((Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14) && this.m_wizardSparkleCounter > 0f)
                //{
                //    this.m_wizardSparkleCounter -= totalSeconds;
                //    if (this.m_wizardSparkleCounter <= 0f)
                //    {
                //        this.m_wizardSparkleCounter = 0.2f;
                //        this.m_levelScreen.ImpactEffectPool.DisplayChestSparkleEffect(base.Position);
                //        this.m_levelScreen.ImpactEffectPool.DisplayChestSparkleEffect(base.Position);
                //    }
                //}

                //if ((Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11) && this.m_assassinSmokeTimer > 0f)
                //{
                //    this.m_assassinSmokeTimer -= totalSeconds;
                //    if (this.m_assassinSmokeTimer <= 0f)
                //    {
                //        this.m_assassinSmokeTimer = base.CurrentSpeed > 0f ? 0.05f : 0.15f;
                //        this.m_levelScreen.ImpactEffectPool.BlackSmokeEffect(this);
                //    }
                //}
            }

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

            if (State != 3 && (!Game.GlobalInput.Pressed(10) && !Game.GlobalInput.Pressed(11) || m_currentLogicSet == m_airAttackLS && m_currentLogicSet.IsActive && !IsAirAttacking) && !m_isTouchingGround && AccelerationY < 0f)
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


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
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
                if (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerBlock) && CanBlock && !m_currentLogicSet.IsActive)
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

                        if (Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerBlock))
                            SoundManager.PlaySound("Player_Block_Action");
                    }
                    else if (Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerBlock))
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
                if (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerLeft1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerLeft2) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight2))
                {
                    if (m_isTouchingGround)
                        State = (int)RogueAPI.Game.PlayerState.Walking;

                    if ((Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight2)) && (!m_collidingRight || m_isTouchingGround))
                    {
                        base.HeadingX = 1f;
                        CurrentSpeed = TotalMovementSpeed;
                    }
                    else if ((Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerLeft1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerLeft2)) && !Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight1) && !Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight2) && (!m_collidingLeft || m_isTouchingGround))
                    {
                        base.HeadingX = -1f;
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
                        if (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerRight2))
                            Flip = SpriteEffects.None;
                        else if (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerLeft1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerLeft2))
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
                if ((Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump1) || Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump2)) && m_isTouchingGround && m_dropThroughGroundTimer <= 0f)
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
                else if ((Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump1) || Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump2)) && !m_isTouchingGround && m_doubleJumpCount < TotalDoubleJumps && m_dropThroughGroundTimer <= 0f)
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
                if ((Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerDown1) || Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerDown2)) && CanAirAttackDownward && Game.GameConfig.QuickDrop && State == (int)RogueAPI.Game.PlayerState.Jumping && m_dropThroughGroundTimer <= 0f)
                {
                    m_currentLogicSet = m_airAttackLS;
                    if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                        FadeSword();

                    if (m_assassinSpecialActive)
                        DisableAssassinAbility();

                    m_currentLogicSet.Execute();
                }
                else if (Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerAttack))
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
                        if ((Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerDown1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerDown2)) && CanAirAttackDownward)
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

                if (Game.PlayerStats.Class == 16 && (Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump1) || Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump2)))
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
            if ((Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerDashLeft) || Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerDashRight)) && CanAirDash &&
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
                base.HeadingX = Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerDashLeft) ? -1f : 1f;
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
                if (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerUp1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerUp2) || InputSystem.InputManager.Pressed(Buttons.LeftThumbstickUp, PlayerIndex.One))
                    AccelerationY = -TotalMovementSpeed;
                else if (Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerDown1) || Game.GlobalInput.Pressed((int)RogueAPI.Game.InputKeys.PlayerDown2) || InputSystem.InputManager.Pressed(Buttons.LeftThumbstickDown, PlayerIndex.One))
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

                if ((Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump1) || Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump2)) && State != (int)RogueAPI.Game.PlayerState.Dragon)
                {
                    State = (int)RogueAPI.Game.PlayerState.Jumping;
                    base.DisableGravity = false;
                    m_isFlying = false;
                    //return;
                }
            }
            else if ((Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump1) || Game.GlobalInput.JustPressed((int)RogueAPI.Game.InputKeys.PlayerJump2)) && !m_isTouchingGround && !startedJump && m_doubleJumpCount >= TotalDoubleJumps && m_dropThroughGroundTimer <= 0f && CanFly && m_flightCounter > 0f &&
                State != (int)RogueAPI.Game.PlayerState.Blocking && State != (int)RogueAPI.Game.PlayerState.Flying && State != (int)RogueAPI.Game.PlayerState.Tanuki && State != (int)RogueAPI.Game.PlayerState.Dragon)
            {
                AccelerationY = 0f;
                State = (int)RogueAPI.Game.PlayerState.Flying;
                base.DisableGravity = true;
                m_isFlying = true;
            }
        }

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
