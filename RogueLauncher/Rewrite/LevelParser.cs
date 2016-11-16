using AssemblyTranslator;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.LevelParser")]
    internal class LevelParser
    {
        [Rewrite(action: RewriteAction.Swap)]
        public static void ParseRooms(string filePath, ContentManager contentManager = null, bool isDLCMap = false)
        {
            //var fullPath = contentManager.RootDirectory + "\\Levels\\" + filePath + ".xml";
            //FileMaps.RoomFileMap roomMap;

            //using (var stream = System.IO.File.OpenRead(fullPath))
            //{
            //    var ser = System.Xml.Serialization.XmlSerializer.FromTypes(new[] { typeof(FileMaps.RoomFileMap) })[0];
            //    roomMap = ser.Deserialize(stream) as FileMaps.RoomFileMap;
            //}

            //System.Diagnostics.Debugger.Break();

            ParseRooms(filePath, contentManager, isDLCMap);
        }
    }
}
