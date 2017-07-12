using AssemblyTranslator;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
        [Rewrite(action: RewriteAction.Replace)]
        public byte Spell
        {
            get
            {
                return RogueAPI.Game.Player.Spell?.SpellId ?? 0;
            }
            set
            {
                RogueAPI.Game.Player.Spell = RogueAPI.Spells.SpellDefinition.GetById(value);
            }
        }
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
        public byte SpecialItem { get; set; }
        [Rewrite]
        public bool TutorialComplete { get; set; }
        [Rewrite]
        public List<PlayerLineageData> CurrentBranches;
        [Rewrite]
        public int CurrentHealth { get; set; }
        [Rewrite]
        public int CurrentMana { get; set; }
        [Rewrite]
        public bool EyeballBossBeaten { get; set; }
        [Rewrite]
        public bool FairyBossBeaten { get; set; }
        [Rewrite]
        public bool FinalDoorOpened { get; set; }
        [Rewrite]
        public bool FireballBossBeaten { get; set; }
        [Rewrite]
        public bool BlobBossBeaten { get; set; }
        [Rewrite]
        public bool LastbossBeaten { get; set; }
        [Rewrite]
        public bool ChallengeBlobBeaten { get; set; }
        [Rewrite]
        public bool ChallengeBlobUnlocked { get; set; }
        [Rewrite]
        public bool ChallengeEyeballBeaten { get; set; }
        [Rewrite]
        public bool ChallengeEyeballUnlocked { get; set; }
        [Rewrite]
        public bool ChallengeFireballBeaten { get; set; }
        [Rewrite]
        public bool ChallengeFireballUnlocked { get; set; }
        [Rewrite]
        public bool ChallengeLastBossUnlocked { get; set; }
        [Rewrite]
        public bool ChallengeSkullBeaten { get; set; }
        [Rewrite]
        public bool ChallengeSkullUnlocked { get; set; }
        [Rewrite]
        public float TotalHoursPlayed { get; set; }
        [Rewrite]
        public byte TotalRunesFound { get { return 0; } }
        [Rewrite]
        public byte TotalBlueprintsFound { get; }
        [Rewrite]
        public bool ReadLastDiary { get; set; }

        [Rewrite]
        public List<Vector4> EnemiesKilledList;

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
