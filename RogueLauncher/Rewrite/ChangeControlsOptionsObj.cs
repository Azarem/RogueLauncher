using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
    [Rewrite("RogueCastle.ChangeControlsOptionsObj")]
    public class ChangeControlsOptionsObj : OptionsObj
    {
        [Rewrite]
        private List<TextObj> m_buttonTitle;
        [Rewrite]
        private List<KeyIconTextObj> m_keyboardControls;
        [Rewrite]
        private List<KeyIconTextObj> m_gamepadControls;
        [Rewrite]
        private int m_selectedEntryIndex;
        [Rewrite]
        private TextObj m_selectedEntry;
        [Rewrite]
        private KeyIconTextObj m_selectedButton;
        [Rewrite]
        private ObjContainer m_setKeyPlate;
        [Rewrite]
        private bool m_settingKey;
        [Rewrite]
        private int[] m_controlKeys;
        [Rewrite]
        private bool m_lockControls;
        [Rewrite]
        private int m_startingY = -200;
        [Rewrite]
        private SpriteObj m_selectionBar;
        [Rewrite]
        public override bool IsActive { get; set; }

        [Rewrite(action: RewriteAction.Add)]
        private static readonly Keys[] _disabledKeys = new Keys[] { Keys.Tab, Keys.CapsLock, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt, Keys.RightAlt, Keys.RightControl, Keys.RightShift, Keys.Enter, Keys.Back, Keys.Space, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
        [Rewrite(action: RewriteAction.Add)]
        private static readonly int[] _menuKeyMap = new int[] { 0, 1, 2, 3, 6, 4, 8, 7, 5 };

        [Rewrite(action: RewriteAction.Add)]
        static ChangeControlsOptionsObj()
        {

        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        private void ChangeKey()
        {
            Keys keyPressed = 0;
            Buttons buttonPressed = 0;

            if (InputManager.IsAnyKeyPressed())
            {
                keyPressed = InputManager.PressedKeys[0];
                if (InputSystem.InputReader.GetInputString(keyPressed, false, false, false).ToUpper() == "")
                    return;

                if (_disabledKeys.Contains(keyPressed))
                    return;

                if (keyPressed == Keys.Escape)
                {
                    Tween.To(m_setKeyPlate, 0.3f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
                    m_settingKey = false;
                    return;
                }
            }
            else if (InputManager.IsAnyButtonPressed())
            {
                buttonPressed = InputManager.PressedButtons;
                if (buttonPressed == Buttons.Start || buttonPressed == Buttons.Back)
                    return;
            }

            SoundManager.PlaySound("Gen_Menu_Toggle");
            Tween.To(m_setKeyPlate, 0.3f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");   
            m_settingKey = false;

            if (buttonPressed != 0)
            {
                InputManager.MapButton(buttonPressed, (InputKeys)m_controlKeys[m_selectedEntryIndex]);
                //var mapArr = InputManager.ButtonMap;
                //for (int keyIndex = 0; keyIndex < mapArr.Length; keyIndex++)
                //{
                //    bool isPlayerKey = !_menuKeyMap.Contains(keyIndex);
                //    if (isPlayerKey && mapArr[keyIndex] == buttonPressed)
                //        mapArr[keyIndex] = mapArr[m_controlKeys[m_selectedEntryIndex]];
                //}
                //mapArr[m_controlKeys[m_selectedEntryIndex]] = buttonPressed;
            }
            else if (keyPressed != 0)
            {
                InputManager.MapKey(keyPressed, (InputKeys)m_controlKeys[m_selectedEntryIndex]);
                //var mapArr = InputManager.KeyMap;
                //for (int keyIndex = 0; keyIndex < mapArr.Length; keyIndex++)
                //{
                //    bool isPlayerKey = !_menuKeyMap.Contains(keyIndex);
                //    if (isPlayerKey && mapArr[keyIndex] == keyPressed)
                //        mapArr[keyIndex] = mapArr[m_controlKeys[m_selectedEntryIndex]];
                //}
                //mapArr[m_controlKeys[m_selectedEntryIndex]] = keyPressed;
            }

            UpdateKeyBindings();
        }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void HandleInput()
        {
            if (m_lockControls)
                return;

            if (m_settingKey)
            {
                if (InputManager.IsAnyButtonPressed() || InputManager.IsAnyKeyPressed())
                    ChangeKey();

                return;
            }

            int lastIndex = m_selectedEntryIndex;

            if (InputManager.IsNewlyPressed(InputFlags.PlayerUp1 | InputFlags.PlayerUp2))
            {
                SoundManager.PlaySound("frame_swap");
                m_selectedEntryIndex--;
            }
            else if (InputManager.IsNewlyPressed(InputFlags.PlayerDown1 | InputFlags.PlayerDown2))
            {
                SoundManager.PlaySound("frame_swap");
                m_selectedEntryIndex++;
            }

            if (m_selectedEntryIndex > m_buttonTitle.Count - 1)
                m_selectedEntryIndex = 0;
            else if (m_selectedEntryIndex < 0)
                m_selectedEntryIndex = m_buttonTitle.Count - 1;

            if (lastIndex != m_selectedEntryIndex)
            {
                m_selectedEntry.TextureColor = Color.White;
                m_selectedEntry = m_buttonTitle[m_selectedEntryIndex];
                m_selectedEntry.TextureColor = Color.Yellow;
            }

            if (InputManager.IsNewlyPressed(InputFlags.MenuConfirm1 | InputFlags.MenuConfirm2))
            {
                SoundManager.PlaySound("Option_Menu_Select");
                m_lockControls = true;

                if (m_selectedEntryIndex == m_controlKeys.Length - 1)
                {
                    var screenManager = Game.ScreenManager;
                    screenManager.DialogueScreen.SetDialogue("RestoreDefaultControlsWarning");
                    screenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    screenManager.DialogueScreen.SetConfirmEndHandler(this, "RestoreControls");
                    screenManager.DialogueScreen.SetCancelEndHandler(this, "CancelRestoreControls");
                    screenManager.DisplayScreen(13, true, null);
                }
                else
                {
                    Tween.To(m_setKeyPlate, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
                    Tween.AddEndHandlerToLastTween(this, "SetKeyTrue");
                }
            }
            else if (InputManager.IsNewlyPressed(InputFlags.MenuCancel1 | InputFlags.MenuCancel2))
                IsActive = false;

            base.HandleInput();
        }


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        private void UpdateKeyBindings()
        {
            for (int i = 0; i < m_keyboardControls.Count; i++)
            {
                var input = (InputKeys)m_controlKeys[i];

                if ((int)input != -1)
                {
                    var button = InputManager.GetMappedButton(input);
                    if (button == 0)
                        m_gamepadControls[i].Text = "";
                    else
                        m_gamepadControls[i].Text = "[Button:" + button + "]";

                    var key = InputManager.GetMappedKey(input);
                    if (key == 0)
                        m_keyboardControls[i].Text = "";
                    else
                    {
                        var baseStr = "[Key:" + key + "]";
                        InputKeys altKey = 0;

                        switch (input)
                        {
                            case InputKeys.PlayerJump1: altKey = InputKeys.PlayerJump2; break;
                            case InputKeys.PlayerUp1: altKey = InputKeys.PlayerUp2; break;
                            case InputKeys.PlayerDown1: altKey = InputKeys.PlayerDown2; break;
                            case InputKeys.PlayerLeft1: altKey = InputKeys.PlayerLeft2; break;
                            case InputKeys.PlayerRight1: altKey = InputKeys.PlayerRight2; break;
                        }

                        if (altKey != 0)
                            baseStr += ", [Key:" + InputManager.GetMappedKey(altKey) + "]";

                        m_keyboardControls[i].Text = baseStr;
                    }
                }
                else
                {
                    m_keyboardControls[i].Text = "";
                    m_gamepadControls[i].Text = "";
                }
            }

            InputManager.MapKey(InputManager.GetMappedKey(InputKeys.PlayerAttack), InputKeys.MenuConfirm2);
            InputManager.MapKey(InputManager.GetMappedKey(InputKeys.PlayerJump1), InputKeys.MenuCancel2);
        }
    }
}
