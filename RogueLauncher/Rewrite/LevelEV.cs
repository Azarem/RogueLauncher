using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.LevelEV")]
    public class LevelEV
    {
        [Rewrite]
        public const int ENEMY_LEVEL_DIFFICULTY_MOD = 32;
        [Rewrite]
        public const float ENEMY_LEVEL_FAKE_MULTIPLIER = 2.75f;
        [Rewrite]
        public const int ROOM_LEVEL_MOD = 4;
        [Rewrite]
        public const byte TOTAL_JOURNAL_ENTRIES = 25;
        [Rewrite]
        public const int ENEMY_EXPERT_LEVEL_MOD = 4;
        [Rewrite]
        public const int ENEMY_MINIBOSS_LEVEL_MOD = 7;
        [Rewrite]
        public const int LAST_BOSS_MODE1_LEVEL_MOD = 8;
        [Rewrite]
        public const int LAST_BOSS_MODE2_LEVEL_MOD = 10;
        [Rewrite]
        public const int CASTLE_ROOM_LEVEL_BOOST = 0;
        [Rewrite]
        public const int GARDEN_ROOM_LEVEL_BOOST = 2;
        [Rewrite]
        public const int TOWER_ROOM_LEVEL_BOOST = 4;
        [Rewrite]
        public const int DUNGEON_ROOM_LEVEL_BOOST = 6;
        [Rewrite]
        public const int NEWGAMEPLUS_LEVEL_BASE = 128;
        [Rewrite]
        public const int NEWGAMEPLUS_LEVEL_APPRECIATION = 128;
        [Rewrite]
        public const int NEWGAMEPLUS_MINIBOSS_LEVEL_BASE = 0;
        [Rewrite]
        public const int NEWGAMEPLUS_MINIBOSS_LEVEL_APPRECIATION = 0;
        [Rewrite]
        public const bool LINK_TO_CASTLE_ONLY = true;
        [Rewrite]
        public const byte CASTLE_BOSS_ROOM = 1;
        [Rewrite]
        public const byte TOWER_BOSS_ROOM = 6;
        [Rewrite]
        public const byte DUNGEON_BOSS_ROOM = 7;
        [Rewrite]
        public const byte GARDEN_BOSS_ROOM = 5;
        [Rewrite]
        public const byte LAST_BOSS_ROOM = 2;
        [Rewrite]
        public const int LEVEL_CASTLE_LEFTDOOR = 90;
        [Rewrite]
        public const int LEVEL_CASTLE_RIGHTDOOR = 90;
        [Rewrite]
        public const int LEVEL_CASTLE_TOPDOOR = 90;
        [Rewrite]
        public const int LEVEL_CASTLE_BOTTOMDOOR = 90;
        [Rewrite]
        public const int LEVEL_GARDEN_LEFTDOOR = 70;
        [Rewrite]
        public const int LEVEL_GARDEN_RIGHTDOOR = 100;
        [Rewrite]
        public const int LEVEL_GARDEN_TOPDOOR = 45;
        [Rewrite]
        public const int LEVEL_GARDEN_BOTTOMDOOR = 45;
        [Rewrite]
        public const int LEVEL_TOWER_LEFTDOOR = 45;
        [Rewrite]
        public const int LEVEL_TOWER_RIGHTDOOR = 45;
        [Rewrite]
        public const int LEVEL_TOWER_TOPDOOR = 100;
        [Rewrite]
        public const int LEVEL_TOWER_BOTTOMDOOR = 60;
        [Rewrite]
        public const int LEVEL_DUNGEON_LEFTDOOR = 55;
        [Rewrite]
        public const int LEVEL_DUNGEON_RIGHTDOOR = 55;
        [Rewrite]
        public const int LEVEL_DUNGEON_TOPDOOR = 45;
        [Rewrite]
        public const int LEVEL_DUNGEON_BOTTOMDOOR = 100;
        [Rewrite]
        public const string GAME_VERSION = "v1.2.0b";
        [Rewrite]
        public static byte[] DEMENTIA_FLIGHT_LIST;
        [Rewrite]
        public static byte[] DEMENTIA_GROUND_LIST;
        [Rewrite]
        public static byte[] CASTLE_ENEMY_LIST;
        [Rewrite]
        public static byte[] GARDEN_ENEMY_LIST;
        [Rewrite]
        public static byte[] TOWER_ENEMY_LIST;
        [Rewrite]
        public static byte[] DUNGEON_ENEMY_LIST;
        [Rewrite]
        public static byte[] CASTLE_ENEMY_DIFFICULTY_LIST;
        [Rewrite]
        public static byte[] GARDEN_ENEMY_DIFFICULTY_LIST;
        [Rewrite]
        public static byte[] TOWER_ENEMY_DIFFICULTY_LIST;
        [Rewrite]
        public static byte[] DUNGEON_ENEMY_DIFFICULTY_LIST;
        [Rewrite]
        public static string[] CASTLE_ASSETSWAP_LIST;
        [Rewrite]
        public static string[] DUNGEON_ASSETSWAP_LIST;
        [Rewrite]
        public static string[] TOWER_ASSETSWAP_LIST;
        [Rewrite]
        public static string[] GARDEN_ASSETSWAP_LIST;
        [Rewrite]
        public static bool SHOW_ENEMY_RADII;
        [Rewrite]
        public static bool ENABLE_PLAYER_DEBUG;
        [Rewrite]
        public static bool UNLOCK_ALL_ABILITIES;
        [Rewrite]
        public static GameTypes.LevelType TESTROOM_LEVELTYPE;
        [Rewrite]
        public static bool TESTROOM_REVERSE;
        [Rewrite]
        public static bool RUN_TESTROOM;
        [Rewrite]
        public static bool SHOW_DEBUG_TEXT;
        [Rewrite]
        public static bool LOAD_TITLE_SCREEN;
        [Rewrite]
        public static bool LOAD_SPLASH_SCREEN;
        [Rewrite]
        public static bool SHOW_SAVELOAD_DEBUG_TEXT;
        [Rewrite]
        public static bool DELETE_SAVEFILE;
        [Rewrite]
        public static bool CLOSE_TESTROOM_DOORS;
        [Rewrite]
        public static bool RUN_TUTORIAL;
        [Rewrite]
        public static bool RUN_DEMO_VERSION;
        [Rewrite]
        public static bool DISABLE_SAVING;
        [Rewrite]
        public static bool RUN_CRASH_LOGS;
        [Rewrite]
        public static bool WEAKEN_BOSSES;
        [Rewrite]
        public static bool ENABLE_OFFSCREEN_CONTROL;
        [Rewrite]
        public static bool ENABLE_BACKUP_SAVING;
        [Rewrite]
        public static bool CREATE_RETAIL_VERSION;
        [Rewrite]
        public static bool SHOW_FPS;
        [Rewrite]
        public static bool SAVE_FRAMES;
    }
}
