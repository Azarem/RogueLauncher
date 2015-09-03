using AssemblyTranslator;
using RogueAPI.Plugins;
using System;
using System.Linq;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    internal class Game
    {
        [Obfuscation(Exclude = true)]
        [Rewrite("RogueCastle.Game", "Initialize", RewriteAction.Swap, oldName: "_Initialize")]
        protected virtual void NewInitialize()
        {
            foreach (var f in Type.GetType("RogueCastle.SpellType").GetFields())
            {
                var name = f.Name;
                if (name.StartsWith("Total"))
                    continue;

                byte id = (byte)f.GetRawConstantValue();
                RogueAPI.Spells.SpellDefinition.Register(new RogueAPI.Spells.SpellDefinition(id)
                {
                    Name = name,
                    Projectile = SpellSystem.GetProjData(id, null),
                    DamageMultiplier = SpellSystem.GetDamageMultiplier(id),
                    Rarity = SpellSystem.GetRarity(id),
                    ManaCost = SpellSystem.GetManaCost(id),
                    MiscValue1 = SpellSystem.GetXValue(id),
                    MiscValue2 = SpellSystem.GetYValue(id),
                    DisplayName = SpellSystem.ToString(id),
                    Icon = SpellSystem.Icon(id),
                    Description = SpellSystem.Description(id)
                });
            }

            foreach (var f in Type.GetType("RogueCastle.ClassType").GetFields())
            {
                var name = f.Name;
                if (name.StartsWith("Total"))
                    continue;

                byte id = (byte)f.GetRawConstantValue();
                var cls = RogueAPI.Classes.ClassDefinition.Register(new RogueAPI.Classes.ClassDefinition(id)
                {
                    Name = name,
                    Description = ClassSystem.Description(id),
                    ProfileCardDescription = ClassSystem.ProfileCardDescription(id),
                    DisplayName = ClassSystem.ToString(id, false),
                    FemaleDisplayName = ClassSystem.ToString(id, true),
                    DamageTakenMultiplier = ClassSystem.ClassDamageTakenMultiplier(id),
                    HealthMultiplier = ClassSystem.ClassTotalHPMultiplier(id),
                    ManaMultiplier = ClassSystem.ClassTotalMPMultiplier(id),
                    MagicDamageMultiplier = ClassSystem.ClassMagicDamageGivenMultiplier(id),
                    MoveSpeedMultiplier = ClassSystem.ClassMoveSpeedMultiplier(id),
                    PhysicalDamageMultiplier = ClassSystem.ClassDamageGivenMultiplier(id)
                });

                foreach (var spellId in ClassSystem.GetSpellList(id))
                    cls.SpellList.Add(RogueAPI.Spells.SpellDefinition.GetById(spellId));
            }

            PluginInitializer.Initialize();

            NewInitialize();
        }
    }

}
