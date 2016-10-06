using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.BorderObj")]
    public class BorderObj : GameObj
    {
        [Rewrite]
        public bool BorderTop;
        [Rewrite]
        public bool BorderBottom;
        [Rewrite]
        public bool BorderLeft;
        [Rewrite]
        public bool BorderRight;
        [Rewrite]
        public GameTypes.LevelType LevelType = GameTypes.LevelType.CASTLE;

        [Rewrite]
        public Texture2D BorderTexture { get; internal set; }
        [Rewrite]
        public SpriteObj CornerLTexture { get; internal set; }
        [Rewrite]
        public SpriteObj CornerTexture { get; internal set; }
        [Rewrite]
        public Texture2D NeoTexture { get; set; }
        [Rewrite]
        public Vector2 TextureOffset { get; set; }
        [Rewrite]
        public Vector2 TextureScale { get; set; }

        [Rewrite]
        public BorderObj() { }

        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }

        [Rewrite]
        public override void Dispose() { }

        [Rewrite]
        public override void Draw(Camera2D camera) { }

        [Rewrite]
        public void DrawCorners(Camera2D camera) { }

        [Rewrite]
        protected override void FillCloneInstance(object obj) { }

        //[Rewrite]
        //public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci) { }

        [Rewrite]
        public void SetBorderTextures(Texture2D borderTexture, string cornerTextureString, string cornerLTextureString) { }

        [Rewrite]
        public void SetHeight(int height) { }

        [Rewrite]
        public void SetWidth(int width) { }
    }
}
