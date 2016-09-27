using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EnemyObj")]
    public class EnemyObj : CharacterObj
    {
        [Rewrite]
        protected const int STATE_WANDER = 0;
        [Rewrite]
        protected const int STATE_ENGAGE = 1;
        [Rewrite]
        protected const int STATE_PROJECTILE_ENGAGE = 2;
        [Rewrite]
        protected const int STATE_MELEE_ENGAGE = 3;

        [Rewrite]
        public bool NonKillable { get; set; }
        [Rewrite]
        public byte Type;
        [Rewrite]
        protected float m_invincibleCounter;
        [Rewrite]
        protected float m_invincibleCounterProjectile;
        [Rewrite]
        protected float m_invincibilityTime = 0.4f;
        [Rewrite]
        private int m_numTouchingGrounds;
        [Rewrite]
        protected bool CanFallOffLedges = true;
        [Rewrite]
        public bool IsDemented { get; set; }
        [Rewrite]
        protected float InvincibilityTime { get { return this.m_invincibilityTime; } }
        [Rewrite]
        protected PlayerObj m_target;

        [Rewrite]
        public EnemyObj(string spriteName, PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base(spriteName, physicsManager, levelToAttachTo)
        {
        }

        [Rewrite]
        public void DrawDetectionRadii(Camera2D camera) { }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            IPhysicsObj absParent = otherBox.AbsParent as IPhysicsObj;

            if (collisionResponseType == Consts.COLLISIONRESPONSE_FIRSTBOXHIT)
            {
                if ((absParent.CollisionTypeTag == GameTypes.CollisionType_PLAYER || absParent.CollisionTypeTag == GameTypes.CollisionType_GLOBAL_DAMAGE_WALL || absParent.CollisionTypeTag == GameTypes.CollisionType_GLOBAL_DAMAGE_WALL && IsWeighted)
                    && (!(otherBox.AbsParent is RogueAPI.Projectiles.ProjectileObj) && m_invincibleCounter <= 0f
                        || otherBox.AbsParent is RogueAPI.Projectiles.ProjectileObj && (m_invincibleCounterProjectile <= 0f || (otherBox.AbsParent as RogueAPI.Projectiles.ProjectileObj).IgnoreInvincibleCounter))
                )
                {
                    //Show demented question mark and return
                    if (IsDemented)
                    {
                        m_invincibleCounter = InvincibilityTime;
                        m_invincibleCounterProjectile = InvincibilityTime;
                        RogueAPI.Effects.QuestionMarkEffect.Display(this);
                        //m_levelScreen.ImpactEffectPool.DisplayQuestionMark(new Vector2(X, Bounds.Top));
                        return;
                    }

                    int damage = (absParent as IDealsDamageObj).Damage;

                    //Apply critical damage
                    bool isPlayer = absParent == m_target;
                    if (isPlayer)
                    {
                        if (CDGMath.RandomFloat(0f, 1f) <= m_target.TotalCritChance && !NonKillable)
                        {
                            RogueAPI.Effects.CriticalTextEffect.Display(this);
                            //m_levelScreen.ImpactEffectPool.DisplayCriticalText(new Vector2(X, Bounds.Top));
                            damage = (int)(damage * m_target.TotalCriticalDamage);
                        }
                        m_invincibleCounter = InvincibilityTime;
                    }

                    //Destroy projectile
                    var projectileObj = otherBox.AbsParent as RogueAPI.Projectiles.ProjectileObj;
                    if (projectileObj != null)
                    {
                        m_invincibleCounterProjectile = InvincibilityTime;
                        if (projectileObj.DestroysWithEnemy && !NonKillable)
                            projectileObj.RunDestroyAnimation(false);
                    }

                    if (projectileObj != null || absParent.CollisionTypeTag != GameTypes.CollisionType_GLOBAL_DAMAGE_WALL || absParent.CollisionTypeTag == GameTypes.CollisionType_GLOBAL_DAMAGE_WALL && IsWeighted)
                    {
                        var allow = true;
                        if (projectileObj != null)
                        {
                            var kbAmount = CanBeKnockedBack && !IsPaused ? (KnockBack != Vector2.Zero ? KnockBack : m_target.EnemyKnockBack) : Vector2.Zero;
                            allow = projectileObj != null ? projectileObj.OnCollision(this, false, kbAmount) : true;
                        }

                        if (allow)
                        {
                            Point center = (thisBox.AbsRotation != 0f || otherBox.AbsRotation != 0f)
                                ? Rectangle.Intersect(thisBox.AbsParent.Bounds, otherBox.AbsParent.Bounds).Center
                                : Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;

                            HitEnemy(damage, new Vector2(center.X, center.Y), isPlayer);
                        }
                    }

                    ////Apply shout knockback
                    //if (projectileObj != null && projectileObj.Spell == SpellType.Shout)
                    //{
                    //    if (CanBeKnockedBack && !IsPaused)
                    //    {
                    //        float inertia = 3f;
                    //        var knockback = KnockBack != Vector2.Zero ? KnockBack : m_target.EnemyKnockBack;

                    //        CurrentSpeed = 0f;
                    //        AccelerationX = knockback.X * (X >= m_target.X ? inertia : -inertia);
                    //        AccelerationY = -knockback.Y * inertia;
                    //    }
                    //}
                    //else if (projectileObj != null || absParent.CollisionTypeTag != GameTypes.CollisionType_GLOBAL_DAMAGE_WALL || absParent.CollisionTypeTag == GameTypes.CollisionType_GLOBAL_DAMAGE_WALL && IsWeighted)
                    //{
                    //    Point center = (thisBox.AbsRotation != 0f || otherBox.AbsRotation != 0f)
                    //        ? Rectangle.Intersect(thisBox.AbsParent.Bounds, otherBox.AbsParent.Bounds).Center
                    //        : Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;

                    //    HitEnemy(damage, new Vector2(center.X, center.Y), isPlayer);
                    //}
                }

            }
            else if (collisionResponseType == Consts.COLLISIONRESPONSE_TERRAIN)
            {
                if ((absParent.CollisionTypeTag == GameTypes.CollisionType_WALL || absParent.CollisionTypeTag == GameTypes.CollisionType_WALL_FOR_ENEMY || absParent.CollisionTypeTag == GameTypes.CollisionType_GLOBAL_DAMAGE_WALL)
                    && CollisionTypeTag != GameTypes.CollisionType_ENEMYWALL)
                {
                    Vector2 vector2 = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);

                    if (CurrentSpeed != 0f && vector2.X != 0f && Math.Abs(vector2.X) > 10f && (vector2.X > 0f && absParent.CollidesRight || vector2.X < 0f && absParent.CollidesLeft))
                        CurrentSpeed = 0f;


                    if (m_numTouchingGrounds <= 1 && CurrentSpeed != 0f && vector2.Y < 0f && !CanFallOffLedges)
                    {
                        if (Bounds.Left < absParent.Bounds.Left && HeadingX < 0f)
                        {
                            X = absParent.Bounds.Left + (AbsX - Bounds.Left);
                            CurrentSpeed = 0f;
                        }
                        else if (Bounds.Right > absParent.Bounds.Right && HeadingX > 0f)
                        {
                            X = absParent.Bounds.Right - (Bounds.Right - AbsX);
                            CurrentSpeed = 0f;
                        }

                        m_isTouchingGround = true;
                    }


                    if (AccelerationX != 0f && m_isTouchingGround)
                        AccelerationX = 0f;

                    bool flag1 = false;
                    if (Math.Abs(vector2.X) < 10f && vector2.X != 0f && Math.Abs(vector2.Y) < 10f && vector2.Y != 0f)
                        flag1 = true;

                    if (m_isTouchingGround && !absParent.CollidesBottom && absParent.CollidesTop && absParent.TerrainBounds.Top < TerrainBounds.Bottom - 30)
                        flag1 = true;

                    if (!absParent.CollidesRight && !absParent.CollidesLeft && absParent.CollidesTop && absParent.CollidesBottom)
                        flag1 = true;

                    Vector2 vector22 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                    if (!flag1)
                        base.CollisionResponse(thisBox, otherBox, collisionResponseType);

                    if (vector22.Y < 0f && otherBox.AbsRotation != 0f && IsWeighted)
                        X -= vector22.X;
                }
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public virtual void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            if (m_target == null || m_target.CurrentHealth <= 0)
                return;

            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "EnemyHit1", "EnemyHit2", "EnemyHit3", "EnemyHit4", "EnemyHit5", "EnemyHit6");
            Blink(Color.Red, 0.1f);
            m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(collisionPt);

            //Apply damage and display text
            CurrentHealth -= damage;
            m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(X, Bounds.Top));


            if (isPlayer)
            {
                ////Apply mana gain
                //if (Game.PlayerStats.Class != ClassType.SpellSword || Game.PlayerStats.Class != ClassType.SpellSword2)
                //{
                //    var manaGain = (int)(damage * 0.3f);
                //    m_target.CurrentMana += manaGain;
                //    m_levelScreen.TextManager.DisplayNumberStringText(manaGain, "mp", Color.RoyalBlue, new Vector2(m_target.X, m_target.Bounds.Top - 30));
                //}

                //Apply down-strike bounce
                m_target.NumSequentialAttacks++;
                if (m_target.IsAirAttacking)
                {
                    m_target.IsAirAttacking = false;
                    m_target.AccelerationY = -m_target.AirAttackKnockBack;
                    m_target.NumAirBounces++;
                }
            }

            var evt = new RogueAPI.EnemyHitEventArgs(this, damage, isPlayer, CanBeKnockedBack && !IsPaused ? 1f : 0);
            RogueAPI.Event.Trigger(evt);

            if (evt.KnockbackForce != 0)
            {
                var knockback = KnockBack != Vector2.Zero ? KnockBack : m_target.EnemyKnockBack;

                CurrentSpeed = 0f;
                AccelerationX = knockback.X * (X >= m_target.X ? evt.KnockbackForce : -evt.KnockbackForce);
                AccelerationY = -knockback.Y * evt.KnockbackForce;
            }

            ////Apply knockback
            //if (CanBeKnockedBack && !IsPaused && Game.PlayerStats.Traits.X != TraitType.Hypogonadism && Game.PlayerStats.Traits.Y != TraitType.Hypogonadism)
            //{
            //    float inertia = (Game.PlayerStats.Traits.X == TraitType.Hypergonadism || Game.PlayerStats.Traits.Y == TraitType.Hypergonadism) ? 2f : 1f;
            //    var knockback = KnockBack != Vector2.Zero ? KnockBack : m_target.EnemyKnockBack;

            //    CurrentSpeed = 0f;
            //    AccelerationX = knockback.X * (X >= m_target.X ? inertia : -inertia);
            //    AccelerationY = -knockback.Y * inertia;
            //}

            m_levelScreen.SetLastEnemyHit(this);
        }
    }
}
