using AssemblyTranslator;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.PlayerPart")]
    public class PlayerPart
    {
        [Rewrite]
        public static Vector3 GetPartIndices(int category) { return new Vector3(); }
    }
}
