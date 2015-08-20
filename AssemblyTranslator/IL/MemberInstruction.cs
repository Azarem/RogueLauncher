using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class MemberInstruction : TokenInstruction<MemberInfo>
    {
        internal override void EmitInstruction(ILGenerator generator)
        {
            if (_operand is MethodInfo)
                generator.Emit(_opCode, (MethodInfo)_operand);
            else if (Operand is FieldInfo)
                generator.Emit(_opCode, (FieldInfo)_operand);
            else if (Operand is Type)
                generator.Emit(_opCode, (Type)_operand);
            else
                throw new InvalidOperationException();
        }
    }
}
