using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.TextManager")]
    public class TextManager
    {
        [Rewrite]
        public void DisplayNumberStringText(int amount, string text, Color color, Vector2 position) { }
        [Rewrite]
        public void Draw(Camera2D camera) { }
        [Rewrite]
        public void DisplayNumberText(int amount, Color color, Vector2 position) { }
    }
}
