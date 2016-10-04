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
        public List<EnemyObj> EnemyList { get; internal set; }
        [Rewrite]
        public int ActiveEnemies { get { return 0; } }
        [Rewrite]
        public List<EnemyObj> TempEnemyList { get; internal set; }
        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }
        [Rewrite]
        public RenderTarget2D BGRender { get { return null; } }
        [Rewrite]
        public GameTypes.LevelType LevelType { get; set; }
        [Rewrite]
        public void DrawBGObjs(Camera2D camera) { }
        [Rewrite]
        public virtual void OnExit() { }
        [Rewrite]
        public List<TerrainObj> TerrainObjList { get; internal set; }

        [Rewrite]
        public virtual void Update(GameTime gameTime) { }
    }
}
