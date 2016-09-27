using AssemblyTranslator;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

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
        public static SpriteFont PixelArtFont;
        [Rewrite]
        public static SpriteFont PixelArtFontBold;
        [Rewrite]
        public static SpriteFont JunicodeFont;
        [Rewrite]
        public static SpriteFont EnemyLevelFont;
        [Rewrite]
        public static SpriteFont PlayerLevelFont;
        [Rewrite]
        public static SpriteFont GoldFont;
        [Rewrite]
        public static SpriteFont HerzogFont;
        [Rewrite]
        public static SpriteFont JunicodeLargeFont;
        [Rewrite]
        public static SpriteFont CinzelFont;
        [Rewrite]
        public static SpriteFont BitFont;
        [Rewrite]
        public static Effect BWMaskEffect;
        [Rewrite]
        public static Effect HSVEffect;
        [Rewrite]
        public static Effect InvertShader;
        [Rewrite]
        public static Effect ShadowEffect;
        [Rewrite]
        public static GaussianBlur GaussianBlur;
        [Rewrite]
        public static Effect RippleEffect;
        [Rewrite]
        public static Effect MaskEffect;
        [Rewrite]
        public static float PlaySessionLength { get; set; }

        [Rewrite]
        public PhysicsManager PhysicsManager { get { return null; } }

        [Rewrite]
        public static Game.SettingStruct GameConfig;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        protected virtual void Initialize()
        {
            string name;
            byte id;

            //foreach (var f in Type.GetType("RogueCastle.SpellType").GetFields())
            //{
            //    name = f.Name;
            //    if (name.StartsWith("Total"))
            //        continue;

            //    id = (byte)f.GetRawConstantValue();
            //    var spell = RogueAPI.Spells.SpellDefinition.Register(new RogueAPI.Spells.SpellDefinition(id)
            //    {
            //        Name = name,
            //        Projectile = SpellEV.GetProjData(id, null),
            //        DamageMultiplier = SpellEV.GetDamageMultiplier(id),
            //        Rarity = SpellEV.GetRarity(id),
            //        ManaCost = SpellEV.GetManaCost(id),
            //        MiscValue1 = SpellEV.GetXValue(id),
            //        MiscValue2 = SpellEV.GetYValue(id),
            //        DisplayName = SpellType.ToString(id),
            //        Icon = SpellType.Icon(id),
            //        Description = SpellType.Description(id)
            //    });

            //    switch (id)
            //    {
            //        case 1:
            //            spell.SoundList = new[] { "Cast_Dagger" };
            //            break;

            //        case 2:
            //            spell.SoundList = new[] { "Cast_Axe" };
            //            break;

            //        case 5:
            //            spell.SoundList = new[] { "Cast_Crowstorm" };
            //            break;

            //        case 9:
            //            spell.SoundList = new[] { "Cast_Chakram" };
            //            break;

            //        case 10:
            //            spell.SoundList = new[] { "Spell_GiantSword" };
            //            break;

            //        case 13:
            //        case 15:
            //            spell.SoundList = new[] { "Enemy_WallTurret_Fire_01", "Enemy_WallTurret_Fire_02", "Enemy_WallTurret_Fire_03", "Enemy_WallTurret_Fire_04" };
            //            break;
            //    }
            //}

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

            RogueAPI.Content.SpriteUtil.GraphicsDeviceManager = GraphicsDeviceManager;


            RogueAPI.Core.CreateEnemy = CreateEnemyById;
            RogueAPI.Core.AttachEnemyToCurrentRoom = AttachEnemyToCurrentRoom;
            //RogueAPI.Core.AttachEffect = AttachEffect;
            RogueAPI.Core.GetCurrentRoomTerrainObjects = GetCurrentTerrainObjects;
            RogueAPI.Effects.EffectDefinition.AllocateSprite = AllocateSprite;
            RogueAPI.Projectiles.ProjectileDefinition.AllocateProjectile = AllocateProjectile;
            RogueAPI.Core.AttachPhysicsObject = AttachPhysicsObject;
            RogueAPI.Core.ActiveEnemyCount = ActiveEnemyCount;
            RogueAPI.Core.GetEnemyList = GetEnemyList;
            RogueAPI.Core.GetTempEnemyList = GetTempEnemyList;
            RogueAPI.Core.GetActiveProjectiles = GetProjectiles;
            RogueAPI.Core.DisplayNumberString = DisplayNumberString;
            RogueAPI.Core.Initialize();

            Initialize();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        private void InitializeDefaultConfig()
        {
            InitializeDefaultConfig();
            RogueAPI.Game.InputManager.ThumbstickDeadzone = InputSystem.InputManager.Deadzone;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public static void InitializeGlobalInput()
        {
            RogueAPI.Game.InputManager.ClearAll();

            RogueAPI.Game.InputManager.MapKey(Keys.Enter, InputKeys.MenuConfirm1);
            RogueAPI.Game.InputManager.MapKey(Keys.D, InputKeys.MenuConfirm2);
            RogueAPI.Game.InputManager.MapKey(Keys.Escape, InputKeys.MenuCancel1);
            RogueAPI.Game.InputManager.MapKey(Keys.S, InputKeys.MenuCancel2);
            RogueAPI.Game.InputManager.MapKey(Keys.LeftControl, InputKeys.MenuCredits);
            RogueAPI.Game.InputManager.MapKey(Keys.Tab, InputKeys.MenuOptions);
            RogueAPI.Game.InputManager.MapKey(Keys.LeftShift, InputKeys.MenuProfileCard);
            RogueAPI.Game.InputManager.MapKey(Keys.Back, InputKeys.MenuRogueMode);
            RogueAPI.Game.InputManager.MapKey(Keys.Escape, InputKeys.MenuPause);
            RogueAPI.Game.InputManager.MapKey(Keys.Tab, InputKeys.MenuMap);
            RogueAPI.Game.InputManager.MapKey(Keys.Escape, InputKeys.MenuProfileSelect);
            RogueAPI.Game.InputManager.MapKey(Keys.Back, InputKeys.MenuDeleteProfile);
            RogueAPI.Game.InputManager.MapKey(Keys.S, InputKeys.PlayerJump1);
            RogueAPI.Game.InputManager.MapKey(Keys.Space, InputKeys.PlayerJump2);
            RogueAPI.Game.InputManager.MapKey(Keys.W, InputKeys.PlayerSpell1);
            RogueAPI.Game.InputManager.MapKey(Keys.D, InputKeys.PlayerAttack);
            RogueAPI.Game.InputManager.MapKey(Keys.A, InputKeys.PlayerBlock);
            RogueAPI.Game.InputManager.MapKey(Keys.Q, InputKeys.PlayerDashLeft);
            RogueAPI.Game.InputManager.MapKey(Keys.E, InputKeys.PlayerDashRight);
            RogueAPI.Game.InputManager.MapKey(Keys.I, InputKeys.PlayerUp1);
            RogueAPI.Game.InputManager.MapKey(Keys.Up, InputKeys.PlayerUp2);
            RogueAPI.Game.InputManager.MapKey(Keys.K, InputKeys.PlayerDown1);
            RogueAPI.Game.InputManager.MapKey(Keys.Down, InputKeys.PlayerDown2);
            RogueAPI.Game.InputManager.MapKey(Keys.J, InputKeys.PlayerLeft1);
            RogueAPI.Game.InputManager.MapKey(Keys.Left, InputKeys.PlayerLeft2);
            RogueAPI.Game.InputManager.MapKey(Keys.L, InputKeys.PlayerRight1);
            RogueAPI.Game.InputManager.MapKey(Keys.Right, InputKeys.PlayerRight2);

            RogueAPI.Game.InputManager.MapButton(Buttons.A, InputKeys.MenuConfirm1);
            RogueAPI.Game.InputManager.MapButton(Buttons.Start, InputKeys.MenuConfirm2);
            RogueAPI.Game.InputManager.MapButton(Buttons.B, InputKeys.MenuCancel1);
            RogueAPI.Game.InputManager.MapButton(Buttons.Back, InputKeys.MenuCancel2);
            RogueAPI.Game.InputManager.MapButton(Buttons.RightTrigger, InputKeys.MenuCredits);
            RogueAPI.Game.InputManager.MapButton(Buttons.Y, InputKeys.MenuOptions);
            RogueAPI.Game.InputManager.MapButton(Buttons.X, InputKeys.MenuProfileCard);
            RogueAPI.Game.InputManager.MapButton(Buttons.Back, InputKeys.MenuRogueMode);
            RogueAPI.Game.InputManager.MapButton(Buttons.Start, InputKeys.MenuPause);
            RogueAPI.Game.InputManager.MapButton(Buttons.Back, InputKeys.MenuMap);
            RogueAPI.Game.InputManager.MapButton(Buttons.Back, InputKeys.MenuProfileSelect);
            RogueAPI.Game.InputManager.MapButton(Buttons.Y, InputKeys.MenuDeleteProfile);
            RogueAPI.Game.InputManager.MapButton(Buttons.A, InputKeys.PlayerJump1);
            RogueAPI.Game.InputManager.MapButton(Buttons.X, InputKeys.PlayerAttack);
            RogueAPI.Game.InputManager.MapButton(Buttons.Y, InputKeys.PlayerBlock);
            RogueAPI.Game.InputManager.MapButton(Buttons.LeftTrigger, InputKeys.PlayerDashLeft);
            RogueAPI.Game.InputManager.MapButton(Buttons.RightTrigger, InputKeys.PlayerDashRight);
            RogueAPI.Game.InputManager.MapButton(Buttons.DPadUp, InputKeys.PlayerUp1);
            RogueAPI.Game.InputManager.MapButton(Buttons.DPadDown, InputKeys.PlayerDown1);
            RogueAPI.Game.InputManager.MapButton(Buttons.DPadLeft, InputKeys.PlayerLeft1);
            RogueAPI.Game.InputManager.MapButton(Buttons.LeftThumbstickLeft, InputKeys.PlayerLeft2);
            RogueAPI.Game.InputManager.MapButton(Buttons.DPadRight, InputKeys.PlayerRight1);
            RogueAPI.Game.InputManager.MapButton(Buttons.LeftThumbstickRight, InputKeys.PlayerRight2);
            RogueAPI.Game.InputManager.MapButton(Buttons.B, InputKeys.PlayerSpell1);
            RogueAPI.Game.InputManager.MapStick(InputKeys.PlayerUp2, RogueAPI.Game.ThumbStick.Left, -90f, 30f);
            RogueAPI.Game.InputManager.MapStick(InputKeys.PlayerDown2, RogueAPI.Game.ThumbStick.Left, 90f, 30f);

        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        protected virtual void LoadContent()
        {
            if (!m_contentLoaded)
            {
                LoadContent();
                RogueAPI.Core.OnContentLoaded();
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public void LoadAllEffects()
        {
            LoadAllEffects();
            RogueAPI.Game.Shaders.Ripple = RippleEffect;
            RogueAPI.Game.Shaders.BWMask = BWMaskEffect;
            RogueAPI.Game.Shaders.ColorSwap = ColourSwapShader;
            //RogueAPI.Game.Shaders.GaussianBlur = GaussianBlur;
            RogueAPI.Game.Shaders.HSV = HSVEffect;
            RogueAPI.Game.Shaders.Invert = InvertShader;
            RogueAPI.Game.Shaders.Mask = MaskEffect;
            RogueAPI.Game.Shaders.Parallax = ParallaxEffect;
            RogueAPI.Game.Shaders.Shadow = ShadowEffect;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        public void LoadAllSpriteFonts()
        {
            LoadAllSpriteFonts();

            RogueAPI.Game.Fonts.JunicodeFont = JunicodeFont;
            RogueAPI.Game.Fonts.JunicodeLargeFont = JunicodeLargeFont;
            RogueAPI.Game.Fonts.BitFont = BitFont;
            RogueAPI.Game.Fonts.CinzelFont = CinzelFont;
            RogueAPI.Game.Fonts.EnemyLevelFont = EnemyLevelFont;
            RogueAPI.Game.Fonts.GoldFont = GoldFont;
            RogueAPI.Game.Fonts.HerzogFont = HerzogFont;
            RogueAPI.Game.Fonts.PixelArtFont = PixelArtFont;
            RogueAPI.Game.Fonts.PixelArtFontBold = PixelArtFontBold;
            RogueAPI.Game.Fonts.PlayerLevelFont = PlayerLevelFont;
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void LoadConfig()
        {
            Console.WriteLine("Loading Config file");
            this.InitializeDefaultConfig();
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                using (StreamReader streamReader = new StreamReader(Path.Combine(folderPath, "Rogue Legacy", "GameConfig.ini")))
                {
                    CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";

                    string line = null;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        int index = line.IndexOf("=");
                        if (index < 0)
                            continue;

                        string key = line.Substring(0, index);
                        string value = line.Substring(index + 1);
                        Keys k;

                        switch (key)
                        {
                            case "ScreenWidth": GameConfig.ScreenWidth = int.Parse(value, NumberStyles.Any, cultureInfo); break;
                            case "ScreenHeight": GameConfig.ScreenHeight = int.Parse(value, NumberStyles.Any, cultureInfo); break;
                            case "Fullscreen": GameConfig.FullScreen = bool.Parse(value); break;
                            case "QuickDrop": GameConfig.QuickDrop = bool.Parse(value); break;
                            case "MusicVol": GameConfig.MusicVolume = float.Parse(value); break;
                            case "SFXVol": GameConfig.SFXVolume = float.Parse(value); break;
                            case "DeadZone": RogueAPI.Game.InputManager.ThumbstickDeadzone = (float)int.Parse(value, NumberStyles.Any, cultureInfo); break;
                            case "EnableDirectInput": GameConfig.EnableDirectInput = bool.Parse(value); break;
                            case "ReduceQuality": LevelEV.SAVE_FRAMES = GameConfig.ReduceQuality = bool.Parse(value); break;
                            case "EnableSteamCloud": GameConfig.EnableSteamCloud = bool.Parse(value); break;
                            case "Slot": GameConfig.ProfileSlot = byte.Parse(value, NumberStyles.Any, cultureInfo); break;
                            case "KeyUP": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerUp1); break;
                            case "KeyDOWN": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerDown1); break;
                            case "KeyLEFT": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerLeft1); break;
                            case "KeyRIGHT": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerRight1); break;
                            case "KeyATTACK":
                                k = (Keys)Enum.Parse(typeof(Keys), value);
                                RogueAPI.Game.InputManager.MapKey(k, InputKeys.PlayerAttack);
                                RogueAPI.Game.InputManager.MapKey(k, InputKeys.MenuConfirm2);
                                break;
                            case "KeyJUMP":
                                k = (Keys)Enum.Parse(typeof(Keys), value);
                                RogueAPI.Game.InputManager.MapKey(k, InputKeys.PlayerJump1);
                                RogueAPI.Game.InputManager.MapKey(k, InputKeys.MenuCancel2);
                                break;
                            case "KeySPECIAL": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerBlock); break;
                            case "KeyDASHLEFT": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerDashLeft); break;
                            case "KeyDASHRIGHT": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerDashRight); break;
                            case "KeySPELL1": RogueAPI.Game.InputManager.MapKey((Keys)Enum.Parse(typeof(Keys), value), InputKeys.PlayerSpell1); break;
                            case "ButtonUP": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerUp1); break;
                            case "ButtonDOWN": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerDown1); break;
                            case "ButtonLEFT": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerLeft1); break;
                            case "ButtonRIGHT": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerRight1); break;
                            case "ButtonATTACK": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerAttack); break;
                            case "ButtonJUMP": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerJump1); break;
                            case "ButtonSPECIAL": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerBlock); break;
                            case "ButtonDASHLEFT": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerDashLeft); break;
                            case "ButtonDASHRIGHT": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerDashRight); break;
                            case "ButtonSPELL1": RogueAPI.Game.InputManager.MapButton((Buttons)Enum.Parse(typeof(Buttons), value), InputKeys.PlayerSpell1); break;
                        }
                    }

                    //RogueAPI.Game.InputManager.MapKey(RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerAttack), InputKeys.MenuConfirm2);
                    //RogueAPI.Game.InputManager.MapKey(RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerJump1), InputKeys.MenuCancel2);

                    streamReader.Close();

                    if (GameConfig.ScreenHeight <= 0 || GameConfig.ScreenWidth <= 0)
                        throw new Exception("Blank Config File");
                }
            }
            catch
            {
                Console.WriteLine("Config File Not Found. Creating Default Config File.");
                InitializeDefaultConfig();
                SaveConfig();
            }
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void SaveConfig()
        {
            Console.WriteLine("Saving Config file");
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string path = Path.Combine(folderPath, "Rogue Legacy");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(folderPath, "Rogue Legacy", "GameConfig.ini"), false))
            {
                streamWriter.WriteLine("[Screen Resolution]");
                streamWriter.WriteLine("ScreenWidth=" + GameConfig.ScreenWidth);
                streamWriter.WriteLine("ScreenHeight=" + GameConfig.ScreenHeight);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Fullscreen]");
                streamWriter.WriteLine("Fullscreen=" + GameConfig.FullScreen);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[QuickDrop]");
                streamWriter.WriteLine("QuickDrop=" + GameConfig.QuickDrop);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Game Volume]");
                streamWriter.WriteLine("MusicVol=" + string.Format("{0:F2}", GameConfig.MusicVolume));
                streamWriter.WriteLine("SFXVol=" + string.Format("{0:F2}", GameConfig.SFXVolume));
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Joystick Dead Zone]");
                streamWriter.WriteLine("DeadZone=" + RogueAPI.Game.InputManager.ThumbstickDeadzone);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Enable DirectInput Gamepads]");
                streamWriter.WriteLine("EnableDirectInput=" + GameConfig.EnableDirectInput);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Reduce Shader Quality]");
                streamWriter.WriteLine("ReduceQuality=" + GameConfig.ReduceQuality);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Profile]");
                streamWriter.WriteLine("Slot=" + GameConfig.ProfileSlot);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Keyboard Config]");
                streamWriter.WriteLine("KeyUP=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerUp1));
                streamWriter.WriteLine("KeyDOWN=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerDown1));
                streamWriter.WriteLine("KeyLEFT=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerLeft1));
                streamWriter.WriteLine("KeyRIGHT=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerRight1));
                streamWriter.WriteLine("KeyATTACK=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerAttack));
                streamWriter.WriteLine("KeyJUMP=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerJump1));
                streamWriter.WriteLine("KeySPECIAL=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerBlock));
                streamWriter.WriteLine("KeyDASHLEFT=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerDashLeft));
                streamWriter.WriteLine("KeyDASHRIGHT=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerDashRight));
                streamWriter.WriteLine("KeySPELL1=" + RogueAPI.Game.InputManager.GetMappedKey(InputKeys.PlayerSpell1));
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Gamepad Config]");
                streamWriter.WriteLine("ButtonUP=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerUp1));
                streamWriter.WriteLine("ButtonDOWN=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerDown1));
                streamWriter.WriteLine("ButtonLEFT=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerLeft1));
                streamWriter.WriteLine("ButtonRIGHT=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerRight1));
                streamWriter.WriteLine("ButtonATTACK=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerAttack));
                streamWriter.WriteLine("ButtonJUMP=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerJump1));
                streamWriter.WriteLine("ButtonSPECIAL=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerBlock));
                streamWriter.WriteLine("ButtonDASHLEFT=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerDashLeft));
                streamWriter.WriteLine("ButtonDASHRIGHT=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerDashRight));
                streamWriter.WriteLine("ButtonSPELL1=" + RogueAPI.Game.InputManager.GetMappedButton(InputKeys.PlayerSpell1));
                streamWriter.Close();
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

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        //protected static RogueAPI.Projectiles.ProjectileObj FireProjectile(RogueAPI.Projectiles.ProjectileDefinition proj, GameObj source, GameObj target)
        //{
        //    var screen = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
        //    if (screen == null)
        //        return null;

        //    return screen.ProjectileManager.FireProjectile(proj, source, target);
        //}

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        //protected static void AttachEffect(RogueAPI.EffectType effect, GameObj target, Vector2? position)
        //{
        //    var screen = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
        //    if (screen == null)
        //        return;

        //    switch (effect)
        //    {
        //        case RogueAPI.EffectType.BlackSmoke:
        //            screen.ImpactEffectPool.BlackSmokeEffect(target);
        //            break;

        //        case RogueAPI.EffectType.ChestSparkle:
        //            screen.ImpactEffectPool.DisplayChestSparkleEffect(position ?? target.Position);
        //            break;

        //        case RogueAPI.EffectType.QuestionMark:
        //            screen.ImpactEffectPool.DisplayQuestionMark(position ?? target.Position);
        //            break;

        //        case RogueAPI.EffectType.FahRoDus:
        //            screen.ImpactEffectPool.DisplayFusRoDahText(position.Value);
        //            break;
        //    }
        //}

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static IEnumerable<BlankObj> GetCurrentTerrainObjects()
        {
            return Game.ScreenManager.Player.AttachedLevel.CurrentRoom.TerrainObjList;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static void AttachPhysicsObject(PhysicsObj obj)
        {
            Game.ScreenManager.Player.AttachedLevel.PhysicsManager.AddObject(obj);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static RogueAPI.Effects.EffectSpriteInstance AllocateSprite()
        {
            return Game.ScreenManager.Player.AttachedLevel.ImpactEffectPool.CheckOut();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static RogueAPI.Projectiles.ProjectileObj AllocateProjectile()
        {
            return Game.ScreenManager.Player.AttachedLevel.ProjectileManager.CheckOut();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static void AttachEnemyToCurrentRoom(PhysicsObjContainer enemy)
        {
            Game.ScreenManager.Player.AttachedLevel.AddEnemyToCurrentRoom((EnemyObj)enemy);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static int ActiveEnemyCount()
        {
            return Game.ScreenManager.Player.AttachedLevel.CurrentRoom.ActiveEnemies;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static bool KillableFilter(EnemyObj obj)
        {
            return !obj.NonKillable && !obj.IsKilled;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static IEnumerable<PhysicsObjContainer> GetEnemyList(bool killableOnly)
        {
            IEnumerable<EnemyObj> list = ScreenManager.Player.AttachedLevel.CurrentRoom.EnemyList;
            if (killableOnly)
                list = list.Where(KillableFilter);
            return list;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static IEnumerable<PhysicsObjContainer> GetTempEnemyList(bool killableOnly)
        {
            IEnumerable<EnemyObj> list = ScreenManager.Player.AttachedLevel.CurrentRoom.TempEnemyList;
            if (killableOnly)
                list = list.Where(KillableFilter);
            return list;
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static IEnumerable<RogueAPI.Projectiles.ProjectileObj> GetProjectiles()
        {
            return Game.ScreenManager.Player.AttachedLevel.ProjectileManager.ActiveProjectileList;
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Add)]
        protected static void DisplayNumberString(int number, string text, Color color, Vector2 position)
        {
            Game.ScreenManager.Player.AttachedLevel.TextManager.DisplayNumberStringText(number, text, color, position);
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Swap)]
        protected virtual void Update(GameTime gameTime)
        {
            Update(gameTime);
            RogueAPI.Game.InputManager.Update();
        }


        [Rewrite("RogueCastle.Game+SettingStruct")]
        public struct SettingStruct
        {
            [Rewrite]
            public int ScreenWidth;
            [Rewrite]
            public int ScreenHeight;
            [Rewrite]
            public bool FullScreen;
            [Rewrite]
            public float MusicVolume;
            [Rewrite]
            public float SFXVolume;
            [Rewrite]
            public bool QuickDrop;
            [Rewrite]
            public bool EnableDirectInput;
            [Rewrite]
            public byte ProfileSlot;
            [Rewrite]
            public bool ReduceQuality;
            [Rewrite]
            public bool EnableSteamCloud;
        }
    }

}
