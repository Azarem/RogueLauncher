using AssemblyTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RogueLauncher.FileMaps
{
    [Rewrite("RogueCastle.FileMaps.RoomFileMap", action: RewriteAction.Add)]
    [XmlRoot("Map")]
    public class RoomFileMap
    {
        [XmlElement("Spritesheet")]
        public List<SpritesheetMap> Spritesheets { get; set; }
        [XmlElement("RoomObject")]
        public List<RoomMap> Rooms { get; set; }
    }

    [Rewrite("RogueCastle.FileMaps.SpritesheetMap", action: RewriteAction.Add)]
    public class SpritesheetMap
    {
        [XmlAttribute]
        public string Name { get; set; }
    }

    [Rewrite("RogueCastle.FileMaps.ObjectMapBase", action: RewriteAction.Add)]
    public class ObjectMapBase
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public float X { get; set; }
        [XmlAttribute]
        public float Y { get; set; }
        [XmlAttribute]
        public float Width { get; set; }
        [XmlAttribute]
        public float Height { get; set; }
        [XmlAttribute]
        public float ScaleX { get; set; }
        [XmlAttribute]
        public float ScaleY { get; set; }
        [XmlAttribute]
        public string Tag { get; set; }
    }

    [Rewrite("RogueCastle.FileMaps.RoomMap", action: RewriteAction.Add)]
    public class RoomMap : ObjectMapBase
    {
        [XmlAttribute]
        public int SelectionMode { get; set; }

        [XmlAttribute("DisplayBG")]
        public string _displayBG { get { return DisplayBG ? "True" : "False"; } set { DisplayBG = value == "True"; } }
        public bool DisplayBG { get; set; }

        [XmlAttribute("CastlePool")]
        public string _castlePool { get { return CastlePool ? "True" : "False"; } set { CastlePool = value == "True"; } }
        public bool CastlePool { get; set; }

        [XmlAttribute("GardenPool")]
        public string _gardenPool { get { return GardenPool ? "True" : "False"; } set { GardenPool = value == "True"; } }
        public bool GardenPool { get; set; }

        [XmlAttribute("TowerPool")]
        public string _towerPool { get { return TowerPool ? "True" : "False"; } set { TowerPool = value == "True"; } }
        public bool TowerPool { get; set; }

        [XmlAttribute("DungeonPool")]
        public string _dungeonPool { get { return DungeonPool ? "True" : "False"; } set { DungeonPool = value == "True"; } }
        public bool DungeonPool { get; set; }

        [XmlElement("GameObject")]
        public List<ObjectMap> GameObjects { get; set; }
    }

    [Rewrite("RogueCastle.FileMaps.ObjectMap", action: RewriteAction.Add)]

    public class ObjectMap : ObjectMapBase
    {
        [XmlAttribute]
        public string Type { get; set; }
        [XmlAttribute]
        public float Rotation { get; set; }

        [XmlAttribute("Flip")]
        public string _flip { get { return Flip ? "True" : "False"; } set { Flip = value == "True"; } }
        public bool Flip { get; set; }

        [XmlAttribute("CollidesTop")]
        public string _collidesTop { get { return CollidesTop ? "True" : "False"; } set { CollidesTop = value == "True"; } }
        public bool CollidesTop { get; set; }

        [XmlAttribute("CollidesBottom")]
        public string _collidesBottom { get { return CollidesBottom ? "True" : "False"; } set { CollidesBottom = value == "True"; } }
        public bool CollidesBottom { get; set; }

        [XmlAttribute("CollidesLeft")]
        public string _collidesLeft { get { return CollidesLeft ? "True" : "False"; } set { CollidesLeft = value == "True"; } }
        public bool CollidesLeft { get; set; }

        [XmlAttribute("CollidesRight")]
        public string _collidesRight { get { return CollidesRight ? "True" : "False"; } set { CollidesRight = value == "True"; } }
        public bool CollidesRight { get; set; }

        [XmlAttribute]
        public int LevelType { get; set; }
    }
}
