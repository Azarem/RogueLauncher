using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.TeleporterObj")]
    public class TeleporterObj : PhysicsObj
    {
        [Rewrite]
        private SpriteObj m_arrowIcon;
        [Rewrite]
        public bool Activated { get; set; }

        [Rewrite]
        public TeleporterObj() : base("TeleporterBase_Sprite", null) { }

        [Rewrite]
        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) { }

        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }

        [Rewrite]
        public override void Dispose() { }

        [Rewrite]
        public override void Draw(Camera2D camera) { }

        [Rewrite]
        protected override void FillCloneInstance(object obj) { }

        [Rewrite]
        public void SetCollision(bool collides) { }
    }
}
