using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class UInt8Instruction : Instruction<byte>
    {
        protected override void ReadOperand(byte[] data, ref int offset) { _operand = data[offset++]; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            *ptr++ = _operand;
        }
    }

    public class Int8Instruction : Instruction<sbyte>
    {
        protected override void ReadOperand(byte[] data, ref int offset) { _operand = (sbyte)data[offset++]; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            *ptr++ = (byte)_operand;
        }
    }

    public class Int16Instruction : Instruction<short>
    {
        protected override void ReadOperand(byte[] data, ref int offset) { _operand = BitConverter.ToInt16(data, offset); offset += 2; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            fixed (short* l = &_operand)
            {
                *(short*)ptr = *l;
                ptr += 2;
            }
        }
    }

    public class Int32Instruction : Instruction<int>
    {
        protected override void ReadOperand(byte[] data, ref int offset)
        {
            if (_opCode.OperandType == OperandType.InlineI)
            {
                _operand = BitConverter.ToInt32(data, offset);
                offset += 4;
            }
            else
                switch (ILCode)
                {
                    case ILCode.Ldc_I4_M1: _operand = -1; break;
                    case ILCode.Ldc_I4_0: _operand = 0; break;
                    case ILCode.Ldc_I4_1: _operand = 1; break;
                    case ILCode.Ldc_I4_2: _operand = 2; break;
                    case ILCode.Ldc_I4_3: _operand = 3; break;
                    case ILCode.Ldc_I4_4: _operand = 4; break;
                    case ILCode.Ldc_I4_5: _operand = 5; break;
                    case ILCode.Ldc_I4_6: _operand = 6; break;
                    case ILCode.Ldc_I4_7: _operand = 7; break;
                    case ILCode.Ldc_I4_8: _operand = 8; break;
                    default:
                        throw new InvalidOperationException();
                }
        }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            if (_opCode.OperandType == OperandType.InlineI)
                fixed (int* l = &_operand)
                {
                    *(int*)ptr = *l;
                    ptr += 4;
                }
        }

        internal override void OptimizeInstruction()
        {
            switch (ILCode)
            {
                case ILCode.Ldc_I4:
                case ILCode.Ldc_I4_S:
                case ILCode.Ldc_I4_M1:
                case ILCode.Ldc_I4_0:
                case ILCode.Ldc_I4_1:
                case ILCode.Ldc_I4_2:
                case ILCode.Ldc_I4_3:
                case ILCode.Ldc_I4_4:
                case ILCode.Ldc_I4_5:
                case ILCode.Ldc_I4_6:
                case ILCode.Ldc_I4_7:
                case ILCode.Ldc_I4_8:
                    switch (_operand)
                    {
                        case -1: _opCode = OpCodes.Ldc_I4_M1; break;
                        case 0: _opCode = OpCodes.Ldc_I4_0; break;
                        case 1: _opCode = OpCodes.Ldc_I4_1; break;
                        case 2: _opCode = OpCodes.Ldc_I4_2; break;
                        case 3: _opCode = OpCodes.Ldc_I4_3; break;
                        case 4: _opCode = OpCodes.Ldc_I4_4; break;
                        case 5: _opCode = OpCodes.Ldc_I4_5; break;
                        case 6: _opCode = OpCodes.Ldc_I4_6; break;
                        case 7: _opCode = OpCodes.Ldc_I4_7; break;
                        case 8: _opCode = OpCodes.Ldc_I4_8; break;
                        default: _opCode = _operand > sbyte.MaxValue || _operand < sbyte.MinValue ? OpCodes.Ldc_I4 : OpCodes.Ldc_I4_S; break;
                    } break;
            }
        }
    }

    public class Int64Instruction : Instruction<long>
    {
        protected override void ReadOperand(byte[] data, ref int offset) { _operand = BitConverter.ToInt64(data, offset); offset += 8; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            fixed (long* l = &_operand)
            {
                *(long*)ptr = *l;
                ptr += 8;
            }
        }
    }

    public class SingleInstruction : Instruction<float>
    {
        protected override void ReadOperand(byte[] data, ref int offset) { _operand = BitConverter.ToSingle(data, offset); offset += 4; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            fixed (float* l = &_operand)
            {
                *(float*)ptr = *l;
                ptr += 4;
            }
        }
    }

    public class DoubleInstruction : Instruction<double>
    {
        protected override void ReadOperand(byte[] data, ref int offset) { _operand = BitConverter.ToDouble(data, offset); offset += 8; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            fixed (double* l = &_operand)
            {
                *(double*)ptr = *l;
                ptr += 8;
            }
        }
    }
}
