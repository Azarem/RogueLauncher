using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyTranslator;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.PlayerStats")]
    public class PlayerStats
    {
        //[Rewrite]
        //public byte Class { get { return 0; } }
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
        public bool ChallengeLastBossBeaten { get; set; }
        [Rewrite]
        public byte Spell { get; set; }
        [Rewrite]
        public bool IsFemale { get; set; }
        [Rewrite]
        public bool IsDead { get; set; }
        [Rewrite]
        public byte HeadPiece { get; set; }
        [Rewrite]
        public byte ShoulderPiece { get; set; }
        [Rewrite]
        public byte ChestPiece { get; set; }
        [Rewrite]
        public byte Age { get; set; }
        [Rewrite]
        public byte ChildAge { get; set; }
        [Rewrite]
        public Vector3 WizardSpellList { get; set; }
        [Rewrite]
        public List<PlayerLineageData> CurrentBranches;

        [Rewrite(action: RewriteAction.Add)]
        private Vector2 m_traits;
        [Rewrite(action: RewriteAction.Replace)]
        public Vector2 Traits
        {
            get { return m_traits; }
            set
            {
                m_traits = value;
                if (this == Game.PlayerStats)
                    RogueAPI.Game.Player.SetTraitVector(value);
            }
        }

        [Rewrite(action: RewriteAction.Add)]
        private byte m_class;
        [Rewrite(action: RewriteAction.Replace)]
        public byte Class
        {
            get { return m_class; }
            set
            {
                m_class = value;
                if (this == Game.PlayerStats)
                    RogueAPI.Game.Player.SetClassId(value);
            }
        }

        //[Rewrite]
        //public Vector2 Traits { get; set; }
        //[Rewrite]
        //public byte Class { get; set; }

        [Rewrite]
        public byte GetNumberOfEquippedRunes(int equipmentAbilityType) { return 0; }
        [Rewrite]
        public int Gold { get; set; }
    }
}
