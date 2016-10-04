using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.MapObj")]
    public class MapObj : GameObj
    {
        [Rewrite]
        public bool DrawNothing { get; set; }
        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }
        [Rewrite]
        public void AddAllIcons(List<RoomObj> roomList) { }

        [Rewrite]
        public void CentreAroundTeleporter(int index, bool tween = false) { }
        [Rewrite]
        public void CentreAroundPlayer() { }

        [Rewrite]
        public void DrawRenderTargets(Camera2D camera) { }
        [Rewrite]
        public void TeleportPlayer(int index) { }
        [Rewrite]
        public Vector2 CameraOffset;
    }
}
