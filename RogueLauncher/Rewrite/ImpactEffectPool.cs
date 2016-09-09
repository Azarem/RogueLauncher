using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ImpactEffectPool")]
    public class ImpactEffectPool
    {
        [Rewrite]
        private int m_poolSize;

        [Rewrite]
        private DS2DPool<SpriteObj> m_resourcePool;

        [Rewrite]
        private bool m_isPaused;

        [Rewrite]
        private bool m_isDisposed;

        [Rewrite]
        public int ActiveTextObjs { get { return this.m_resourcePool.NumActiveObjs; } }

        [Rewrite]
        public int CurrentPoolSize { get { return this.TotalPoolSize - this.ActiveTextObjs; } }

        [Rewrite]
        public bool IsDisposed { get { return this.m_isDisposed; } }

        [Rewrite]
        public int TotalPoolSize { get { return m_resourcePool.TotalPoolSize; } }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void SpellCastEffect(Vector2 pos, float angle, bool megaSpell)
        {
            RogueAPI.Effects.SpellCastEffect.Display(pos, angle);
        }

        [Rewrite]
        public void StartInverseEmit(Vector2 pos) { }

        //[Rewrite]
        //public void DisplayQuestionMark(Vector2 pos) { }

        //NO NEED, TAKEN CARE OF BY CLASS
        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        //public void DisplayChestSparkleEffect(Vector2 position)
        //{
        //    RogueAPI.Effects.ChestSparkleEffect.Display(position);
        //}

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayCriticalText(Vector2 position)
        {
            RogueAPI.Effects.CriticalTextEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayDashEffect(Vector2 position, bool flip)
        {
            RogueAPI.Effects.DashEffect.Display(position, flip);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayDeathEffect(Vector2 position)
        {
            RogueAPI.Effects.DeathEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayDoubleJumpEffect(Vector2 position)
        {
            RogueAPI.Effects.DoubleJumpEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayDustEffect(GameObj obj)
        {
            RogueAPI.Effects.DustEffect.Display(obj);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayDustEffect(Vector2 pos)
        {
            RogueAPI.Effects.DustEffect.Display(pos);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayEarthParticleEffect(GameObj sprite)
        {
            RogueAPI.Effects.EarthParticleEffect.Display(sprite);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayEnemyImpactEffect(Vector2 position)
        {
            RogueAPI.Effects.EnemyImpactEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayExplosionEffect(Vector2 position)
        {
            RogueAPI.Effects.ExplosionEffect.Display(position);
        }

        //[Rewrite]
        //public void BlackSmokeEffect(GameObj obj) { }



        [Rewrite]
        public void DisplayFartEffect(GameObj obj) { }
        [Rewrite]
        public void Draw(Camera2D camera) { }
        [Rewrite]
        public void DisplayFusRoDahText(Vector2 position) { }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public SpriteObj CheckOut()
        {
            return m_resourcePool.CheckOut();
        }
    }
}
