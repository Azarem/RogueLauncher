using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.RoomObj")]
    public class RoomObj : GameObj
    {
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
    }
}
