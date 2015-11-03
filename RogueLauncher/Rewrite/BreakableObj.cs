using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Reflection;
using Tweener;
using Tweener.Ease;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.BreakableObj")]
    public class BreakableObj : PhysicsObjContainer
    {
        [Rewrite]
        private bool m_internalIsWeighted;
        [Rewrite]
        public bool Broken { get; internal set; }
        [Rewrite]
        public bool DropItem { get; set; }

        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void Break()
        {
            PlayerObj player = Game.ScreenManager.Player;
            foreach (GameObj gameObj in this._objectList)
            {
                gameObj.Visible = true;
            }
            base.GoToFrame(2);
            this.Broken = true;
            this.m_internalIsWeighted = base.IsWeighted;
            base.IsWeighted = false;
            base.IsCollidable = false;
            if (this.DropItem)
            {
                bool flag = false;
                if (base.Name == "Health")
                {
                    player.AttachedLevel.ItemDropManager.DropItem(base.Position, 2, 0.1f);
                    flag = true;
                }
                else if (base.Name == "Mana")
                {
                    player.AttachedLevel.ItemDropManager.DropItem(base.Position, 3, 0.1f);
                    flag = true;
                }
                if (flag)
                {
                    for (int i = 0; i < base.NumChildren; i++)
                    {
                        GameObj childAt = base.GetChildAt(i);
                        Easing easing = new Easing(Linear.EaseNone);
                        string[] str = new string[] { "X", null, null, null, null, null };
                        int num = CDGMath.RandomInt(-50, 50);
                        str[1] = num.ToString();
                        str[2] = "Y";
                        str[3] = "50";
                        str[4] = "Rotation";
                        int num1 = CDGMath.RandomInt(-360, 360);
                        str[5] = num1.ToString();
                        Tween.By(childAt, 0.3f, easing, str);
                        GameObj childAt1 = base.GetChildAt(i);
                        Easing easing1 = new Easing(Linear.EaseNone);
                        string[] strArrays = new string[] { "delay", "0.2", "Opacity", "0" };
                        Tween.To(childAt1, 0.1f, easing1, strArrays);
                    }
                    PlayerObj playerObj = Game.ScreenManager.Player;
                    string[] strArrays1 = new string[] { "EnemyHit1", "EnemyHit2", "EnemyHit3", "EnemyHit4", "EnemyHit5", "EnemyHit6" };
                    SoundManager.Play3DSound(this, playerObj, strArrays1);
                    PlayerObj player1 = Game.ScreenManager.Player;
                    string[] strArrays2 = new string[] { "Break1", "Break2", "Break3" };
                    SoundManager.Play3DSound(this, player1, strArrays2);
                    if (Game.PlayerStats.Traits.X == 15f || Game.PlayerStats.Traits.Y == 15f)
                    {
                        PlayerObj currentMana = player;
                        currentMana.CurrentMana = currentMana.CurrentMana + 1f;
                        TextManager textManager = player.AttachedLevel.TextManager;
                        Color royalBlue = Color.RoyalBlue;
                        float x = player.X;
                        Rectangle bounds = player.Bounds;
                        textManager.DisplayNumberStringText(1, "mp", royalBlue, new Vector2(x, (float)(bounds.Top - 30)));
                    }
                    return;
                }

                var drop = RogueAPI.Game.RandomItemDrop.PipeRandomDrop(this);
                if(drop.Target != null && !drop.Handled)
                    player.AttachedLevel.ItemDropManager.DropItem(base.Position, drop.Target.DropType, drop.Target.Amount);

                //int num2 = CDGMath.RandomInt(1, 100);
                //int bREAKABLEITEMDROPCHANCE = 0;
                //int num3 = 0;
                //while (num3 < (int)GameEV.BREAKABLE_ITEMDROP_CHANCE.Length)
                //{
                //    bREAKABLEITEMDROPCHANCE = bREAKABLEITEMDROPCHANCE + GameEV.BREAKABLE_ITEMDROP_CHANCE[num3];
                //    if (num2 > bREAKABLEITEMDROPCHANCE)
                //    {
                //        num3++;
                //    }
                //    else if (num3 == 0)
                //    {
                //        if (Game.PlayerStats.Traits.X == 24f || Game.PlayerStats.Traits.Y == 24f)
                //        {
                //            EnemyObj_Chicken enemyObjChicken = new EnemyObj_Chicken(null, null, null, GameTypes.EnemyDifficulty.BASIC)
                //            {
                //                AccelerationY = -500f,
                //                Position = base.Position
                //            };
                //            EnemyObj_Chicken y = enemyObjChicken;
                //            y.Y = y.Y - 50f;
                //            enemyObjChicken.SaveToFile = false;
                //            player.AttachedLevel.AddEnemyToCurrentRoom(enemyObjChicken);
                //            enemyObjChicken.IsCollidable = false;
                //            Tween.RunFunction(0.2f, enemyObjChicken, "MakeCollideable", new object[0]);
                //            PlayerObj playerObj1 = Game.ScreenManager.Player;
                //            string[] strArrays3 = new string[] { "Chicken_Cluck_01", "Chicken_Cluck_02", "Chicken_Cluck_03" };
                //            SoundManager.Play3DSound(this, playerObj1, strArrays3);
                //            break;
                //        }
                //        else
                //        {
                //            player.AttachedLevel.ItemDropManager.DropItem(base.Position, 2, 0.1f);
                //            break;
                //        }
                //    }
                //    else if (num3 == 1)
                //    {
                //        player.AttachedLevel.ItemDropManager.DropItem(base.Position, 3, 0.1f);
                //        break;
                //    }
                //    else if (num3 != 2)
                //    {
                //        if (num3 != 3)
                //        {
                //            break;
                //        }
                //        player.AttachedLevel.ItemDropManager.DropItem(base.Position, 10, 100f);
                //        break;
                //    }
                //    else
                //    {
                //        player.AttachedLevel.ItemDropManager.DropItem(base.Position, 1, 10f);
                //        break;
                //    }
                //}
            }
            for (int j = 0; j < base.NumChildren; j++)
            {
                GameObj gameObj1 = base.GetChildAt(j);
                Easing easing2 = new Easing(Linear.EaseNone);
                string[] str1 = new string[] { "X", null, null, null, null, null };
                int num4 = CDGMath.RandomInt(-50, 50);
                str1[1] = num4.ToString();
                str1[2] = "Y";
                str1[3] = "50";
                str1[4] = "Rotation";
                int num5 = CDGMath.RandomInt(-360, 360);
                str1[5] = num5.ToString();
                Tween.By(gameObj1, 0.3f, easing2, str1);
                GameObj childAt2 = base.GetChildAt(j);
                Easing easing3 = new Easing(Linear.EaseNone);
                string[] strArrays4 = new string[] { "delay", "0.2", "Opacity", "0" };
                Tween.To(childAt2, 0.1f, easing3, strArrays4);
            }
            PlayerObj player2 = Game.ScreenManager.Player;
            string[] strArrays5 = new string[] { "EnemyHit1", "EnemyHit2", "EnemyHit3", "EnemyHit4", "EnemyHit5", "EnemyHit6" };
            SoundManager.Play3DSound(this, player2, strArrays5);
            PlayerObj playerObj2 = Game.ScreenManager.Player;
            string[] strArrays6 = new string[] { "Break1", "Break2", "Break3" };
            SoundManager.Play3DSound(this, playerObj2, strArrays6);
            if (Game.PlayerStats.Traits.X == 15f || Game.PlayerStats.Traits.Y == 15f)
            {
                PlayerObj currentMana1 = player;
                currentMana1.CurrentMana = currentMana1.CurrentMana + 1f;
                TextManager textManager1 = player.AttachedLevel.TextManager;
                Color color = Color.RoyalBlue;
                float single = player.X;
                Rectangle rectangle = player.Bounds;
                textManager1.DisplayNumberStringText(1, "mp", color, new Vector2(single, (float)(rectangle.Top - 30)));
            }
        }
    }
}
