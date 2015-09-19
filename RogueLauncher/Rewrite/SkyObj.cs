using System;
using System.Linq;
using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SkyObj")]
    public class SkyObj : GameObj
    {
        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }
    }
}
