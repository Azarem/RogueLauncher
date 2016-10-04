using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueAPI.Game;
using System;
using System.Collections.Generic;
using System.Reflection;
using Tweener;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.TutorialRoomObj")]
    public class TutorialRoomObj : RoomObj
    {
        [Rewrite]
        private KeyIconTextObj m_tutorialText;
        [Rewrite]
        private int m_waypointIndex;
        [Rewrite]
        private List<GameObj> m_waypointList;
        [Rewrite]
        private string[] m_tutorialTextList;
        [Rewrite]
        private string[] m_tutorialControllerTextList;
        [Rewrite]
        private TextObj m_creditsText;
        [Rewrite]
        private TextObj m_creditsTitleText;
        [Rewrite]
        private string[] m_creditsTextList;
        [Rewrite]
        private string[] m_creditsTextTitleList;
        [Rewrite]
        private Vector2 m_creditsPosition;
        [Rewrite]
        private int m_creditsIndex;
        [Rewrite]
        private SpriteObj m_diary;
        [Rewrite]
        private SpriteObj m_doorSprite;
        [Rewrite]
        private DoorObj m_door;
        [Rewrite]
        private SpriteObj m_speechBubble;

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public override void Update(GameTime gameTime)
        {
            if (Game.PlayerStats.TutorialComplete)
            {
                Rectangle bounds = m_diary.Bounds;
                bounds.X -= 50;
                bounds.Width += 100;

                m_speechBubble.Y = m_diary.Y - m_speechBubble.Height - 20f - 30f + (float)Math.Sin(Game.TotalGameTime * 20f) * 2f;

                var intersects = CollisionMath.Intersects(Player.Bounds, bounds);

                if (intersects && Player.IsTouchingGround)
                {
                    if (m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
                        m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");

                    if (InputManager.IsNewlyPressed(InputFlags.PlayerUp1 | InputFlags.PlayerUp2))
                    {
                        if (Game.PlayerStats.ReadLastDiary)
                        {
                            RCScreenManager screenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
                            screenManager.DisplayScreen(ScreenType.DiaryEntry, true, null);
                        }
                        else
                        {
                            RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                            rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + 24);
                            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "RunFlashback");
                            rCScreenManager.DisplayScreen(ScreenType.Dialogue, true, null);
                        }
                    }
                }
                else if (m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
                    m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");


                if (!Game.PlayerStats.ReadLastDiary || intersects)
                    m_speechBubble.Visible = true;
                else if (Game.PlayerStats.ReadLastDiary && !intersects)
                    m_speechBubble.Visible = false;
            }
            else
            {
                int lastWaypoint = m_waypointIndex;
                m_waypointIndex = PlayerNearWaypoint();
                if (m_waypointIndex != lastWaypoint)
                {
                    Tween.StopAllContaining(m_tutorialText, false);
                    if (m_waypointIndex == -1)
                        Tween.To(m_tutorialText, 0.25f, Tween.EaseNone, "Opacity", "0");
                    else
                    {
                        if (InputManager.IsGamepadConnected())
                            m_tutorialText.Text = m_tutorialControllerTextList[m_waypointIndex];
                        else
                            m_tutorialText.Text = m_tutorialTextList[m_waypointIndex];

                        Tween.To(m_tutorialText, 0.25f, Tween.EaseNone, "Opacity", "1");
                    }
                }
            }
            base.Update(gameTime);
        }

        [Rewrite]
        private int PlayerNearWaypoint() { return 0; }
    }
}
