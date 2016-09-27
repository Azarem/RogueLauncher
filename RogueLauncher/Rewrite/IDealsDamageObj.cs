using AssemblyTranslator;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.IDealsDamageObj")]
    public interface IDealsDamageObj
    {
        [Rewrite]
        int Damage { get; }
    }
}
