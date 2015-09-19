using System;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Classes;
using RogueAPI.Spells;
using RogueAPI.Traits;

namespace RogueLauncher.Rewrite
{
    [Obfuscation(Exclude = true)]
    [Rewrite("RogueCastle.LineageObj")]
    public class LineageObj : ObjContainer
    {
        [Rewrite]
        private ObjContainer m_playerSprite;
        [Rewrite]
        private TextObj m_playerName;
        [Rewrite]
        private SpriteObj m_plaqueSprite;
        [Rewrite]
        private int m_textYPos = 140;
        [Rewrite]
        private SpriteObj m_frameSprite;
        [Rewrite]
        private TextObj m_classTextObj;
        [Rewrite]
        private TextObj m_trait1Title;
        [Rewrite]
        private TextObj m_trait2Title;
        [Rewrite]
        private TextObj m_ageText;
        [Rewrite]
        private SpriteObj m_spellIcon;
        [Rewrite]
        private SpriteObj m_spellIconHolder;
        [Rewrite]
        public bool IsFemale;
        [Rewrite]
        public byte Age = 30;
        [Rewrite]
        public byte ChildAge = 4;
        [Rewrite]
        public byte Class;
        [Rewrite]
        public byte Spell;
        [Rewrite]
        public Vector2 Traits { get; internal set; }
        [Rewrite]
        public byte HeadPiece { get; set; }
        [Rewrite]
        public byte ShoulderPiece { get; set; }
        [Rewrite]
        public byte ChestPiece { get; set; }
        [Rewrite]
        public string PlayerName { get { return this.m_playerName.Text; } set { this.m_playerName.Text = value; } }
        //[Rewrite(action: RewriteAction.Add)]
        //public TraitDefinition[] TraitDefs;
        //[Rewrite(action: RewriteAction.Add)]
        //public ClassDefinition ClassDef;
        //[Rewrite(action: RewriteAction.Add)]
        //public SpellDefinition SpellDef;


        [Rewrite(action: RewriteAction.Replace)]
        public LineageObj(LineageScreen screen, bool createEmpty = false)
        {
            base.Name = "";
            this.m_frameSprite = new SpriteObj("LineageScreenFrame_Sprite")
            {
                Scale = new Vector2(2.8f, 2.8f),
                DropShadow = new Vector2(4f, 6f)
            };
            this.m_plaqueSprite = new SpriteObj("LineageScreenPlaque1Long_Sprite")
            {
                Scale = new Vector2(1.8f, 2f)
            };
            this.m_playerSprite = new ObjContainer("PlayerIdle_Character")
            {
                AnimationDelay = 0.1f,
                Scale = new Vector2(2f, 2f),
                OutlineWidth = 2
            };
            this.m_playerSprite.GetChildAt(10).Visible = false;
            this.m_playerSprite.GetChildAt(11).Visible = false;
            this.m_playerSprite.GetChildAt(1).TextureColor = Color.Red;
            this.m_playerSprite.GetChildAt(7).TextureColor = Color.Red;
            this.m_playerSprite.GetChildAt(14).Visible = false;
            this.m_playerSprite.GetChildAt(16).Visible = false;
            this.m_playerSprite.GetChildAt(13).TextureColor = new Color(251, 156, 172);

            this.AddChild(this.m_playerName = new TextObj(Game.JunicodeFont)
            {
                FontSize = 10f,
                Text = "Sir Skunky IV",
                Align = Types.TextAlign.Centre,
                OutlineColour = new Color(181, 142, 39),
                OutlineWidth = 2,
                Y = this.m_textYPos,
                LimitCorners = true
            });

            this.AddChild(this.m_classTextObj = new TextObj(Game.JunicodeFont)
            {
                FontSize = 8f,
                Align = Types.TextAlign.Centre,
                OutlineColour = new Color(181, 142, 39),
                OutlineWidth = 2,
                Text = "the Knight",
                Y = this.m_playerName.Y + (float)this.m_playerName.Height - 8f,
                LimitCorners = true
            });

            this.AddChild(this.m_trait1Title = new TextObj(Game.JunicodeFont)
            {
                FontSize = 8f,
                Align = Types.TextAlign.Centre,
                OutlineColour = new Color(181, 142, 39),
                OutlineWidth = 2,
                Y = this.m_classTextObj.Y + (float)this.m_classTextObj.Height + 5f,
                Text = "",
                LimitCorners = true
            });

            this.AddChild(this.m_trait2Title = this.m_trait1Title.Clone() as TextObj);
            this.m_trait2Title.Y += 20f;
            this.m_trait2Title.Text = "";
            this.m_trait2Title.LimitCorners = true;

            this.AddChild(this.m_ageText = this.m_trait1Title.Clone() as TextObj);
            this.m_ageText.Text = "xxx - xxx";
            this.m_ageText.Visible = false;
            this.m_ageText.LimitCorners = true;

            this.m_spellIcon = new SpriteObj("Blank_Sprite") { OutlineWidth = 1 };

            this.m_spellIconHolder = new SpriteObj("BlacksmithUI_IconBG_Sprite");

            if (!createEmpty)
            {
                this.IsFemale = CDGMath.RandomInt(0, 1) == 1;

                if (!this.IsFemale)
                    this.CreateMaleName(screen);
                else
                    this.CreateFemaleName(screen);

                this.Class = ClassType.GetRandomClass();
                var classDef = ClassDefinition.GetById(this.Class);

                this.m_classTextObj.Text = "the " + classDef.GetDisplayName(this.IsFemale);

                var traitDefs = TraitType.CreateRandomTraits(classDef);
                var traitVec = Vector2.Zero;
                for (int ix = 0; ix < traitDefs.Length; ix++)
                {
                    if (ix == 0)
                        traitVec.X = traitDefs[ix].TraitId;
                    else
                        traitVec.Y = traitDefs[ix].TraitId;
                }

                this.Traits = traitVec;

                var spellDef = RogueAPI.Spells.SpellDefinition.GetRandomSpell(classDef, traitDefs);


                this.Spell = spellDef.SpellId;
                this.Age = (byte)CDGMath.RandomInt(18, 30);
                this.ChildAge = (byte)CDGMath.RandomInt(2, 5);
                this.UpdateData();
            }
        }

        [Rewrite]
        private void CreateMaleName(LineageScreen screen) { }
        [Rewrite]
        private void CreateFemaleName(LineageScreen screen) { }
        [Rewrite]
        public void UpdateData() { }
    }
}
