using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class StringInstruction : TokenInstruction<string>
    {
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }

        //internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        //{
        //    _token = _list._moduleBuilder.GetStringConstant(_operand).Token;
        //    base.WriteILBytes(ref ptr, fixups);
        //}
    }
}
