using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.DeadZoneOptionsObj")]
    public class DeadZoneOptionsObj : OptionsObj
    {
        [Rewrite]
        private SpriteObj m_deadZoneBarBG;
        [Rewrite]
        private SpriteObj m_deadZoneBar;
        //[Rewrite]
        //public override bool IsActive { get { return base.IsActive; } set { base.IsActive = value; } }
        //{
        //    get
        //    {
        //        return base.IsActive;
        //    }
        //    set
        //    {
        //        base.IsActive = value;
        //        if (value)
        //        {
        //            this.m_deadZoneBar.TextureColor = Color.Yellow;
        //            return;
        //        }
        //        this.m_deadZoneBar.TextureColor = Color.White;
        //    }
        //}

        //public DeadZoneOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Joystick Dead Zone")
        //{
        //    this.m_deadZoneBarBG = new SpriteObj("OptionsScreenVolumeBG_Sprite")
        //    {
        //        X = (float)this.m_optionsTextOffset,
        //        Y = (float)this.m_deadZoneBarBG.Height / 2f - 2f
        //    };
        //    this.AddChild(this.m_deadZoneBarBG);
        //    this.m_deadZoneBar = new SpriteObj("OptionsScreenVolumeBar_Sprite")
        //    {
        //        X = this.m_deadZoneBarBG.X + 6f,
        //        Y = this.m_deadZoneBarBG.Y + 5f
        //    };
        //    this.AddChild(this.m_deadZoneBar);
        //}

        //public override void Dispose()
        //{
        //    if (!base.IsDisposed)
        //    {
        //        this.m_deadZoneBar = null;
        //        this.m_deadZoneBarBG = null;
        //        base.Dispose();
        //    }
        //}

        public override void HandleInput()
        {
            if (InputManager.IsPressedOr(InputKeys.PlayerLeft1, InputKeys.PlayerLeft2) && InputManager.ThumbstickDeadzone - 1f >= 0f)
            {
                InputManager.ThumbstickDeadzone -= 1f;
                UpdateDeadZoneBar();
            }
            else if (InputManager.IsPressedOr(InputKeys.PlayerRight1, InputKeys.PlayerRight2) && InputManager.ThumbstickDeadzone + 1f <= 95f)
            {
                InputManager.ThumbstickDeadzone += 1f;
                UpdateDeadZoneBar();
            }

            if (InputManager.IsNewlyPressedOr(InputKeys.MenuConfirm1, InputKeys.MenuConfirm2, InputKeys.MenuCancel1, InputKeys.MenuCancel2))
                IsActive = false;

            base.HandleInput();
        }

        public override void Initialize()
        {
            this.m_deadZoneBar.ScaleX = InputManager.ThumbstickDeadzone / 95f;
            base.Initialize();
        }


        public void UpdateDeadZoneBar()
        {
            this.m_deadZoneBar.ScaleX = InputManager.ThumbstickDeadzone / 95f;
        }
    }
}
