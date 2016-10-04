using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tweener;
using Tweener.Ease;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.OptionsScreen")]
    public class OptionsScreen : Screen
    {
        [Rewrite]
        private OptionsObj m_selectedOption;
        [Rewrite]
        private int m_selectedOptionIndex;
        [Rewrite]
        private bool m_changingControls;
        [Rewrite]
        private List<OptionsObj> m_optionsArray;
        [Rewrite]
        private ObjContainer m_bgSprite;
        [Rewrite]
        private bool m_transitioning;
        [Rewrite]
        private OptionsObj m_backToMenuObj;
        [Rewrite]
        private KeyIconTextObj m_confirmText;
        [Rewrite]
        private KeyIconTextObj m_cancelText;
        [Rewrite]
        private KeyIconTextObj m_navigationText;
        [Rewrite]
        private SpriteObj m_optionsTitle;
        [Rewrite]
        private SpriteObj m_changeControlsTitle;
        [Rewrite]
        private SpriteObj m_optionsBar;
        [Rewrite]
        private TextObj m_quickDropText;
        [Rewrite]
        private OptionsObj m_quickDropObj;
        [Rewrite]
        private OptionsObj m_reduceQualityObj;
        [Rewrite]
        private OptionsObj m_enableSteamCloudObj;
        [Rewrite]
        private bool m_titleScreenOptions;


        [Rewrite(action: RewriteAction.Replace)]
        public override void HandleInput()
        {
            if (m_transitioning)
                m_quickDropText.Visible = false;
            else
            {
                if (!m_selectedOption.IsActive)
                {
                    int oldIndex = m_selectedOptionIndex;

                    if (InputManager.IsNewlyPressed(InputFlags.PlayerUp1 | InputFlags.PlayerUp2))
                    {
                        if (m_selectedOptionIndex > 0)
                            SoundManager.PlaySound("frame_swap");

                        m_selectedOptionIndex--;
                    }
                    else if (InputManager.IsNewlyPressed(InputFlags.PlayerDown1 | InputFlags.PlayerDown2))
                    {
                        if (m_selectedOptionIndex < m_optionsArray.Count - 1)
                            SoundManager.PlaySound("frame_swap");

                        m_selectedOptionIndex++;
                    }

                    if (m_selectedOptionIndex < 0)
                        m_selectedOptionIndex = m_optionsArray.Count - 1;
                    else if (m_selectedOptionIndex > m_optionsArray.Count - 1)
                        m_selectedOptionIndex = 0;

                    if (oldIndex != m_selectedOptionIndex)
                    {
                        if (m_selectedOption != null)
                            m_selectedOption.IsSelected = false;

                        m_selectedOption = m_optionsArray[m_selectedOptionIndex];
                        m_selectedOption.IsSelected = true;
                    }

                    if (InputManager.IsNewlyPressed(InputFlags.MenuConfirm1 | InputFlags.MenuConfirm2))
                    {
                        SoundManager.PlaySound("Option_Menu_Select");
                        m_selectedOption.IsActive = true;
                    }

                    if (InputManager.IsNewlyPressed(InputFlags.MenuCancel1 | InputFlags.MenuCancel2 | InputFlags.MenuOptions))
                        ExitTransition();
                }
                else
                    m_selectedOption.HandleInput();

                if (m_selectedOption == m_quickDropObj)
                {
                    m_quickDropText.Visible = true;
                    m_quickDropText.Text = "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
                }
                else if (m_selectedOption == m_reduceQualityObj)
                {
                    m_quickDropText.Visible = true;
                    m_quickDropText.Text = "*The game must be restarted for this change to come into effect.";
                }
                else if (m_selectedOption == m_enableSteamCloudObj)
                {
                    m_quickDropText.Visible = true;
                    m_quickDropText.Text = "*Cloud support must be enabled on the Steam platform as well for\nthis feature to work.";
                }
                else
                    m_quickDropText.Visible = false;
            }

            base.HandleInput();
        }

        [Rewrite]
        private void ExitTransition() { }


        [Rewrite(action: RewriteAction.Replace)]
        public override void OnEnter()
        {
            m_quickDropText.Visible = false;
            if (!InputManager.IsGamepadConnected())
            {
                m_confirmText.ForcedScale = new Vector2(1f, 1f);
                m_cancelText.ForcedScale = new Vector2(1f, 1f);
                m_navigationText.Text = "Arrow keys to navigate options";
            }
            else
            {
                m_confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
                m_cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
                m_navigationText.Text = "[Button:LeftStick] to navigate options";
            }

            m_confirmText.Text = "[Input:" + (int)InputKeys.MenuConfirm1 + "] to select option";
            m_cancelText.Text = "[Input:" + (int)InputKeys.MenuCancel1 + "] to exit options";
            m_confirmText.Opacity = 0f;
            m_cancelText.Opacity = 0f;
            m_navigationText.Opacity = 0f;

            Tween.To(m_confirmText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(m_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(m_navigationText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "DialogueMenuOpen");

            if (!m_optionsArray.Contains(m_backToMenuObj))
                m_optionsArray.Insert(m_optionsArray.Count - 1, m_backToMenuObj);

            if (m_titleScreenOptions)
                m_optionsArray.RemoveAt(m_optionsArray.Count - 2);

            m_transitioning = true;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");

            m_selectedOptionIndex = 0;
            m_selectedOption = m_optionsArray[m_selectedOptionIndex];
            m_selectedOption.IsActive = false;
            m_bgSprite.Position = new Vector2(660f, 0f);
            m_bgSprite.Opacity = 0f;

            Tween.To(m_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(m_bgSprite, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
            Tween.AddEndHandlerToLastTween(this, "EndTransition");

            int num = 0;
            foreach (OptionsObj opt in m_optionsArray)
            {
                opt.Y = (160 + num * 30) - 360f;
                opt.Opacity = 0f;
                Tween.By(opt, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
                Tween.To(opt, 0.2f, Tween.EaseNone, "Opacity", "1");
                opt.Initialize();
                num++;
            }

            m_optionsBar.Opacity = 0f;
            Tween.To(m_optionsBar, 0.2f, Tween.EaseNone, "Opacity", "1");
            base.OnEnter();
        }
    }
}
