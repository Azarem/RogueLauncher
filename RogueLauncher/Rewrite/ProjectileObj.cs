using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProjectileObj")]
    public class ProjectileObj : PhysicsObj
    {
        [Rewrite]
        public float LifeSpan;
        [Rewrite]
        public int Spell { get; set; }
        [Rewrite]
        public float AltX { get; set; }
        [Rewrite]
        public float AltY { get; set; }
        [Rewrite]
        public bool ShowIcon { get; set; }
        [Rewrite]
        public float BlinkTime { get; set; }
        [Rewrite]
        public bool IgnoreBoundsCheck { get; set; }
        [Rewrite]
        public GameObj Target { get; set; }
        [Rewrite]
        public bool WrapProjectile { get; set; }
        [Rewrite]
        public bool ChaseTarget { get; set; }
        [Rewrite]
        public GameObj Source { get; set; }
        [Rewrite]
        public bool CollidesWithTerrain { get; set; }
        [Rewrite]
        public bool DestroysWithTerrain { get; set; }
        [Rewrite]
        public bool DestroysWithEnemy { get; set; }
        [Rewrite]
        public bool FollowArc { get; set; }
        [Rewrite]
        public bool CollidesWith1Ways { get; set; }
        [Rewrite]
        public bool DestroyOnRoomTransition { get; set; }
        [Rewrite]
        public bool CanBeFusRohDahed { get; set; }
        [Rewrite]
        public bool IgnoreInvincibleCounter { get; set; }

        [Rewrite]
        public ProjectileObj(string spriteName) : base(spriteName, null) { }

        [Rewrite]
        public void RunDisplacerEffect(RoomObj room, PlayerObj player) { }
        [Rewrite]
        public void KillProjectile() { }
        [Rewrite]
        public void Reset() { }
        [Rewrite]
        public void UpdateHeading() { }
    }
}
