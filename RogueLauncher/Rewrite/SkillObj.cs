using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.SkillObj")]
    public class SkillObj : SpriteObj
    {
        //[Rewrite]
        //private TextObj LevelText;
        //[Rewrite]
        //private SpriteObj m_coinIcon;
        [Rewrite]
        public SkillType TraitType { get; set; }
        //[Rewrite]
        //public byte StatType { get; set; }
        //[Rewrite]
        //public bool DisplayStat { get; set; }
        //[Rewrite]
        //public string InputDescription { get; set; }

        [Rewrite]
        public float ModifierAmount
        {
            get
            {
                return 0;
                //return (float)this.CurrentLevel * this.PerLevelModifier;
            }
        }

        [Rewrite]
        public SkillObj(string spriteName)
            : base(spriteName)
        {
            //this.StatType = 0;
            //this.DisplayStat = false;
            //base.Visible = false;
            //base.ForceDraw = true;
            //this.LevelText = new TextObj(Game.JunicodeFont)
            //{
            //    FontSize = 10f,
            //    Align = Types.TextAlign.Centre,
            //    OutlineWidth = 2
            //};
            //this.InputDescription = "";
            //base.OutlineWidth = 2;
            //this.m_coinIcon = new SpriteObj("UpgradeIcon_Sprite");
        }

        //[Rewrite]
        //protected override GameObj CreateCloneInstance()
        //{
        //    return null;
        //    //return new SkillObj(this._spriteName);
        //}

        //protected override void FillCloneInstance(object obj) { }
    }
}
