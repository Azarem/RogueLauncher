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
            InputKeys input = 0;
            bool isButton = false;

            if (substring.StartsWith("Input"))
            {
                input = (InputKeys)byte.Parse(substring.Substring(6));
                isButton = InputManager.IsGamepadConnected();
            }
            else if (substring.StartsWith("Key"))
            {
                input = (InputKeys)byte.Parse(substring.Substring(4));
            }
            else if (substring.StartsWith("Button"))
            {
                input = (InputKeys)byte.Parse(substring.Substring(7));
                isButton = true;
            }
            else
                return firstPart + lastPart;


            KeyIconObj keyIconObj = new KeyIconObj();

            if (isButton)
            {
                keyIconObj.SetButton(InputManager.GetMappedButton(input));
            }
            else
            {
                var key = InputManager.GetMappedKey(input);
                keyIconObj.SetKey(key, key != Keys.Enter && key != Keys.Space);
            }

            var spacer = " ";
            var spacerWidth = Font.MeasureString(spacer).X * m_internalFontSizeScale.X * Scale.X;
            var spaceNum = (int)Math.Ceiling(keyIconObj.Width / spacerWidth);

            var height = Font.MeasureString("0").Y * m_internalFontSizeScale.Y * Scale.Y;

            var firstWidth = Font.MeasureString(firstPart).X * m_internalFontSizeScale.X * Scale.X;

            keyIconObj.Scale = new Vector2(height / keyIconObj.Height) * ForcedScale;

            m_yOffset = height / 2;
            m_iconList.Add(keyIconObj);
            m_iconOffset.Add(firstWidth + (spacerWidth * spaceNum / 2));

            return firstPart + new string(' ', spaceNum) + lastPart;
        }
    }
}
