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
        protected override void ReadOperand(byte[] data, ref int offset) { _operand = BitConverter.ToInt32(data, offset); offset += 4; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);
            fixed (int* l = &_operand)
            {
                *(int*)ptr = *l;
                ptr += 4;
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
