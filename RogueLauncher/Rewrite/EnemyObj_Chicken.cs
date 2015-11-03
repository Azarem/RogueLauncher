using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EnemyObj_Chicken")]
    public class EnemyObj_Chicken : EnemyObj
    {
        [Rewrite]
        public EnemyObj_Chicken(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyChickenRun_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = 26;
        }
    }
}
