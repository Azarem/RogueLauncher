using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class SignatureInstruction : TokenInstruction<SignatureHelper>
    {
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }

        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            _token = _list._moduleBuilder.GetSignatureToken(_operand).Token;
            base.WriteILBytes(ref ptr, fixups);

            if (_opCode.StackBehaviourPop == StackBehaviour.Varpop)
                _list.UpdateStackSize(_opCode, -1 - Util.ArgumentCount(_operand));// (int)ILInstructionList._argumentCount.GetValue(_operand));
        }
    }
}
