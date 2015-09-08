using System;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using InputSystem;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.Game")]
    public class Game : Microsoft.Xna.Framework.Game
    {
        [Rewrite]
        private bool m_contentLoaded;

        [Rewrite]
        public static RCScreenManager ScreenManager { get { return null; } }

        [Rewrite]
        public static PlayerStats PlayerStats;

        [Rewrite]
        public static EquipmentSystem EquipmentSystem;

        [Rewrite]
        public static InputMap GlobalInput;

        [Rewrite]
        public Microsoft.Xna.Framework.GraphicsDeviceManager GraphicsDeviceManager { get { return null; } }

        [Rewrite]
        public SaveGameManager SaveManager { get { return null; } }

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Swap)]
        protected virtual void Initialize()
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
                    Projectile = SpellEV.GetProjData(id, null),
                    DamageMultiplier = SpellEV.GetDamageMultiplier(id),
                    Rarity = SpellEV.GetRarity(id),
                    ManaCost = SpellEV.GetManaCost(id),
                    MiscValue1 = SpellEV.GetXValue(id),
                    MiscValue2 = SpellEV.GetYValue(id),
                    DisplayName = SpellType.ToString(id),
                    Icon = SpellType.Icon(id),
                    Description = SpellType.Description(id)
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
                    Description = ClassType.Description(id),
                    ProfileCardDescription = ClassType.ProfileCardDescription(id),
                    DisplayName = ClassType.ToString(id, false),
                    FemaleDisplayName = ClassType.ToString(id, true),
                    DamageTakenMultiplier = PlayerObj.get_ClassDamageTakenMultiplier(id),
                    HealthMultiplier = PlayerObj.get_ClassTotalHPMultiplier(id),
                    ManaMultiplier = PlayerObj.get_ClassTotalMPMultiplier(id),
                    MagicDamageMultiplier = PlayerObj.get_ClassMagicDamageGivenMultiplier(id),
                    MoveSpeedMultiplier = PlayerObj.get_ClassMoveSpeedMultiplier(id),
                    PhysicalDamageMultiplier = PlayerObj.get_ClassDamageGivenMultiplier(id)
                });

                foreach (var spellId in ClassType.GetSpellList(id))
                    cls.SpellList.Add(RogueAPI.Spells.SpellDefinition.GetById(spellId));
            }

            var equipArray = EquipmentSystem.EquipmentDataArray;
            int catIx = 0, catCount = equipArray.Count;
            while (catIx < catCount)
            {
                var catList = equipArray[catIx];
                var catString = EquipmentCategoryType.ToString(catIx);
                var catString2 = EquipmentCategoryType.ToString2(catIx);

                int itemIx = 0, itemCount = catList.Length;
                while (itemIx < itemCount)
                {
                    var item = catList[itemIx];
                    var name = EquipmentBaseType.ToString(itemIx);

                    item.CategoryId = catIx;
                    item.Index = itemIx;
                    item.DisplayName = name + " " + catString2;
                    item.ShortDisplayName = name + " " + catString;

                    itemIx++;
                }
                catIx++;
            }

            RogueAPI.Content.SpriteUtil.GraphicsDeviceManager = this.GraphicsDeviceManager;

            RogueAPI.Core.Initialize();

            Initialize();
        }


        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Swap)]
        protected virtual void LoadContent()
        {
            if (!this.m_contentLoaded)
            {
                this.LoadContent();
                RogueAPI.Core.OnContentLoaded();
            }
        }

    }

}
