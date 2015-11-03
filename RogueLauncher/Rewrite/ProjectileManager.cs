using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueAPI.Projectiles;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    //[Obfuscation(Exclude = true)]
    [Rewrite("RogueCastle.ProjectileManager")]
    public class ProjectileManager
    {
        [Rewrite]
        private DS2DPool<RogueAPI.Projectiles.ProjectileObj> m_projectilePool;
        [Rewrite]
        private ProceduralLevelScreen m_levelScreen;
        [Rewrite]
        private List<RogueAPI.Projectiles.ProjectileObj> m_projectilesToRemoveList;
        [Rewrite]
        private int m_poolSize;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public RogueAPI.Projectiles.ProjectileObj FireProjectile(ProjectileInstance data) { return FireProjectile(data, data.Source, data.Target); }

        //[Rewrite(action: RewriteAction.Replace)]
        //public ProjectileManager(ProceduralLevelScreen level, int poolSize)
        //{
        //    m_projectilesToRemoveList = new List<ProjectileObj>();
        //    m_levelScreen = level;
        //    m_projectilePool = new DS2DPool<ProjectileObj>();
        //    m_poolSize = poolSize;
        //}

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public RogueAPI.Projectiles.ProjectileObj FireProjectile(ProjectileDefinition data, GameObj source, GameObj target)
        {
            if (source == null)
                throw new Exception("Cannot have a projectile with no source");

            var proj = m_projectilePool.CheckOut();
            proj.Reset();
            proj.LifeSpan = data.Lifespan;
            //GameObj source = data.Source;
            proj.ChaseTarget = data.ChaseTarget;
            proj.Source = source;
            proj.Target = target;
            proj.UpdateHeading();
            proj.TurnSpeed = data.TurnSpeed;
            proj.CollidesWithTerrain = data.CollidesWithTerrain;
            proj.DestroysWithTerrain = data.DestroysWithTerrain;
            proj.DestroysWithEnemy = data.DestroysWithEnemy;
            proj.FollowArc = data.FollowArc;
            proj.Orientation = MathHelper.ToRadians(data.StartingRotation);
            proj.ShowIcon = data.ShowIcon;
            proj.IsCollidable = data.IsCollidable;
            proj.CollidesWith1Ways = data.CollidesWith1Ways;
            proj.DestroyOnRoomTransition = data.DestroyOnRoomTransition;
            proj.CanBeFusRohDahed = data.CanBeFusRohDahed;
            proj.IgnoreInvincibleCounter = data.IgnoreInvincibleCounter;
            proj.WrapProjectile = data.WrapProjectile;

            float x = 0f;
            if (data.Target == null)
            {
                x = data.Angle.X + data.AngleOffset;
                if (data.Angle.X != data.Angle.Y)
                    x = CDGMath.RandomFloat(data.Angle.X, data.Angle.Y) + data.AngleOffset;

                if (source.Flip != SpriteEffects.None && source.Rotation != 0f)
                    x -= 180f;
                else if (source.Flip != SpriteEffects.None && source.Rotation == 0f)
                    x = 180f - x;
            }
            else
            {
                float single = data.Target.X - source.X;
                float y = data.Target.Y - source.Y - data.SourceAnchor.Y;
                if (source.Flip != SpriteEffects.FlipHorizontally)
                {
                    x = MathHelper.ToDegrees((float)Math.Atan2(y, single));
                    single -= data.SourceAnchor.X;
                    x += data.AngleOffset;
                }
                else
                {
                    x = 180f - x;
                    single += data.SourceAnchor.X;
                    x = MathHelper.ToDegrees((float)Math.Atan2(y, single));
                    x -= data.AngleOffset;
                }
            }

            if (!data.LockPosition)
                proj.Rotation = x;

            x = MathHelper.ToRadians(x);
            proj.Damage = data.Damage;
            m_levelScreen.PhysicsManager.AddObject(proj);
            proj.ChangeSprite(data.SpriteName);
            proj.RotationSpeed = data.RotationSpeed;
            proj.Visible = true;

            if (source.Flip == SpriteEffects.None)
                proj.X = source.AbsX + data.SourceAnchor.X;
            else
                proj.X = source.AbsX - data.SourceAnchor.X;

            proj.Y = source.AbsY + data.SourceAnchor.Y;
            proj.IsWeighted = data.IsWeighted;

            Vector2 vector2 = new Vector2((float)Math.Cos(x), (float)Math.Sin(x));

            float speed = data.Speed.X != data.Speed.Y
                ? CDGMath.RandomFloat(data.Speed.X, data.Speed.Y)
                : data.Speed.X;

            proj.AccelerationX = vector2.X * speed;
            proj.AccelerationY = vector2.Y * speed;
            proj.CurrentSpeed = speed;

            if (!(source is PlayerObj))
            {
                if (proj.LifeSpan == 0f)
                    proj.LifeSpan = 15f;

                proj.CollisionTypeTag = 3;
                proj.Scale = data.Scale;
            }
            else
            {
                if (proj.LifeSpan == 0f)
                    proj.LifeSpan = (source as PlayerObj).ProjectileLifeSpan;

                proj.CollisionTypeTag = 2;
                proj.Scale = data.Scale;
            }

            if (data.Target != null && source.Flip == SpriteEffects.FlipHorizontally && data.ChaseTarget)
                proj.Orientation = MathHelper.ToRadians(180f);

            RogueAPI.Event<RogueAPI.ProjectileFireEvent>.Trigger(new RogueAPI.ProjectileFireEvent(source, data, proj));

            //if (source is PlayerObj && (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f))
            //{
            //    proj.AccelerationX *= -1f;
            //    if (!data.LockPosition)
            //        proj.Flip = source.Flip != SpriteEffects.FlipHorizontally
            //            ? SpriteEffects.FlipHorizontally
            //            : SpriteEffects.None;
            //}

            proj.PlayAnimation(true);
            return proj;
        }

        [Rewrite]
        public void Draw(Camera2D camera) { }
    }
}
