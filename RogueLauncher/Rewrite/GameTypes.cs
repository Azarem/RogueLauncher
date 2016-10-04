using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.GameTypes")]
    public class GameTypes
    {
        [Rewrite]
        public const int CollisionType_NULL = 0;
        [Rewrite]
        public const int CollisionType_WALL = 1;
        [Rewrite]
        public const int CollisionType_PLAYER = 2;
        [Rewrite]
        public const int CollisionType_ENEMY = 3;
        [Rewrite]
        public const int CollisionType_ENEMYWALL = 4;
        [Rewrite]
        public const int CollisionType_WALL_FOR_PLAYER = 5;
        [Rewrite]
        public const int CollisionType_WALL_FOR_ENEMY = 6;
        [Rewrite]
        public const int CollisionType_PLAYER_TRIGGER = 7;
        [Rewrite]
        public const int CollisionType_ENEMY_TRIGGER = 8;
        [Rewrite]
        public const int CollisionType_GLOBAL_TRIGGER = 9;
        [Rewrite]
        public const int CollisionType_GLOBAL_DAMAGE_WALL = 10;
        [Rewrite]
        public const int LogicSetType_NULL = 0;
        [Rewrite]
        public const int LogicSetType_NONATTACK = 1;
        [Rewrite]
        public const int LogicSetType_ATTACK = 2;
        [Rewrite]
        public const int LogicSetType_CD = 3;

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

        [Rewrite("RogueCastle.GameTypes+DoorType")]
        public enum DoorType
        {
            [Rewrite]
            NULL,
            [Rewrite]
            OPEN,
            [Rewrite]
            LOCKED,
            [Rewrite]
            BLOCKED
        }
    }
}
