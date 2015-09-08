using System;
using System.Linq;
using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SaveType")]
    public enum SaveType
    {
        [Rewrite]
        None,
        [Rewrite]
        PlayerData,
        [Rewrite]
        UpgradeData,
        [Rewrite]
        Map,
        [Rewrite]
        MapData,
        [Rewrite]
        Lineage
    }
}
