using System;
using System.Linq;
using AssemblyTranslator;
using RogueAPI.Projectiles;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProjectileManager")]
    public class ProjectileManager
    {
        [Rewrite]
        public ProjectileObj FireProjectile(ProjectileInstance data) { return null; }
    }
}
