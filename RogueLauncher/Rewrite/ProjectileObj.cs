using System;
using System.Linq;
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
        public ProjectileObj(string spriteName) : base(spriteName, null) { }

        [Rewrite]
        public void RunDisplacerEffect(RoomObj room, PlayerObj player) { }
        [Rewrite]
        public void KillProjectile() { }
    }
}
