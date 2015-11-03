using AssemblyTranslator;
using DS2DEngine;
using System;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProjectileIconPool")]
    public class ProjectileIconPool : IDisposable
    {
        [Rewrite]
        public void Dispose() { }
        [Rewrite]
        public void Draw(Camera2D camera) { }
    }
}
