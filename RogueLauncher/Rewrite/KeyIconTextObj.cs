using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.KeyIconTextObj")]
    public class KeyIconTextObj : TextObj
    {
        [Rewrite]
        private List<KeyIconObj> m_iconList;
        [Rewrite]
        private List<float> m_iconOffset;
        [Rewrite]
        public Vector2 ForcedScale { get; set; }
        [Rewrite]
        private float m_yOffset;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public string AddKeyIcon(string text, int startIndex, int endIndex)
        {
            string firstPart = text.Substring(0, startIndex);
            string lastPart = endIndex + 1 < text.Length ? text.Substring(endIndex + 1) : "";
            string substring = text.Substring(startIndex + 1, endIndex - startIndex - 1);
            Keys? key = null;
            Buttons? button = null;

            if (substring.StartsWith("Input"))
            {
                var input = (InputKeys)byte.Parse(substring.Substring(6));
                if (InputManager.IsGamepadConnected())
                    button = InputManager.GetMappedButton(input);
                else
                    key = InputManager.GetMappedKey(input);
            }
            else if (substring.StartsWith("Key"))
                key = (Keys)Enum.Parse(typeof(Keys), substring.Substring(4));
            else if (substring.StartsWith("Button"))
                button = (Buttons)Enum.Parse(typeof(Buttons), substring.Substring(7));
            else
                return firstPart + lastPart;


            KeyIconObj keyIconObj = new KeyIconObj();

            if (button != null)
                keyIconObj.SetButton(button.Value);
            else
                keyIconObj.SetKey(key, key != Keys.Enter && key != Keys.Space);

            var scale = m_internalFontSizeScale * Scale;
            var height = Font.MeasureString("0").Y * scale.Y;
            keyIconObj.Scale = new Vector2(height / keyIconObj.Height) * ForcedScale;

            var spacer = " ";
            var spacerWidth = Font.MeasureString(spacer).X * scale.X;
            var spaceNum = (int)Math.Ceiling(keyIconObj.Width / spacerWidth);

            var firstWidth = Font.MeasureString(firstPart).X * scale.X;

            m_yOffset = height / 2;
            m_iconList.Add(keyIconObj);
            m_iconOffset.Add(firstWidth + (spacerWidth * spaceNum / 2));

            return firstPart + new string(' ', spaceNum) + lastPart;
        }
    }
}
