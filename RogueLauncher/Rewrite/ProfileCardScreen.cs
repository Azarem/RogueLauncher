using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProfileCardScreen")]
    public class ProfileCardScreen
    {
        [Rewrite]
        private List<TextObj> m_dataList1;
        [Rewrite]
        private List<TextObj> m_dataList2;
        [Rewrite]
        private List<TextObj> m_equipmentList;
        [Rewrite]
        private TextObj m_equipmentTitle;
        [Rewrite]
        private TextObj m_runesTitle;
        [Rewrite]
        private List<TextObj> m_runeBackTitleList;
        [Rewrite]
        private List<TextObj> m_runeBackDescriptionList;
        [Rewrite]
        private ObjContainer m_backCard;

        [Obfuscation(Exclude = true)]
        [Rewrite(action: RewriteAction.Replace)]
        private void LoadBackCardStats(PlayerObj player)
        {
            TextObj obj1, obj2;

            int ix = 0, count = this.m_dataList1.Count;
            while(ix < count)
            {
                obj1 = this.m_dataList1[ix];
                obj2 = this.m_dataList2[ix];

                switch(ix)
                {
                    case 0:
                        obj1.Text = player.MaxHealth.ToString();
                        obj2.Text = player.Damage.ToString();
                        break;

                    case 1:
                        obj1.Text = player.MaxMana.ToString();
                        obj2.Text = player.TotalMagicDamage.ToString();
                        break;

                    case 2:
                        obj1.Text = player.TotalArmor + "(" + (int)(player.TotalDamageReduc * 100f) + "%)";
                        obj2.Text = (int)Math.Round(player.TotalCritChance * 100f, MidpointRounding.AwayFromZero) + "%";
                        break;

                    case 3:
                        obj1.Text = player.CurrentWeight + "/" + player.MaxWeight;
                        obj2.Text = (int)(player.TotalCriticalDamage * 100f) + "%";
                        break;
                }

                ix++;
            }

            var getEquippedArray = Game.PlayerStats.GetEquippedArray;
            ix = 0;
            count = getEquippedArray.Length;
            int posY = (int)this.m_equipmentTitle.Y + 40;
            while(ix < count)
            {
                int id = getEquippedArray[ix];
                obj1 = this.m_equipmentList[ix];

                if(id >= 0)
                {
                    obj1.Y = posY;
                    obj1.Visible = true;
                    obj1.Text = Game.EquipmentSystem.EquipmentDataArray[ix][id].DisplayName;
                    posY += 20;
                }
                else
                    obj1.Visible = false;

                ix++;
            }

            ix = 0;
            count = this.m_runeBackTitleList.Count;
            posY = (int)this.m_runesTitle.Y + 40;
            while(ix < count)
            {
                obj1 = this.m_runeBackTitleList[ix];
                obj2 = this.m_runeBackDescriptionList[ix];

                float value = 0f;
                switch(ix)
                {
                    case 0: value = player.TotalDoubleJumps; break;
                    case 1: value = player.TotalAirDashes; break;
                    case 2: value = player.TotalVampBonus; break;
                    case 3: value = player.TotalFlightTime; break;
                    case 4: value = player.ManaGain; break;
                    case 5: value = player.TotalDamageReturn * 100f; break;
                    case 6: value = player.TotalGoldBonus * 100f; break;
                    case 7: value = player.TotalMovementSpeedPercent * 100f - 100f; break;
                    case 8: value = Game.PlayerStats.GetNumberOfEquippedRunes(8) * 8; break;
                    case 9: value = Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f; break;
                    case 10: value = Game.PlayerStats.HasArchitectFee ? 1f : 0f; break;
                    case 11: value = Game.PlayerStats.TimesCastleBeaten * 50; break;
                }

                if(value > 0f)
                {
                    obj2.Text = "(" + EquipmentAbilityType.ShortDescription(ix + (ix > 9 ? 10 : 0), value) + ")";
                    obj1.Visible = true;
                    obj2.Visible = true;
                    obj1.Y = posY;
                    obj2.Y = posY;
                    posY += 20;
                }
                else
                {
                    obj1.Visible = false;
                    obj2.Visible = false;
                }

                ix++;
            }

            (this.m_backCard.GetChildAt(3) as TextObj).Text = Game.PlayerStats.PlayerName;
        }
    }


}
