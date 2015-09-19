using System;
using System.Linq;
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
    }
}
