using System;
using System.Linq;
using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.GameUtil")]
    public static class GameUtil
    {
        [Rewrite]
        public static void UnlockAchievement(string achievementName)
        {
            //SWManager.instance().unlockAchievement(achievementName);
        }
    }
}
