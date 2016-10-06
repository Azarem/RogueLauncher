using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.RaindropObj")]
    public class RaindropObj : SpriteObj
    {
        [Rewrite]
        private float m_speedY;
        [Rewrite]
        private float m_speedX;
        [Rewrite]
        private Vector2 m_startingPos;
        [Rewrite]
        private bool m_splashing;
        [Rewrite]
        private bool m_isSnowflake;
        [Rewrite]
        private bool m_isParticle;

        [Rewrite]
        public bool IsCollidable { get; set; }
        [Rewrite]
        public Vector2 MaxXSpeed { get; set; }
        [Rewrite]
        public Vector2 MaxYSpeed { get; set; }

        [Rewrite]
        public RaindropObj(Vector2 startingPos) : base("Raindrop_Sprite") { }

        [Rewrite]
        public void ChangeToParticle() { }

        [Rewrite]
        public void ChangeToRainDrop() { }

        [Rewrite]
        public void ChangeToSnowflake() { }

        [Rewrite]
        public void KillDrop() { }

        [Rewrite]
        private void RunSplashAnimation() { }

        [Rewrite]
        public void Update(List<TerrainObj> collisionList, GameTime gameTime) { }

        [Rewrite]
        public void UpdateNoCollision(GameTime gameTime) { }
    }
}
