using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.HazardObj")]
    public class HazardObj : PhysicsObj, IDealsDamageObj
    {
        [Rewrite]
        private Texture2D m_texture;
        [Rewrite]
        public int Damage { get; }
        [Rewrite]
        public override int Height { get; }
        [Rewrite]
        public override Rectangle TerrainBounds { get; }
        [Rewrite]
        public override int Width { get; }

        [Rewrite]
        public HazardObj(int width, int height) : base("Spikes_Sprite", null) { }

        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }

        [Rewrite]
        public override void Dispose() { }

        [Rewrite]
        public override void Draw(Camera2D camera) { }

        [Rewrite]
        protected override void FillCloneInstance(object obj) { }

        [Rewrite]
        public void InitializeTextures(Camera2D camera) { }

        //[Rewrite]
        //public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci) { }

        [Rewrite]
        public void SetHeight(int height) { }

        [Rewrite]
        public void SetWidth(int width) { }
    }
}
