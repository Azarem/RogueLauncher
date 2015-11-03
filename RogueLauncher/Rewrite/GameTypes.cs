using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.GameTypes")]
    public class GameTypes
    {
        [Rewrite("RogueCastle.GameTypes+EnemyDifficulty")]
        public enum EnemyDifficulty
        {
            [Rewrite]
            BASIC,
            [Rewrite]
            ADVANCED,
            [Rewrite]
            EXPERT,
            [Rewrite]
            MINIBOSS
        }

        [Rewrite("RogueCastle.GameTypes+LevelType")]
        public enum LevelType
        {
            [Rewrite]
            NONE,
            [Rewrite]
            CASTLE,
            [Rewrite]
            GARDEN,
            [Rewrite]
            DUNGEON,
            [Rewrite]
            TOWER
        }
    }
}
