using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.EnemyHUDObj")]
    public class EnemyHUDObj : SpriteObj
    {
        [Rewrite]
        public EnemyHUDObj() : base("EnemyHUD_Sprite") { }
    }
}
