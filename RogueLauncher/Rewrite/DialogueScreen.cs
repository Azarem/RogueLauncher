using AssemblyTranslator;
using DS2DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.DialogueScreen")]
    public class DialogueScreen : Screen
    {
        [Rewrite]
        private ObjContainer m_dialogContainer;
        [Rewrite]
        private byte m_dialogCounter;
        [Rewrite]
        private string[] m_dialogTitles;
        [Rewrite]
        private string[] m_dialogText;
        [Rewrite]
        private float m_dialogContinueIconY;
        [Rewrite]
        private string m_dialogueObjName;
        [Rewrite]
        private MethodInfo m_confirmMethodInfo;
        [Rewrite]
        private object m_confirmMethodObj;
        [Rewrite]
        private object[] m_confirmArgs;
        [Rewrite]
        private MethodInfo m_cancelMethodInfo;
        [Rewrite]
        private object m_cancelMethodObj;
        [Rewrite]
        private object[] m_cancelArgs;
        [Rewrite]
        private bool m_runChoiceDialogue;
        [Rewrite]
        private ObjContainer m_dialogChoiceContainer;
        [Rewrite]
        private bool m_runCancelEndHandler;
        [Rewrite]
        private byte m_highlightedChoice = 2;
        [Rewrite]
        private bool m_lockControls;
        [Rewrite]
        private float m_textScrollSpeed = 0.03f;
        [Rewrite]
        private float m_inputDelayTimer;
        [Rewrite]
        public float BackBufferOpacity { get; set; }

        [Rewrite]
        public void SetDialogue(string dialogueObjName) { }
        [Rewrite]
        public void SetDialogueChoice(string dialogueObjName) { }
        [Rewrite]
        public void SetConfirmEndHandler(Type methodType, string functionName, params object[] args) { }
        [Rewrite]
        public void SetConfirmEndHandler(object methodObject, string functionName, params object[] args) { }
        [Rewrite]
        public void SetCancelEndHandler(Type methodType, string functionName, params object[] args) { }
        [Rewrite]
        public void SetCancelEndHandler(object methodObject, string functionName, params object[] args) { }
    }
}
