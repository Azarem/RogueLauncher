using System;
using System.Linq;
using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.PlayerHUDObj")]
    public class PlayerHUDObj : SpriteObj
    {
        [Rewrite]
        public PlayerHUDObj() : base("PlayerHUDLvlText_Sprite") { }
        [Rewrite]
        public override void Dispose() { }
        [Rewrite]
        public override void Draw(Camera2D camera) { }
        [Rewrite]
        public void SetPosition(Vector2 position) { }
    }
}
