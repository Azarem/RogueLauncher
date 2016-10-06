using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.CharacterObj")]
    public class CharacterObj : PhysicsObjContainer
    {
        [Rewrite]
        protected ProceduralLevelScreen m_levelScreen;
        [Rewrite]
        protected bool m_isTouchingGround;
        [Rewrite]
        public int State { get; set; }
        [Rewrite]
        public bool IsKilled { get { return false; } }
        [Rewrite]
        public float JumpHeight { get; set; }
        [Rewrite]
        public float DoubleJumpHeight { get; internal set; }
        [Rewrite]
        public int CurrentHealth { get; set; }
        [Rewrite]
        public bool CanBeKnockedBack { get; set; }
        [Rewrite]
        public Vector2 KnockBack { get; internal set; }
        [Rewrite]
        public bool IsTouchingGround { get; }

        [Rewrite]
        public CharacterObj(string spriteName, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo)
            : base(spriteName, physicsManager)
        {

        }

        [Rewrite]
        public virtual void HandleInput() { }

        [Rewrite]
        public virtual void Update(GameTime gameTime) { }
        [Rewrite]
        public void Blink(Color blinkColour, float duration) { }
        [Rewrite]
        public virtual void Kill(bool giveXP = true) { }
    }
}
