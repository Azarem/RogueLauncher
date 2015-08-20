using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class TypeInstruction : TokenInstruction<Type>
    {
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }

        //internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        //{
        //    _token = (_opCode != OpCodes.Ldtoken || _operand == null || !_operand.IsGenericTypeDefinition
        //        ? ((TypeToken)ILInstructionList._getTypeTokenInternal.Invoke(_list._moduleBuilder, new object[] { _operand })).Token
        //        : _list._moduleBuilder.GetTypeToken(_operand).Token);
        //    base.WriteILBytes(ref ptr, fixups);
        //}
    }
}
