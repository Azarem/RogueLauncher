using AssemblyTranslator;
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
        public DoorObj(RoomObj roomRef, int width, int height, GameTypes.DoorType doorType) : base(width, height) { }
    }
}
