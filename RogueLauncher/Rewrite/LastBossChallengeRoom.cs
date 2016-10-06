using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.LastBossChallengeRoom")]
    public class LastBossChallengeRoom : ChallengeBossRoomObj
    {
        //[Rewrite]
        //private EnemyObj_LastBoss m_boss;
        //[Rewrite]
        //private EnemyObj_LastBoss m_boss2;

        [Rewrite]
        public override bool BossKilled { get; }


        [Rewrite]
        public LastBossChallengeRoom() { }

        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }

        [Rewrite]
        public override void Dispose() { }

        [Rewrite]
        public void EndCutscene() { }

        [Rewrite]
        protected override void FillCloneInstance(object obj) { }

        [Rewrite]
        public override void Initialize() { }

        [Rewrite]
        public void Intro2() { }

        [Rewrite]
        public override void OnEnter() { }

        [Rewrite]
        public override void OnExit() { }

        [Rewrite]
        protected override void SaveCompletionData() { }

        [Rewrite]
        private void SetRoomData() { }
    }
}
