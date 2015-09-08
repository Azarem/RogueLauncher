using System;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using RogueAPI.Classes;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ClassType")]
    public class ClassType
    {
        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string Description(byte id) { return ClassDefinition.GetById(id).Description; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ProfileCardDescription(byte id) { return ClassDefinition.GetById(id).ProfileCardDescription; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static string ToString(byte id, bool isFemale) { return ClassDefinition.GetById(id).GetDisplayName(isFemale); }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static byte[] GetSpellList(byte id) { return ClassDefinition.GetById(id).SpellByteArray; }
    }
}
