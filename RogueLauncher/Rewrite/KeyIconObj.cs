using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework.Input;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.KeyIconObj")]
    public class KeyIconObj : ObjContainer
    {
        [Rewrite]
        public void SetButton(Buttons button) { }
        [Rewrite]
        public void SetKey(Keys? key, bool upperCase = true) { }
    }
}
