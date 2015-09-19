using System;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.Game")]
    public class Game : Microsoft.Xna.Framework.Game
    {
        [Rewrite]
        private bool m_contentLoaded;

        [Rewrite]
        public static Texture2D GenericTexture;

        [Rewrite]
        public static Effect ParallaxEffect;

        [Rewrite]
        public static Effect ColourSwapShader;

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

        [Rewrite]
        public static SpriteFont JunicodeFont;

        [Rewrite]
        public PhysicsManager PhysicsManager { get { return null; } }

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Swap)]
        protected virtual void Initialize()
        {
            string name;
            byte id;

            foreach (var f in Type.GetType("RogueCastle.SpellType").GetFields())
            {
                name = f.Name;
                if (name.StartsWith("Total"))
                    continue;

                id = (byte)f.GetRawConstantValue();
                var spell = RogueAPI.Spells.SpellDefinition.Register(new RogueAPI.Spells.SpellDefinition(id)
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

                switch (id)
                {
                    case 1:
                        spell.SoundList = new[] { "Cast_Dagger" };
                        break;

                    case 2:
                        spell.SoundList = new[] { "Cast_Axe" };
                        break;

                    case 5:
                        spell.SoundList = new[] { "Cast_Crowstorm" };
                        break;

                    case 9:
                        spell.SoundList = new[] { "Cast_Chakram" };
                        break;

                    case 10:
                        spell.SoundList = new[] { "Spell_GiantSword" };
                        break;

                    case 13:
                    case 15:
                        spell.SoundList = new[] { "Enemy_WallTurret_Fire_01", "Enemy_WallTurret_Fire_02", "Enemy_WallTurret_Fire_03", "Enemy_WallTurret_Fire_04" };
                        break;
                }
            }

            //foreach (var f in Type.GetType("RogueCastle.ClassType").GetFields())
            //{
            //    name = f.Name;
            //    if (name.StartsWith("Total"))
            //        continue;

            //    id = (byte)f.GetRawConstantValue();
            //    var cls = RogueAPI.Classes.ClassDefinition.Register(new RogueAPI.Classes.ClassDefinition(id)
            //    {
            //        Name = name,
            //        Description = ClassType.Description(id),
            //        ProfileCardDescription = ClassType.ProfileCardDescription(id),
            //        DisplayName = ClassType.ToString(id, false),
            //        FemaleDisplayName = ClassType.ToString(id, true),
            //        DamageTakenMultiplier = PlayerObj.get_ClassDamageTakenMultiplier(id),
            //        HealthMultiplier = PlayerObj.get_ClassTotalHPMultiplier(id),
            //        ManaMultiplier = PlayerObj.get_ClassTotalMPMultiplier(id),
            //        MagicDamageMultiplier = PlayerObj.get_ClassMagicDamageGivenMultiplier(id),
            //        MoveSpeedMultiplier = PlayerObj.get_ClassMoveSpeedMultiplier(id),
            //        PhysicalDamageMultiplier = PlayerObj.get_ClassDamageGivenMultiplier(id)
            //    });

            //    foreach (var spellId in ClassType.GetSpellList(id))
            //        cls.AssignedSpells.Add(RogueAPI.Spells.SpellDefinition.GetById(spellId));
            //}

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
                    name = EquipmentBaseType.ToString(itemIx);

                    item.CategoryId = catIx;
                    item.Index = itemIx;
                    item.DisplayName = name + " " + catString2;
                    item.ShortDisplayName = name + " " + catString;

                    itemIx++;
                }
                catIx++;
            }

            //foreach (var f in Type.GetType("RogueCastle.TraitType").GetFields())
            //{
            //    name = f.Name;
            //    if (name.StartsWith("Total"))
            //        continue;

            //    id = (byte)f.GetRawConstantValue();
            //    var trait = RogueAPI.Traits.TraitDefinition.Register(new RogueAPI.Traits.TraitDefinition(id)
            //    {
            //        Name = name,
            //        DisplayName = TraitType.ToString(id),
            //        Description = TraitType.Description(id, false),
            //        ProfileCardDescription = TraitType.ProfileCardDescription(id),
            //        Rarity = TraitType.Rarity(id)
            //    });

            //    switch (id)
            //    {
            //        case 2:
            //            trait.ProfileCardDescription = trait.Description;
            //            trait.FemaleProfileCardDescription = trait.FemaleDescription;
            //            break;

            //        case 4:
            //            trait.TraitConflicts.Add(RogueAPI.Traits.TraitDefinition.GetById(3));
            //            break;

            //        case 7:
            //            trait.TraitConflicts.Add(RogueAPI.Traits.TraitDefinition.GetById(6));
            //            break;

            //        case 10:
            //            trait.TraitConflicts.Add(RogueAPI.Traits.TraitDefinition.GetById(9));
            //            break;

            //        case 12:
            //            trait.ClassConflicts.Add(RogueAPI.Classes.ClassDefinition.GetById(7));
            //            trait.ClassConflicts.Add(RogueAPI.Classes.ClassDefinition.GetById(15));
            //            break;

            //        case 17:
            //            trait.TraitConflicts.Add(RogueAPI.Traits.TraitDefinition.GetById(16));
            //            break;

            //        case 29:
            //            trait.TraitConflicts.Add(RogueAPI.Traits.TraitDefinition.GetById(1));
            //            break;

            //        case 31:
            //            trait.ClassConflicts.Add(RogueAPI.Classes.ClassDefinition.GetById(1));
            //            trait.ClassConflicts.Add(RogueAPI.Classes.ClassDefinition.GetById(9));
            //            trait.ClassConflicts.Add(RogueAPI.Classes.ClassDefinition.GetById(16));
            //            trait.SpellConflicts.Add(RogueAPI.Spells.SpellDefinition.GetById(4));
            //            trait.SpellConflicts.Add(RogueAPI.Spells.SpellDefinition.GetById(6));
            //            trait.SpellConflicts.Add(RogueAPI.Spells.SpellDefinition.GetById(11));
            //            break;
            //    }
            //}

            RogueAPI.Content.SpriteUtil.GraphicsDeviceManager = this.GraphicsDeviceManager;

            RogueAPI.Core.CreateEnemy = CreateEnemyById;
            RogueAPI.Core.AttachEnemyToCurrentRoom = AttachEnemyToCurrentRoom;
            RogueAPI.Core.AttachEffect = AttachEffect;
            RogueAPI.Core.Initialize();

            Initialize();
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        protected virtual void LoadContent()
        {
            if (!this.m_contentLoaded)
            {
                this.LoadContent();
                RogueAPI.Core.OnContentLoaded();
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static PhysicsObjContainer CreateEnemyById(byte id, RogueAPI.Enemies.EnemyDifficulty difficulty)
        {
            switch (id)
            {
                case RogueAPI.Enemies.Chicken.Id:
                    return new EnemyObj_Chicken(null, null, null, (GameTypes.EnemyDifficulty)difficulty);
                default:
                    throw new InvalidOperationException();
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static void AttachEffect(RogueAPI.EffectType effect, GameObj target, Vector2? position)
        {
            var screen = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
            if (screen == null)
                return;

            switch (effect)
            {
                case RogueAPI.EffectType.BlackSmoke:
                    screen.ImpactEffectPool.BlackSmokeEffect(target);
                    break;

                case RogueAPI.EffectType.ChestSparkle:
                    screen.ImpactEffectPool.DisplayChestSparkleEffect(position ?? target.Position);
                    break;

                case RogueAPI.EffectType.QuestionMark:
                    screen.ImpactEffectPool.DisplayQuestionMark(position ?? target.Position);
                    break;
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static void AttachEnemyToCurrentRoom(PhysicsObjContainer enemy)
        {
            Game.ScreenManager.Player.AttachedLevel.AddEnemyToCurrentRoom((EnemyObj)enemy);
        }

    }

}
