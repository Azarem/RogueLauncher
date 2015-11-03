using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ImpactEffectPool")]
    public class ImpactEffectPool
    {
        [Rewrite]
        public void SpellCastEffect(Vector2 pos, float angle, bool megaSpell) { }
        [Rewrite]
        public void StartInverseEmit(Vector2 pos) { }
        [Rewrite]
        public void DisplayQuestionMark(Vector2 pos) { }
        [Rewrite]
        public void DisplayChestSparkleEffect(Vector2 position) { }
        [Rewrite]
        public void BlackSmokeEffect(GameObj obj) { }
        [Rewrite]
        public void DisplayDustEffect(GameObj obj) { }
        [Rewrite]
        public void DisplayDoubleJumpEffect(Vector2 position) { }
        [Rewrite]
        public void DisplayDashEffect(Vector2 position, bool flip) { }
        [Rewrite]
        public void DisplayFartEffect(GameObj obj) { }
        [Rewrite]
        public void Draw(Camera2D camera) { }
        [Rewrite]
        public void DisplayFusRoDahText(Vector2 position) { }
    }
}
