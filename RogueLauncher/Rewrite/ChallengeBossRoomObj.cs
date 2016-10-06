using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ChallengeBossRoomObj")]
    public abstract class ChallengeBossRoomObj : RoomObj
    {
        [Rewrite]
        protected bool m_cutsceneRunning;
        [Rewrite]
        protected ChestObj m_bossChest;
        [Rewrite]
        private float m_sparkleTimer;
        [Rewrite]
        private bool m_teleportingOut;
        [Rewrite]
        private float m_roomFloor;
        [Rewrite]
        private TextObj m_bossTitle1;
        [Rewrite]
        private TextObj m_bossTitle2;
        [Rewrite]
        private SpriteObj m_bossDivider;
        [Rewrite]
        private PlayerStats m_storedPlayerStats;
        [Rewrite]
        private int m_storedHealth;
        [Rewrite]
        private float m_storedMana;
        [Rewrite]
        private Vector2 m_storedScale;
        [Rewrite]
        private List<RaindropObj> m_rainFG;

        [Rewrite]
        public abstract bool BossKilled { get; }
        [Rewrite]
        public int StoredHP { get; }
        [Rewrite]
        public float StoredMP { get; }

        [Rewrite]
        public ChallengeBossRoomObj() { }

        [Rewrite]
        public virtual void BossCleanup() { }

        [Rewrite]
        public void DisplayBossTitle(string bossTitle1, string bossTitle2, string endHandler) { }

        [Rewrite]
        public override void Dispose() { }

        [Rewrite]
        public override void Draw(Camera2D camera) { }

        [Rewrite]
        public override void Initialize() { }

        [Rewrite]
        public void KickPlayerOut() { }

        [Rewrite]
        public void KickPlayerOut2() { }

        [Rewrite]
        public void LoadPlayerData() { }

        [Rewrite]
        public void LockCamera() { }

        [Rewrite]
        public override void OnEnter() { }

        [Rewrite]
        public override void OnExit() { }

        [Rewrite]
        protected abstract void SaveCompletionData();

        [Rewrite]
        public void StorePlayerData() { }

        [Rewrite]
        public void TeleportPlayer() { }

        [Rewrite]
        public void TeleportScaleIn() { }

        [Rewrite]
        public void TeleportScaleOut() { }

        [Rewrite]
        public void UnlockChest() { }

        [Rewrite]
        public override void Update(GameTime gameTime) { }
    }
}
