using System;
using System.Linq;
using AssemblyTranslator;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.TextManager")]
    public class TextManager
    {
        [Rewrite]
        public void DisplayNumberStringText(int amount, string text, Color color, Vector2 position) { }
    }
}
