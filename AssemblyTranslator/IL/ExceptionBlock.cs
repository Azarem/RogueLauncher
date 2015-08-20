using System;
using System.Linq;
using System.Reflection;

namespace AssemblyTranslator.IL
{
    public class ExceptionBlock
    {
        private InstructionBase _beginInstruction;
        private InstructionBase _handlerInstruction;
        private InstructionBase _filterInstruction;
        private InstructionBase _endInstruction;

        public ExceptionHandlingClauseOptions Flags { get; set; }
        public Type CatchType { get; set; }
        public InstructionBase BeginInstruction { get { return _beginInstruction; } set { LinkInstruction(ref _beginInstruction, value); } }
        public InstructionBase HandlerInstruction { get { return _handlerInstruction; } set { LinkInstruction(ref _handlerInstruction, value); } }
        public InstructionBase FilterInstruction { get { return _filterInstruction; } set { LinkInstruction(ref _filterInstruction, value); } }
        public InstructionBase EndInstruction { get { return _endInstruction; } set { LinkInstruction(ref _endInstruction, value); } }

        private void LinkInstruction(ref InstructionBase current, InstructionBase value)
        {
            if (current == value)
                return;

            if (current != null)
                current._exceptionBlocks.Remove(this);

            if (value != null)
                value._exceptionBlocks.Add(this);

            current = value;
        }
    }
}
