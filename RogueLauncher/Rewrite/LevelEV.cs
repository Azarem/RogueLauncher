using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.LevelEV")]
    public class LevelEV
    {
        [Rewrite]
        public static bool RUN_TESTROOM;
        [Rewrite]
        public static bool CREATE_RETAIL_VERSION;
        [Rewrite]
        public static bool RUN_DEMO_VERSION;
        [Rewrite]
        public static bool SHOW_ENEMY_RADII;
        [Rewrite]
        public static bool SAVE_FRAMES;
    }
}
