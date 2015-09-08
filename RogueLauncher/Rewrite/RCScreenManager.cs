using System;
using System.Linq;
using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.RCScreenManager")]
    public class RCScreenManager
    {
        [Rewrite]
        public PlayerObj Player { get { return null; } }
    }
}
