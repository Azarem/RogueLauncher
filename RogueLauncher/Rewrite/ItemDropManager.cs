using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ItemDropManager")]
    public class ItemDropManager
    {
        [Rewrite]
        public void DropItem(Vector2 position, int dropType, float amount) { }
        [Rewrite]
        public void Draw(Camera2D camera) { }
    }
}
