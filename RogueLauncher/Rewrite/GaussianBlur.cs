using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.GaussianBlur")]
    public class GaussianBlur
    {
        [Rewrite]
        public bool InvertMask { get; set; }
        [Rewrite]
        public void Draw(RenderTarget2D srcTexture, Camera2D Camera, RenderTarget2D mask = null) { }
    }
}
