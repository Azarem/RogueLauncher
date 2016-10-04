using AssemblyTranslator;
using DS2DEngine;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.OptionsObj")]
    public abstract class OptionsObj : ObjContainer
    {

        //protected bool m_isSelected;

        //protected bool m_isActive;

        //protected TextObj m_nameText;

        //protected OptionsScreen m_parentScreen;

        //protected int m_optionsTextOffset = 250;

        [Rewrite]
        public virtual bool IsActive
        {
            get; set;
            //get
            //{
            //    return this.m_isActive;
            //}
            //set
            //{
            //    if (!value)
            //    {
            //        this.IsSelected = true;
            //    }
            //    else
            //    {
            //        this.IsSelected = false;
            //    }
            //    this.m_isActive = value;
            //    if (!value)
            //    {
            //        (this.m_parentScreen.ScreenManager.Game as RogueCastle.Game).SaveConfig();
            //    }
            //}
        }

        [Rewrite]
        public bool IsSelected { get; set; }

        //public bool IsSelected
        //{
        //    get
        //    {
        //        return this.m_isSelected;
        //    }
        //    set
        //    {
        //        this.m_isSelected = value;
        //        if (value)
        //        {
        //            this.m_nameText.TextureColor = Color.Yellow;
        //            return;
        //        }
        //        this.m_nameText.TextureColor = Color.White;
        //    }
        //}

        //public OptionsObj(OptionsScreen parentScreen, string name)
        //{
        //    this.m_parentScreen = parentScreen;
        //    this.m_nameText = new TextObj(RogueCastle.Game.JunicodeFont)
        //    {
        //        FontSize = 12f,
        //        Text = name,
        //        DropShadow = new Vector2(2f, 2f)
        //    };
        //    this.AddChild(this.m_nameText);
        //    base.ForceDraw = true;
        //}

        //public override void Dispose()
        //{
        //    if (!base.IsDisposed)
        //    {
        //        this.m_parentScreen = null;
        //        this.m_nameText = null;
        //        base.Dispose();
        //    }
        //}

        [Rewrite(action: RewriteAction.Replace)]
        public virtual void HandleInput()
        {
            if (InputManager.IsNewlyPressed(InputFlags.MenuCancel1 | InputFlags.MenuCancel2))
                SoundManager.PlaySound("Options_Menu_Deselect");
        }

        [Rewrite]
        public virtual void Initialize()
        {
        }

        //public virtual void Update(GameTime gameTime)
        //{
        //}
    }
}
