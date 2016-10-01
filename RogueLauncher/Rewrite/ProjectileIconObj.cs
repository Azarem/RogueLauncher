using AssemblyTranslator;
using DS2DEngine;
using RogueAPI.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProjectileIconObj")]
    class ProjectileIconObj : GameObj
    {
        [Rewrite]
        private SpriteObj m_iconBG;

        [Rewrite]
        private SpriteObj m_iconProjectile;

        [Rewrite]
        private ProjectileObj m_attachedProjectile;

        [Rewrite]
        private int m_iconOffset = 60;

        [Rewrite]
        public ProjectileObj AttachedProjectile { get; set; }

        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }
    }
}
