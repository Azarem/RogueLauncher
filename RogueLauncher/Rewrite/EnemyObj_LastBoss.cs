using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EnemyObj_LastBoss")]
    public class EnemyObj_LastBoss : EnemyObj
    {
        [Rewrite]
        public EnemyObj_LastBoss(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("PlayerIdle_Character", target, physicsManager, levelToAttachTo, difficulty) { }
    }
}
