using System;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using RogueAPI.Spells;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SpellType")]
    public class SpellType
    {
        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ToString(byte id) { return SpellDefinition.GetById(id).DisplayName; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Description(byte id) { return SpellDefinition.GetById(id).Description; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Icon(byte id) { return SpellDefinition.GetById(id).Icon; }
    }
}
