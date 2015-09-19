using System;
using System.Linq;
using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProceduralLevelScreen")]
    public class ProceduralLevelScreen : Screen
    {
        [Rewrite]
        public RoomObj CurrentRoom { get { return null; } }
        [Rewrite]
        public TextManager TextManager { get { return null; } }
        [Rewrite]
        public ProjectileManager ProjectileManager { get { return null; } }
        [Rewrite]
        public ImpactEffectPool ImpactEffectPool { get { return null; } }
        [Rewrite]
        public ItemDropManager ItemDropManager { get { return null; } }

        [Rewrite]
        public void UpdatePlayerSpellIcon() { }
        [Rewrite]
        public void CastTimeStop(float duration) { }
        [Rewrite]
        public void StopTimeStop() { }
        [Rewrite]
        public void AddEnemyToCurrentRoom(EnemyObj enemy) { }
    }
}
