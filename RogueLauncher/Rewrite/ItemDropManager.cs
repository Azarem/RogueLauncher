using System;
using System.Linq;
using AssemblyTranslator;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ItemDropManager")]
    public class ItemDropManager
    {
        [Rewrite]
        public void DropItem(Vector2 position, int dropType, float amount) { }
    }
}
