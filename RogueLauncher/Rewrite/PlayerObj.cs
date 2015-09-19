using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueAPI.Classes;
using RogueAPI.Projectiles;
using RogueAPI.Spells;
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
        public int MaxHealth { get { return 0; } }
        [Rewrite]
        public float MaxMana { get { return 0; } }
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
        [Rewrite]
        public float CurrentMana { get; internal set; }
        [Rewrite]
        public ProceduralLevelScreen AttachedLevel { get { return null; } }
        [Rewrite]
        public bool IsFlying { get { return this.m_isFlying; } }
        [Rewrite]
        public bool IsAirAttacking { get; set; }

        [Rewrite]
        public void UpdateEquipmentColours() { }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassDamageGivenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).PhysicalDamageMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassDamageTakenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).DamageTakenMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassMagicDamageGivenMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).MagicDamageMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassMoveSpeedMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).MoveSpeedMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassTotalHPMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).HealthMultiplier; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap, contentHandler: "ChangeFieldCall")]
        public static float get_ClassTotalMPMultiplier(byte id) { return ClassDefinition.GetById(Game.PlayerStats.Class).ManaMultiplier; }

        public static void ChangeFieldCall(MethodGraph sourceGraph, MethodGraph newGraph)
        {
            sourceGraph.Attributes = MethodAttributes.Public | MethodAttributes.Static;
            sourceGraph.CallingConvention = CallingConventions.Standard;

            newGraph.Parameters[1].DeclaringObject = sourceGraph;
            newGraph.CallingConvention = CallingConventions.Standard | CallingConventions.HasThis;

            var instr = sourceGraph.InstructionList;
            instr.Locals.Clear();

            instr.RemoveAt(0);
            instr.RemoveAt(0);
            instr.RemoveAt(0);

            int ix = 0, count = instr.Count;
            while (ix < count)
            {
                var i = instr[ix++];
                if (i.ILCode == ILCode.Ldloc_0)
                    i.Replace(new ParameterInstruction() { OpCode = OpCodes.Ldarg_0 });
            }
        }

        [Rewrite]
        public PlayerObj(string spriteName, PlayerIndex playerIndex, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, Game game)
            : base(spriteName, physicsManager, levelToAttachTo)
        {
        }

        [Rewrite]
        public void Initialize() { }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void CastSpell(bool activateSecondary, bool megaSpell = false)
        {
            var spellId = Game.PlayerStats.Spell;
            var spell = SpellDefinition.GetById(spellId);

            Color white = Color.White;
            ProjectileInstance projData = spell.GetProjectileInstance(this);
            float damageMultiplier = spell.DamageMultiplier;
            projData.Damage = (int)((float)this.TotalMagicDamage * damageMultiplier);
            int manaCost = (int)(spell.ManaCost * (1f - SkillSystem.GetSkill(SkillType.Mana_Cost_Down).ModifierAmount));

            if (megaSpell)
            {
                manaCost *= 2;
                projData.Scale *= 1.75f;
                projData.Damage *= 2;
            }


            if (this.CurrentMana >= (float)manaCost)
            {
                this.m_spellCastDelay = 0.5f;
                if (!(this.AttachedLevel.CurrentRoom is CarnivalShoot1BonusRoom) && !(this.AttachedLevel.CurrentRoom is CarnivalShoot2BonusRoom) && (Game.PlayerStats.Traits.X == 31f || Game.PlayerStats.Traits.Y == 31f) && Game.PlayerStats.Class != 16 && Game.PlayerStats.Class != 17)
                {
                    var cls = ClassDefinition.GetById(Game.PlayerStats.Class);
                    var newSpell = SpellDefinition.GetRandomSpell(cls, new RogueAPI.Traits.TraitDefinition[0]);
                    Game.PlayerStats.Spell = newSpell.SpellId;

                    //byte[] spellList = ClassType.GetSpellList(Game.PlayerStats.Class);
                    //do
                    //{
                    //    Game.PlayerStats.Spell = spellList[CDGMath.RandomInt(0, (int)spellList.Length - 1)];
                    //}
                    //while (Game.PlayerStats.Spell == 6 || Game.PlayerStats.Spell == 4 || Game.PlayerStats.Spell == 11);
                    this.AttachedLevel.UpdatePlayerSpellIcon();
                }
            }

            float xValue = spell.MiscValue1;
            float yValue = spell.MiscValue2;

            if (this.CurrentMana < (float)manaCost)
            {
                SoundManager.PlaySound("Error_Spell");
            }
            else if (spellId != 6 && spellId != 5 && !this.m_damageShieldCast && manaCost > 0)
            {
                TextManager textManager = this.m_levelScreen.TextManager;
                Color skyBlue = Color.SkyBlue;
                float x = base.X;
                Rectangle bounds = this.Bounds;
                textManager.DisplayNumberStringText(-manaCost, "mp", skyBlue, new Vector2(x, (float)bounds.Top));
            }
            if (spellId != 12 && spellId != 11 && (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f))
            {
                projData.SourceAnchor = new Vector2(projData.SourceAnchor.X * -1f, projData.SourceAnchor.Y);
            }

            switch (spellId)
            {
                case 1:
                case 2:
                case 3:
                case 7:
                case 9:
                case 10:
                case 13:
                case 15:
                    {
                        if (this.CurrentMana < (float)manaCost || activateSecondary)
                        {
                            break;
                        }
                        if (spellId == 15)
                        {
                            projData.Lifespan = 0.75f;
                            projData.WrapProjectile = true;
                        }
                        if (spellId == 1)
                        {
                            SoundManager.PlaySound("Cast_Dagger");
                        }
                        else if (spellId == 2)
                        {
                            SoundManager.PlaySound("Cast_Axe");
                        }
                        else if (spellId == 9)
                        {
                            SoundManager.PlaySound("Cast_Chakram");
                        }
                        else if (spellId == 10)
                        {
                            SoundManager.PlaySound("Cast_GiantSword");
                        }
                        else if (spellId == 13 || spellId == 15)
                        {
                            string[] strArrays = new string[] { "Enemy_WallTurret_Fire_01", "Enemy_WallTurret_Fire_02", "Enemy_WallTurret_Fire_03", "Enemy_WallTurret_Fire_04" };
                            SoundManager.PlaySound(strArrays);
                        }
                        ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                        projectileObj.Spell = spellId;
                        projectileObj.TextureColor = white;
                        projectileObj.AltY = yValue;
                        projectileObj.AltX = xValue;
                        if (spellId == 8 && this.Flip == SpriteEffects.FlipHorizontally)
                        {
                            projectileObj.AltX = -xValue;
                        }
                        if (spellId == 10)
                        {
                            projectileObj.LifeSpan = xValue;
                            projectileObj.Opacity = 0f;
                            ProjectileObj y = projectileObj;
                            y.Y = y.Y - 20f;
                            Easing easing = new Easing(Tween.EaseNone);
                            string[] strArrays1 = new string[] { "Y", "20" };
                            Tween.By(projectileObj, 0.1f, easing, strArrays1);
                            Easing easing1 = new Easing(Tween.EaseNone);
                            string[] strArrays2 = new string[] { "Opacity", "1" };
                            Tween.To(projectileObj, 0.1f, easing1, strArrays2);
                        }
                        if (spellId == 9)
                        {
                            projData.Angle = new Vector2(-10f, -10f);
                            if (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f)
                            {
                                projData.SourceAnchor = new Vector2(-50f, -30f);
                                this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, -projectileObj.Rotation, megaSpell);
                            }
                            else
                            {
                                projData.SourceAnchor = new Vector2(50f, -30f);
                                this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, projectileObj.Rotation, megaSpell);
                            }
                            projData.RotationSpeed = -20f;
                            projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                        }
                        if (spellId == 3)
                        {
                            projectileObj.ShowIcon = true;
                            projectileObj.Rotation = 0f;
                            projectileObj.BlinkTime = xValue / 1.5f;
                            projectileObj.LifeSpan = 20f;
                        }
                        if (spellId == 7)
                        {
                            projectileObj.Rotation = 0f;
                            projectileObj.RunDisplacerEffect(this.m_levelScreen.CurrentRoom, this);
                            projectileObj.KillProjectile();
                        }
                        if (spellId == 10)
                        {
                            this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, 90f, megaSpell);
                        }
                        else if (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f)
                        {
                            this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, -projectileObj.Rotation, megaSpell);
                        }
                        else
                        {
                            this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, projectileObj.Rotation, megaSpell);
                        }
                        PlayerObj currentMana = this;
                        currentMana.CurrentMana = currentMana.CurrentMana - (float)manaCost;
                        break;
                    }
                case 4:
                    {
                        if (!this.m_timeStopCast)
                        {
                            if (this.CurrentMana < (float)manaCost || activateSecondary)
                            {
                                break;
                            }
                            PlayerObj playerObj = this;
                            playerObj.CurrentMana = playerObj.CurrentMana - (float)manaCost;
                            this.AttachedLevel.CastTimeStop(0f);
                            this.m_timeStopCast = true;
                            break;
                        }
                        else
                        {
                            this.AttachedLevel.StopTimeStop();
                            this.m_timeStopCast = false;
                            break;
                        }
                    }
                case 5:
                    {
                        int activeEnemies = this.AttachedLevel.CurrentRoom.ActiveEnemies;
                        int num1 = 9;
                        if (activeEnemies > num1)
                        {
                            activeEnemies = num1;
                        }
                        if (this.CurrentMana < (float)manaCost || activateSecondary || activeEnemies <= 0)
                        {
                            break;
                        }
                        SoundManager.PlaySound("Cast_Crowstorm");
                        int num2 = 200;
                        float single = 360f / (float)activeEnemies;
                        float single1 = 0f;
                        int num3 = 0;
                        List<EnemyObj>.Enumerator enumerator = this.AttachedLevel.CurrentRoom.EnemyList.GetEnumerator();
                        try
                        {
                            do
                            {
                                if (!enumerator.MoveNext())
                                {
                                    break;
                                }
                                EnemyObj current = enumerator.Current;
                                if (current.NonKillable || current.IsKilled)
                                {
                                    continue;
                                }
                                ProjectileObj radians = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                                radians.LifeSpan = 10f;
                                radians.AltX = 0.25f;
                                radians.AltY = 0.05f;
                                radians.Orientation = MathHelper.ToRadians(single1);
                                radians.Spell = spellId;
                                radians.TurnSpeed = 0.075f;
                                radians.IgnoreBoundsCheck = true;
                                radians.Target = current;
                                radians.CollisionTypeTag = 1;
                                radians.Position = CDGMath.GetCirclePosition(single1, (float)num2, base.Position);
                                this.m_levelScreen.ImpactEffectPool.SpellCastEffect(radians.Position, radians.Rotation, megaSpell);
                                single1 = single1 + single;
                                num3++;
                            }
                            while (num3 <= num1);
                        }
                        finally
                        {
                            ((IDisposable)enumerator).Dispose();
                        }
                        List<EnemyObj>.Enumerator enumerator1 = this.AttachedLevel.CurrentRoom.TempEnemyList.GetEnumerator();
                        try
                        {
                            do
                            {
                                if (!enumerator1.MoveNext())
                                {
                                    break;
                                }
                                EnemyObj enemyObj = enumerator1.Current;
                                if (enemyObj.NonKillable || enemyObj.IsKilled)
                                {
                                    continue;
                                }
                                ProjectileObj circlePosition = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                                circlePosition.LifeSpan = 99f;
                                circlePosition.AltX = 0.25f;
                                circlePosition.AltY = 0.05f;
                                circlePosition.Orientation = MathHelper.ToRadians(single1);
                                circlePosition.Spell = spellId;
                                circlePosition.TurnSpeed = 0.05f;
                                circlePosition.IgnoreBoundsCheck = true;
                                circlePosition.Target = enemyObj;
                                circlePosition.CollisionTypeTag = 1;
                                circlePosition.Position = CDGMath.GetCirclePosition(single1, (float)num2, base.Position);
                                this.m_levelScreen.ImpactEffectPool.SpellCastEffect(circlePosition.Position, circlePosition.Rotation, megaSpell);
                                single1 = single1 + single;
                                num3++;
                            }
                            while (num3 <= num1);
                        }
                        finally
                        {
                            ((IDisposable)enumerator1).Dispose();
                        }
                        PlayerObj currentMana1 = this;
                        currentMana1.CurrentMana = currentMana1.CurrentMana - (float)manaCost;
                        TextManager textManager1 = this.m_levelScreen.TextManager;
                        Color color = Color.SkyBlue;
                        float x1 = base.X;
                        Rectangle rectangle = this.Bounds;
                        textManager1.DisplayNumberStringText(-manaCost, "mp", color, new Vector2(x1, (float)rectangle.Top));
                        break;
                    }
                case 6:
                    {
                        if (this.m_translocatorSprite.Visible || this.CurrentMana < (float)manaCost)
                        {
                            if (!this.m_translocatorSprite.Visible || !(this.m_translocatorSprite.Scale == this.Scale))
                            {
                                break;
                            }
                            SoundManager.PlaySound("mfqt_teleport_in");
                            this.Translocate(this.m_translocatorSprite.Position);
                            this.m_translocatorSprite.Visible = false;
                            break;
                        }
                        else
                        {
                            PlayerObj playerObj1 = this;
                            playerObj1.CurrentMana = playerObj1.CurrentMana - (float)manaCost;
                            this.m_translocatorSprite.ChangeSprite(base.SpriteName);
                            this.m_translocatorSprite.GoToFrame(base.CurrentFrame);
                            this.m_translocatorSprite.Visible = true;
                            this.m_translocatorSprite.Position = base.Position;
                            this.m_translocatorSprite.Flip = this.Flip;
                            this.m_translocatorSprite.TextureColor = Color.Black;
                            this.m_translocatorSprite.Scale = Vector2.Zero;
                            for (int i = 0; i < base.NumChildren; i++)
                            {
                                (this.m_translocatorSprite.GetChildAt(i) as SpriteObj).ChangeSprite((this._objectList[i] as SpriteObj).SpriteName);
                                this.m_translocatorSprite.GetChildAt(i).Visible = this._objectList[i].Visible;
                            }
                            this.m_translocatorSprite.GetChildAt(16).Visible = false;
                            if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                            {
                                this.m_translocatorSprite.GetChildAt(10).Visible = false;
                                this.m_translocatorSprite.GetChildAt(11).Visible = false;
                            }
                            TextManager textManager2 = this.m_levelScreen.TextManager;
                            Color skyBlue1 = Color.SkyBlue;
                            float x2 = base.X;
                            Rectangle bounds1 = this.Bounds;
                            textManager2.DisplayNumberStringText(-manaCost, "mp", skyBlue1, new Vector2(x2, (float)bounds1.Top));
                            this.AttachedLevel.ImpactEffectPool.StartInverseEmit(this.m_translocatorSprite.Position);
                            ObjContainer mTranslocatorSprite = this.m_translocatorSprite;
                            Easing easing2 = new Easing(Linear.EaseNone);
                            string[] str = new string[] { "ScaleX", null, null, null };
                            str[1] = this.ScaleX.ToString();
                            str[2] = "ScaleY";
                            str[3] = this.ScaleY.ToString();
                            Tween.To(mTranslocatorSprite, 0.4f, easing2, str);
                            SoundManager.PlaySound("mfqt_teleport_out");
                            break;
                        }
                    }
                case 8:
                    {
                        if (this.CurrentMana < (float)manaCost || activateSecondary)
                        {
                            break;
                        }
                        SoundManager.PlaySound("Cast_Boomerang");
                        ProjectileObj projectileObj1 = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                        projectileObj1.Spell = spellId;
                        projectileObj1.IgnoreBoundsCheck = true;
                        projectileObj1.TextureColor = white;
                        projectileObj1.ShowIcon = true;
                        projectileObj1.AltX = xValue;
                        if (this.Flip == SpriteEffects.FlipHorizontally && Game.PlayerStats.Traits.X != 22f && Game.PlayerStats.Traits.Y != 22f || this.Flip == SpriteEffects.None && (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f))
                        {
                            projectileObj1.AltX = -xValue;
                        }
                        projectileObj1.AltY = 0.5f;
                        PlayerObj currentMana2 = this;
                        currentMana2.CurrentMana = currentMana2.CurrentMana - (float)manaCost;
                        this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj1.Position, projectileObj1.Rotation, megaSpell);
                        break;
                    }
                case 11:
                    {
                        if (!this.m_damageShieldCast)
                        {
                            if (this.CurrentMana < (float)manaCost || activateSecondary)
                            {
                                break;
                            }
                            this.m_damageShieldCast = true;
                            if (megaSpell)
                            {
                                this.m_megaDamageShieldCast = true;
                            }
                            SoundManager.PlaySound("Cast_FireShield");
                            int num4 = 200;
                            for (int j = 0; j < (int)yValue; j++)
                            {
                                float single2 = 360f / yValue * (float)j;
                                ProjectileObj projectileObj2 = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                                projectileObj2.LifeSpan = xValue;
                                projectileObj2.AltX = single2;
                                projectileObj2.AltY = (float)num4;
                                projectileObj2.Spell = spellId;
                                projectileObj2.AccelerationXEnabled = false;
                                projectileObj2.AccelerationYEnabled = false;
                                projectileObj2.IgnoreBoundsCheck = true;
                                this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj2.Position, projectileObj2.Rotation, megaSpell);
                            }
                            PlayerObj playerObj2 = this;
                            playerObj2.CurrentMana = playerObj2.CurrentMana - (float)manaCost;
                            break;
                        }
                        else
                        {
                            this.m_damageShieldCast = false;
                            this.m_megaDamageShieldCast = false;
                            break;
                        }
                    }
                case 12:
                    {
                        if (this.CurrentMana < (float)manaCost || activateSecondary)
                        {
                            break;
                        }
                        SoundManager.PlaySound("Cast_Dagger");
                        for (int k = 0; k < 4; k++)
                        {
                            ProjectileObj radians1 = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                            radians1.Orientation = MathHelper.ToRadians(projData.Angle.X);
                            radians1.Spell = spellId;
                            radians1.ShowIcon = true;
                            radians1.AltX = xValue;
                            radians1.AltY = 0.5f;
                            switch (k)
                            {
                                case 0:
                                    {
                                        projData.SourceAnchor = new Vector2(10f, -10f);
                                        break;
                                    }
                                case 1:
                                    {
                                        projData.SourceAnchor = new Vector2(10f, 10f);
                                        break;
                                    }
                                case 2:
                                    {
                                        projData.SourceAnchor = new Vector2(-10f, 10f);
                                        break;
                                    }
                            }
                            projData.Angle = new Vector2(projData.Angle.X + 90f, projData.Angle.Y + 90f);
                            this.m_levelScreen.ImpactEffectPool.SpellCastEffect(radians1.Position, radians1.Rotation, megaSpell);
                        }
                        PlayerObj currentMana3 = this;
                        currentMana3.CurrentMana = currentMana3.CurrentMana - (float)manaCost;
                        break;
                    }
                case 14:
                    {
                        if (this.CurrentMana < (float)manaCost)
                        {
                            break;
                        }
                        PlayerObj playerObj3 = this;
                        playerObj3.CurrentMana = playerObj3.CurrentMana - (float)manaCost;
                        this.ThrowDaggerProjectiles();
                        break;
                    }
                default:
                    {
                        if (spellId == 100)
                        {
                            if (this.CurrentMana < (float)manaCost || activateSecondary)
                            {
                                break;
                            }
                            ProjectileObj vector2 = this.m_levelScreen.ProjectileManager.FireProjectile(projData);
                            vector2.AltX = 1f;
                            vector2.AltY = 0.5f;
                            vector2.Opacity = 0f;
                            vector2.X = this.AttachedLevel.CurrentRoom.X;
                            vector2.Y = base.Y;
                            vector2.Scale = new Vector2((float)(this.AttachedLevel.CurrentRoom.Width / vector2.Width), 0f);
                            vector2.IgnoreBoundsCheck = true;
                            vector2.Spell = spellId;
                            PlayerObj currentMana4 = this;
                            currentMana4.CurrentMana = currentMana4.CurrentMana - (float)manaCost;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
            }
            projData.Dispose();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Draw(Camera2D camera)
        {
            this.m_swearBubble.Scale = new Vector2(this.ScaleX * 1.2f, this.ScaleY * 1.2f);
            this.m_swearBubble.Position = new Vector2(base.X - 30f * this.ScaleX, base.Y - 35f * this.ScaleX);
            this.m_swearBubble.Draw(camera);
            this.m_translocatorSprite.Draw(camera);
            base.Draw(camera);
            if (this.IsFlying && base.State != 9)
            {
                this.m_flightDurationText.Text = string.Format("{0:F1}", this.m_flightCounter);
                TextObj mFlightDurationText = this.m_flightDurationText;
                float x = base.X;
                Rectangle terrainBounds = this.TerrainBounds;
                mFlightDurationText.Position = new Vector2(x, (float)(terrainBounds.Top - 70));
                this.m_flightDurationText.Draw(camera);
            }
            camera.End();
            Game.ColourSwapShader.Parameters["desiredTint"].SetValue(this.m_playerHead.TextureColor.ToVector4());

            var args = RogueAPI.Game.Player.PipeSkinShaderArgs(this.m_levelScreen, this);
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
            this.m_playerHead.Draw(camera);
            camera.End();
            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
            if (Game.PlayerStats.IsFemale)
            {
                this._objectList[13].Draw(camera);
            }
            this._objectList[14].Draw(camera);
            this._objectList[15].Draw(camera);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Update(GameTime gameTime)
        {
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.m_dropThroughGroundTimer > 0f)
                this.m_dropThroughGroundTimer -= totalSeconds;

            if (this.m_ninjaTeleportDelay > 0f)
                this.m_ninjaTeleportDelay -= totalSeconds;

            if (this.m_rapidSpellCastDelay > 0f)
                this.m_rapidSpellCastDelay -= totalSeconds;


            if (!(this.m_levelScreen.CurrentRoom is EndingRoomObj) && this.ScaleX > 0.1f)
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

            if (this.m_swearBubbleCounter > 0f)
            {
                this.m_swearBubbleCounter -= totalSeconds;
                if (this.m_swearBubbleCounter <= 0f)
                    this.m_swearBubble.Visible = false;
            }

            if (this.m_blockInvincibleCounter > 0f)
                this.m_blockInvincibleCounter -= totalSeconds;

            if (this.IsFlying && this.State != 9)
            {
                this.m_flightCounter -= totalSeconds;

                if (this.m_flightCounter <= 0f)
                {
                    base.State = 2;
                    base.DisableGravity = false;
                    this.m_isFlying = false;
                }
            }

            if (this.AccelerationX < 0f)
                this.AccelerationX += 200f * totalSeconds;
            else if (this.AccelerationX > 0f)
                this.AccelerationX -= 200f * totalSeconds;

            if (Math.Abs(this.AccelerationX) < 3.6f)
                base.AccelerationX = 0f;

            this.X += this.Heading.X * (this.CurrentSpeed * totalSeconds);

            if (base.State == 1)
            {
                if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
                {
                    this.m_walkDownSoundLow.Update();
                    this.m_walkUpSoundLow.Update();
                }
                else if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
                {
                    this.m_walkDownSoundHigh.Update();
                    this.m_walkUpSoundHigh.Update();
                }
                else
                {
                    this.m_walkDownSound.Update();
                    this.m_walkUpSound.Update();
                }
            }

            if (this.m_externalLS.IsActive)
            {
                if (this.m_externalLS.IsActive)
                    this.m_externalLS.Update(gameTime);

                return;
            }

            if (this.m_attackCounter <= 0f)
                this.m_attackNumber = 0;
            else
                this.m_attackCounter -= totalSeconds;

            if (this.m_currentLogicSet.IsActive)
                this.m_currentLogicSet.Update(gameTime);

            if (this.m_dashCooldownCounter > 0)
                this.m_dashCooldownCounter -= gameTime.ElapsedGameTime.Milliseconds;

            if (this.m_dashCounter > 0)
            {
                this.m_dashCounter -= gameTime.ElapsedGameTime.Milliseconds;
                if (this.m_dashCounter <= 0 && base.State != 3)
                {
                    this.UnlockControls();
                    base.AccelerationYEnabled = true;
                }
            }

            if (this.m_invincibleCounter > 0)
            {
                this.m_invincibleCounter -= gameTime.ElapsedGameTime.Milliseconds;

                if (!this.m_assassinSpecialActive && this.Opacity != 0.6f)
                    base.Opacity = 0.6f;
            }
            else if (!this.m_assassinSpecialActive && base.Opacity == 0.6f)
                base.Opacity = 1f;

            if (!base.IsPaused && (this.m_currentLogicSet == null || !this.m_currentLogicSet.IsActive))
                this.UpdateAnimationState();

            this.CheckGroundCollision();

            if (base.State != 3 && (!Game.GlobalInput.Pressed(10) && !Game.GlobalInput.Pressed(11) || this.m_currentLogicSet == this.m_airAttackLS && this.m_currentLogicSet.IsActive && !this.IsAirAttacking) && !this.m_isTouchingGround && base.AccelerationY < 0f)
                this.AccelerationY += this.JumpDeceleration * totalSeconds;

            if (Game.PlayerStats.Class == 16 && this.CurrentMana < this.MaxMana)
            {
                this.m_dragonManaRechargeCounter += totalSeconds;
                if (this.m_dragonManaRechargeCounter >= 0.33f)
                {
                    this.m_dragonManaRechargeCounter = 0f;
                    this.CurrentMana += 4f;
                }
            }

            if (this.m_assassinSpecialActive)
            {
                this.m_assassinDrainCounter += totalSeconds;
                if (this.m_assassinDrainCounter >= 0.33f)
                {
                    this.m_assassinDrainCounter = 0f;
                    this.CurrentMana -= 7f;
                    if (this.CurrentMana <= 0f)
                        this.DisableAssassinAbility();
                }
            }

            if (this.m_timeStopCast)
            {
                this.m_timeStopDrainCounter += totalSeconds;
                if (this.m_timeStopDrainCounter >= 0.33f)
                {
                    this.m_timeStopDrainCounter = 0f;
                    this.CurrentMana -= 8f;
                    if (this.CurrentMana <= 0f)
                    {
                        this.AttachedLevel.StopTimeStop();
                        this.m_timeStopCast = false;
                    }
                }
            }

            if (this.m_damageShieldCast)
            {
                this.m_damageShieldDrainCounter += totalSeconds;
                if (this.m_damageShieldDrainCounter >= 0.33f)
                {
                    this.m_damageShieldDrainCounter = 0f;
                    this.CurrentMana -= this.m_megaDamageShieldCast ? 12f : 6f;
                    if (this.CurrentMana <= 0f)
                    {
                        this.m_damageShieldCast = false;
                        this.m_megaDamageShieldCast = false;
                    }
                }
            }

            if (this.m_lightOn)
            {
                this.m_lightDrainCounter += totalSeconds;
                if (this.m_lightDrainCounter >= 1f)
                {
                    this.m_lightDrainCounter = 0f;
                    this.CurrentMana -= 0f;
                    if (this.CurrentMana <= 0f)
                    {
                        this.m_lightOn = false;
                        this._objectList[16].Visible = false;
                    }
                }
            }

            if (base.State == 8)
            {
                this.m_tanookiDrainCounter += totalSeconds;
                if (this.m_tanookiDrainCounter >= 0.33f)
                {
                    this.m_tanookiDrainCounter = 0f;
                    this.CurrentMana -= 6f;
                    if (this.CurrentMana <= 0f)
                        this.DeactivateTanooki();
                }
            }

            if (this.m_spellCastDelay > 0f)
                this.m_spellCastDelay -= totalSeconds;

            base.Update(gameTime);
        }

        [Rewrite]
        public void UnlockControls()
        {
            //this.m_lockControls = false;
        }

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
    }
}
