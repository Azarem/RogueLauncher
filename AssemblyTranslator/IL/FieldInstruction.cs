using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.IL
{
    public class FieldInstruction : TokenInstruction<FieldInfo>
    {
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
    }
}
