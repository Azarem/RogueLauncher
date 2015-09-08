using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.PlayerStats")]
    public class PlayerStats
    {
        [Rewrite]
        public byte Class { get { return 0; } }
        [Rewrite]
        public sbyte[] GetEquippedArray { get { return null; } }
        [Rewrite]
        public List<byte[]> GetBlueprintArray { get { return null; } }
        [Rewrite]
        public string PlayerName { get; set; }
        [Rewrite]
        public bool HasArchitectFee { get; set; }
        [Rewrite]
        public int TimesCastleBeaten { get; set; }

        [Rewrite]
        public byte GetNumberOfEquippedRunes(int equipmentAbilityType) { return 0; }
        [Rewrite]
        public int Gold { get; set; }
    }
}
