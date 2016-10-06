using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.RoomObj")]
    public class RoomObj : GameObj
    {
        [Rewrite]
        public int PoolIndex = -1;
        [Rewrite]
        private int m_roomNumber = -1;
        [Rewrite]
        protected float m_roomActivityDelay = 0.2f;
        [Rewrite]
        private float m_roomActivityCounter;
        [Rewrite]
        protected float m_doorSparkleDelay = 0.2f;
        [Rewrite]
        public string RedEnemyType;
        [Rewrite]
        public string BlueEnemyType;
        [Rewrite]
        public string GreenEnemyType;
        [Rewrite]
        private TextObj m_fairyChestText;
        [Rewrite]
        private TextObj m_indexText;
        [Rewrite]
        private TextObj m_roomInfoText;
        [Rewrite]
        private Vector2 m_debugRoomPosition;
        [Rewrite]
        private RenderTarget2D m_bwRender;
        [Rewrite]
        public PlayerObj Player;
        [Rewrite]
        private SpriteObj m_pauseBG;

        [Rewrite]
        public int ActiveEnemies { get; }
        [Rewrite]
        public bool AddToCastlePool { get; set; }
        [Rewrite]
        public bool AddToDungeonPool { get; set; }
        [Rewrite]
        public bool AddToGardenPool { get; set; }
        [Rewrite]
        public bool AddToTowerPool { get; set; }
        [Rewrite]
        public RenderTarget2D BGRender { get; }
        [Rewrite]
        public List<BorderObj> BorderList { get; internal set; }
        [Rewrite]
        public Vector2 DebugRoomPosition { get; set; }
        [Rewrite]
        public List<DoorObj> DoorList { get; internal set; }
        [Rewrite]
        public List<EnemyObj> EnemyList { get; internal set; }
        [Rewrite]
        public List<GameObj> GameObjList { get; internal set; }
        [Rewrite]
        public bool HasFairyChest { get; }
        [Rewrite]
        public bool IsDLCMap { get; set; }
        [Rewrite]
        public bool IsReversed { get; internal set; }
        [Rewrite]
        public int Level { get; set; }
        [Rewrite]
        public GameTypes.LevelType LevelType { get; set; }
        [Rewrite]
        public RoomObj LinkedRoom { get; set; }
        [Rewrite]
        public int RoomNumber { get; set; }
        [Rewrite]
        public List<EnemyObj> TempEnemyList { get; internal set; }
        [Rewrite]
        public List<TerrainObj> TerrainObjList { get; internal set; }

        [Rewrite]
        public RoomObj() { }

        [Rewrite]
        public void CopyRoomObjects(RoomObj room) { }

        [Rewrite]
        public void CopyRoomProperties(RoomObj room) { }

        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }

        [Rewrite]
        public void DarkenRoom() { }

        [Rewrite]
        public void DisplayFairyChestInfo() { }

        [Rewrite]
        public override void Dispose() { }

        [Rewrite]
        public override void Draw(Camera2D camera) { }

        [Rewrite]
        public void DrawBGObjs(Camera2D camera) { }

        [Rewrite]
        public void DrawRenderTargets(Camera2D camera) { }

        [Rewrite]
        protected override void FillCloneInstance(object obj) { }

        [Rewrite]
        public virtual void Initialize() { }

        [Rewrite]
        public virtual void InitializeRenderTarget(RenderTarget2D bgRenderTarget) { }

        [Rewrite]
        public virtual void LoadContent(GraphicsDevice graphics) { }

        [Rewrite]
        public virtual void OnEnter() { }

        [Rewrite]
        public virtual void OnExit() { }

        [Rewrite]
        public virtual void PauseRoom() { }

        //[Rewrite]
        //public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci) { }

        [Rewrite]
        public virtual void Reset() { }

        [Rewrite]
        public void Reverse() { }

        [Rewrite]
        private void ReverseObjNames(GameObj obj) { }

        [Rewrite]
        public void SetHeight(int height) { }

        [Rewrite]
        public void SetWidth(int width) { }

        [Rewrite]
        public virtual void UnpauseRoom() { }

        [Rewrite]
        public virtual void Update(GameTime gameTime) { }
    }
}
