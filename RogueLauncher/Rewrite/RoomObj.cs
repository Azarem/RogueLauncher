using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyTranslator;
using DS2DEngine;

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

        protected override GameObj CreateCloneInstance() { return null; }
    }
}
