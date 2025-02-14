﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace AssemblyTranslator.IL
{
    public class ParameterInstruction : Instruction<ushort>
    {
        protected override void ReadOperand(byte[] data, ref int offset)
        {
            if (_opCode.OperandType == OperandType.ShortInlineVar)
                _operand = data[offset++];
            else if (_opCode.OperandType == OperandType.InlineVar)
            {
                _operand = BitConverter.ToUInt16(data, offset);
                offset += 2;
            }
            else
                switch (ILCode)
                {
                    case ILCode.Ldarg_0: _operand = 0; break;
                    case ILCode.Ldarg_1: _operand = 1; break;
                    case ILCode.Ldarg_2: _operand = 2; break;
                    case ILCode.Ldarg_3: _operand = 3; break;
                    default:
                        throw new InvalidOperationException();
                }
        }

        internal override void OptimizeInstruction()
        {
            var type = _operand < 4 ? _operand : _operand <= byte.MaxValue ? 4 : 5;
            switch (ILCode)
            {
                case ILCode.Starg: if (type != 5) _opCode = OpCodes.Starg_S; break;
                case ILCode.Starg_S: if (type == 5) _opCode = OpCodes.Starg; break;
                case ILCode.Ldarga: if (type != 5) _opCode = OpCodes.Ldarga_S; break;
                case ILCode.Ldarga_S: if (type == 5) _opCode = OpCodes.Ldarga; break;

                case ILCode.Ldarg_0:
                case ILCode.Ldarg_1:
                case ILCode.Ldarg_2:
                case ILCode.Ldarg_3:
                case ILCode.Ldarg_S:
                case ILCode.Ldarg:
                    switch (type)
                    {
                        case 0: _opCode = OpCodes.Ldarg_0; break;
                        case 1: _opCode = OpCodes.Ldarg_1; break;
                        case 2: _opCode = OpCodes.Ldarg_2; break;
                        case 3: _opCode = OpCodes.Ldarg_3; break;
                        case 4: _opCode = OpCodes.Ldarg_S; break;
                        default: _opCode = OpCodes.Ldarg; break;
                    } break;

                default: throw new InvalidOperationException();
            }
        }

        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }

        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);

            if (_opCode.OperandType == OperandType.ShortInlineVar)
                *ptr++ = (byte)_operand;
            else if (_opCode.OperandType == OperandType.InlineVar)
            {
                *(ushort*)ptr = _operand;
                ptr += 2;
            }
        }

    }
}
