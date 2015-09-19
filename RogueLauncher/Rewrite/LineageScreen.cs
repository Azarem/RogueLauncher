using System;
using System.Linq;
using System.Reflection;
using AssemblyTranslator;
using DS2DEngine;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.LineageScreen")]
    public class LineageScreen : Screen
    {
        [Rewrite]
        private LineageObj m_selectedLineageObj;

        //[Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        //public void StartGame()
        //{
        //    Game.PlayerStats.HeadPiece = this.m_selectedLineageObj.HeadPiece;
        //    Game.PlayerStats.ShoulderPiece = this.m_selectedLineageObj.ShoulderPiece;
        //    Game.PlayerStats.ChestPiece = this.m_selectedLineageObj.ChestPiece;
        //    Game.PlayerStats.IsFemale = this.m_selectedLineageObj.IsFemale;
        //    Game.PlayerStats.Class = this.m_selectedLineageObj.Class;
        //    Game.PlayerStats.Traits = this.m_selectedLineageObj.Traits;
        //    Game.PlayerStats.Spell = this.m_selectedLineageObj.Spell;
        //    Game.PlayerStats.PlayerName = this.m_selectedLineageObj.PlayerName;
        //    Game.PlayerStats.Age = this.m_selectedLineageObj.Age;
        //    Game.PlayerStats.ChildAge = this.m_selectedLineageObj.ChildAge;

        //    if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
        //        Game.PlayerStats.WizardSpellList = SpellType.GetNext3Spells();

        //    Game.PlayerStats.CurrentBranches.Clear();
        //    (base.ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
        //}
    }
}
