using AssemblyTranslator;
using DS2DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.DoorObj")]
    public class DoorObj : TerrainObj
    {
        [Rewrite]
        private GameTypes.DoorType m_doorType = GameTypes.DoorType.OPEN;
        [Rewrite]
        public string DoorPosition = "NONE";
        [Rewrite]
        public bool Attached;
        [Rewrite]
        public bool IsBossDoor;
        [Rewrite]
        public bool Locked;
        [Rewrite]
        private SpriteObj m_arrowIcon;

        [Rewrite]
        public GameTypes.DoorType DoorType { get { return m_doorType; } }

        [Rewrite]
        public RoomObj Room { get; set; }

        [Rewrite]
        public DoorObj(RoomObj roomRef, int width, int height, GameTypes.DoorType doorType) : base(width, height) { }
    }
}
