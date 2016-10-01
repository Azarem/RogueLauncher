using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ImpactEffectPool")]
    public class ImpactEffectPool
    {
        [Rewrite]
        private int m_poolSize;

        [Rewrite(action: RewriteAction.Replace)]
        private DS2DPool<RogueAPI.Effects.EffectSpriteInstance> m_resourcePool;

        [Rewrite]
        private bool m_isPaused;

        [Rewrite]
        private bool m_isDisposed;

        [Rewrite(action: RewriteAction.Replace)]
        public int ActiveTextObjs { get { return this.m_resourcePool.NumActiveObjs; } }

        [Rewrite]
        public int CurrentPoolSize { get { return this.TotalPoolSize - this.ActiveTextObjs; } }

        [Rewrite]
        public bool IsDisposed { get { return this.m_isDisposed; } }

        [Rewrite(action: RewriteAction.Replace)]
        public int TotalPoolSize { get { return m_resourcePool.TotalPoolSize; } }


        [Rewrite(action: RewriteAction.Replace)]
        public ImpactEffectPool(int poolSize)
        {
            m_poolSize = poolSize;
            m_resourcePool = new DS2DPool<RogueAPI.Effects.EffectSpriteInstance>();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void SpellCastEffect(Vector2 pos, float angle, bool megaSpell)
        {
            RogueAPI.Effects.SpellCastEffect.Display(pos, angle);
        }


        //[Rewrite]
        //public void DisplayQuestionMark(Vector2 pos) { }

        //NO NEED, TAKEN CARE OF BY CLASS
        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        //public void DisplayChestSparkleEffect(Vector2 position)
        //{
        //    RogueAPI.Effects.ChestSparkleEffect.Display(position);
        //}

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Remove)]
        public void DestroyAllEffects()
        {
            foreach (SpriteObj activeObjsList in m_resourcePool.ActiveObjsList)
                activeObjsList.StopAnimation();
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void Initialize()
        {
            for (int i = 0; i < m_poolSize; i++)
                m_resourcePool.AddToPool(new RogueAPI.Effects.EffectSpriteInstance("Blank_Sprite")
                {
                    AnimationDelay = 0.0333333351f,
                    Visible = false,
                    TextureColor = Color.White
                });
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void IntroSmokeEffect(Vector2 position)
        {
            RogueAPI.Effects.BlackSmokeEffect.DisplayIntroSmoke(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayPlayerImpactEffect(Vector2 position)
        {
            RogueAPI.Effects.ImpactEffect.Display(position, true);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayMusicNote(Vector2 position)
        {
            RogueAPI.Effects.MusicNoteEffect.Display(position);
        }

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
            RogueAPI.Effects.ImpactEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayExplosionEffect(Vector2 position)
        {
            RogueAPI.Effects.ExplosionEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayChestSparkleEffect(Vector2 position)
        {
            RogueAPI.Effects.ChestSparkleEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void CrowSmokeEffect(Vector2 position)
        {
            RogueAPI.Effects.BlackSmokeEffect.DisplayCrowSmoke(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayBlockImpactEffect(Vector2 position, Vector2 scale)
        {
            RogueAPI.Effects.BlockImpactEffect.Display(position, scale);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplaySpawnEffect(Vector2 position)
        {
            RogueAPI.Effects.SpawnEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayTeleportEffect(Vector2 position)
        {
            RogueAPI.Effects.TeleportRockEffect.Display(position);
            RogueAPI.Effects.TeleportBeamEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void CrowDestructionEffect(Vector2 position)
        {
            RogueAPI.Effects.BlackSmokeEffect.DisplayCrowDestruction(position);
            RogueAPI.Effects.CrowFeatherEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayFusRoDahText(Vector2 position)
        {
            RogueAPI.Effects.FusRoDahTextEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayQuestionMark(Vector2 position)
        {
            RogueAPI.Effects.QuestionMarkEffect.Display(position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void BlackSmokeEffect(GameObj obj)
        {
            RogueAPI.Effects.BlackSmokeEffect.Display(obj);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void BlackSmokeEffect(Vector2 pos, Vector2 scale)
        {
            RogueAPI.Effects.BlackSmokeEffect.Display(pos, scale: scale.X, longerDuration: true, furtherDrift: true);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DoorSparkleEffect(Rectangle rect)
        {
            RogueAPI.Effects.DoorSparkleEffect.Display(rect);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayFartEffect(GameObj obj)
        {
            RogueAPI.Effects.DustEffect.DisplayFart(obj);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayTanookiEffect(GameObj obj)
        {
            RogueAPI.Effects.TanukiEffect.Display(obj.Position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayFountainShatterSmoke(GameObj sprite)
        {
            RogueAPI.Effects.FountainShatterSmokeEffect.Display(sprite);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void MegaTeleport(Vector2 pos, Vector2 scale)
        {
            RogueAPI.Effects.MegaTeleportEffect.Display(pos, scale);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void MegaTeleportReverse(Vector2 pos, Vector2 scale)
        {
            RogueAPI.Effects.MegaTeleportReverseEffect.Display(pos, scale);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayFireParticleEffect(GameObj sprite)
        {
            RogueAPI.Effects.FireParticleEffect.Display(sprite);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DisplayIceParticleEffect(GameObj sprite)
        {
            RogueAPI.Effects.IceParticleEffect.Display(sprite);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void TurretFireEffect(Vector2 pos, Vector2 scale)
        {
            RogueAPI.Effects.DustEffect.DisplayTurretFire(pos, scale.X);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void StartInverseEmit(Vector2 pos)
        {
            RogueAPI.Effects.InverseEmitEffect.Display(pos);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void SkillTreeDustEffect(Vector2 pos, bool horizontal, float length)
        {
            RogueAPI.Effects.SkillTreeDustEffect.Display(pos, horizontal, length);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void AssassinCastEffect(Vector2 pos)
        {
            RogueAPI.Effects.AssassinSmokeEffect.Display(pos);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void WoodChipEffect(Vector2 pos)
        {
            RogueAPI.Effects.WoodChipEffect.Display(pos);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void StartTranslocateEmit(Vector2 pos)
        {
            RogueAPI.Effects.TranslocateEffect.Display(pos);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void CarnivalGoldEffect(Vector2 startPos, Vector2 endPos, int numCoins)
        {
            RogueAPI.Effects.CarnivalGoldEffect.Display(startPos, endPos, numCoins);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void NinjaDisappearEffect(GameObj obj)
        {
            RogueAPI.Effects.LogEffect.Display(obj);
            RogueAPI.Effects.NinjaSmokeEffect.Display(obj);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void NinjaAppearEffect(GameObj obj)
        {
            RogueAPI.Effects.NinjaSmokeEffect.Display(obj);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void LoadingGateSmokeEffect(int numEntities)
        {
            RogueAPI.Effects.LoadingGateSmokeEffect.Display(numEntities);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DestroyFireballBoss(Vector2 pos)
        {
            RogueAPI.Effects.DestroyFireballEffect.Display(pos);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void LastBossSpellCastEffect(GameObj obj, float angle, bool megaSpell)
        {
            RogueAPI.Effects.SpellCastEffect.Display(obj, angle, false, megaSpell);
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Remove)]
        public SpriteObj DisplayEffect(Vector2 position, string spriteName)
        {
            var proj = m_resourcePool.CheckOut();
            proj.ChangeSprite(spriteName);
            proj.TextureColor = Color.White;
            proj.Visible = true;
            proj.Position = position;
            proj.PlayAnimation(false);
            return proj;
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void Draw(Camera2D camera)
        {
            for (int i = 0; i < m_resourcePool.ActiveObjsList.Count; i++)
            {
                if (m_resourcePool.ActiveObjsList[i].IsAnimating || m_isPaused)
                    m_resourcePool.ActiveObjsList[i].Draw(camera);
                else
                {
                    DestroyEffect(m_resourcePool.ActiveObjsList[i]);
                    i--;
                }
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DestroyEffect(RogueAPI.Effects.EffectSpriteInstance obj)
        {
            obj.OutlineWidth = 0;
            obj.Visible = false;
            obj.Rotation = 0f;
            obj.TextureColor = Color.White;
            obj.Opacity = 1f;
            m_resourcePool.CheckIn(obj);
            obj.Flip = SpriteEffects.None;
            obj.Scale = new Vector2(1f);
            obj.AnimationDelay = 0.0333333351f;
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Impact Effect Pool");
                m_isDisposed = true;
                m_resourcePool.Dispose();
                m_resourcePool = null;
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void PauseAllAnimations()
        {
            m_isPaused = true;
            foreach (SpriteObj activeObjsList in m_resourcePool.ActiveObjsList)
                activeObjsList.PauseAnimation();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void ResumeAllAnimations()
        {
            m_isPaused = false;
            foreach (SpriteObj activeObjsList in m_resourcePool.ActiveObjsList)
                activeObjsList.ResumeAnimation();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        public RogueAPI.Effects.EffectSpriteInstance CheckOut()
        {
            return m_resourcePool.CheckOut();
        }
    }
}
