using System;
using System.Linq;
using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SaveGameManager")]
    public class SaveGameManager
    {
        [Rewrite]
        public void SaveFiles(params SaveType[] saveList) { }
    }
}
