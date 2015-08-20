using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class LocalInstruction : Instruction<LocalBuilder>
    {
        private ushort _index;
        private ILLocal _localVar;
        //public ILLocalInstruction(byte[] data, ref int offset) : base(data, ref offset) { }
        protected override void ReadOperand(byte[] data, ref int offset)
        {
            if (_opCode.OperandType == OperandType.ShortInlineVar)
                _index = data[offset++];
            else
            {
                _index = BitConverter.ToUInt16(data, offset);
                offset += 2;
            }
        }
        internal override void ParseOperand() { _localVar = _list._locals[_index]; }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }

        internal override void OptimizeInstruction()
        {
            _index = (ushort)_list._locals.IndexOf(_localVar);
            _operand = _list._locals[_index].LocalBuilder;
            var type = _index < 4 ? _index : _index < 0x100 ? 4 : 5;

            switch ((ILCode)_opCode.Value)
            {
                case ILCode.Ldloca: if (type != 5) _opCode = OpCodes.Ldloca_S; break;
                case ILCode.Ldloca_S: if (type == 5) _opCode = OpCodes.Ldloca; break;

                case ILCode.Ldloc_0:
                case ILCode.Ldloc_1:
                case ILCode.Ldloc_2:
                case ILCode.Ldloc_3:
                case ILCode.Ldloc_S:
                case ILCode.Ldloc:
                    switch (type)
                    {
                        case 0: _opCode = OpCodes.Ldloc_0; break;
                        case 1: _opCode = OpCodes.Ldloc_1; break;
                        case 2: _opCode = OpCodes.Ldloc_2; break;
                        case 3: _opCode = OpCodes.Ldloc_3; break;
                        case 4: _opCode = OpCodes.Ldloc_S; break;
                        default: _opCode = OpCodes.Ldloc; break;
                    } break;

                case ILCode.Stloc_0:
                case ILCode.Stloc_1:
                case ILCode.Stloc_2:
                case ILCode.Stloc_3:
                case ILCode.Stloc_S:
                case ILCode.Stloc:
                    switch (type)
                    {
                        case 0: _opCode = OpCodes.Stloc_0; break;
                        case 1: _opCode = OpCodes.Stloc_1; break;
                        case 2: _opCode = OpCodes.Stloc_2; break;
                        case 3: _opCode = OpCodes.Stloc_3; break;
                        case 4: _opCode = OpCodes.Stloc_S; break;
                        default: _opCode = OpCodes.Stloc; break;
                    } break;

                default: throw new InvalidOperationException();
            }

            base.OptimizeInstruction();
        }

        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);

            if (_opCode.OperandType == OperandType.ShortInlineVar)
                *ptr++ = (byte)_index;
            else if (_opCode.OperandType != OperandType.InlineNone)
            {
                *(ushort*)ptr = _index;
                ptr += 2;
            }
        }
    }
}
