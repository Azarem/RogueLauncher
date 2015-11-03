using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EnemyObj")]
    public class EnemyObj : CharacterObj
    {
        [Rewrite]
        public bool NonKillable { get; set; }
        [Rewrite]
        public byte Type;

        [Rewrite]
        public EnemyObj(string spriteName, PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base(spriteName, physicsManager, levelToAttachTo)
        {
        }

        [Rewrite]
        public void DrawDetectionRadii(Camera2D camera) { }
    }
}
