using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class LabelInstruction : Instruction<Label>
    {
        private int _offset;
        protected InstructionBase _jumpTo;
        public InstructionBase JumpToInstruction
        {
            get { return _jumpTo; }
            set
            {
                if (_jumpTo == value)
                    return;

                if (_jumpTo != null)
                    _jumpTo._referencedBy.Remove(this);

                if (value != null)
                    value._referencedBy.Add(this);

                _jumpTo = value;
            }
        }

        //public ILLabelInstruction(byte[] data, ref int offset) : base(data, ref offset) { }
        protected override void ReadOperand(byte[] data, ref int offset)
        {
            if (_opCode.OperandType == OperandType.ShortInlineBrTarget)
                _offset = (sbyte)data[offset++] + offset;
            else
                _offset = BitConverter.ToInt32(data, offset) + (offset += 4);
        }
        internal override void ParseOperand() { JumpToInstruction = _list._instructions.First(x => x.LineNumber == _offset); }
        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }

        internal override void OptimizeInstruction()
        {
            //_operand = _jumpTo._referenceLabel.Value; 
        }
        internal override bool OptimizeLineNumber(ref int index)
        {
            var oldLine = _lineNumber;
            _lineNumber = index;
            var size = _opCode.Size + OperandSize;
            //var potential = size == 5 ? 3 : -3;

            //bool changed = base.SetLineNumber(ref index);

            var offset = _jumpTo._lineNumber - (index + size);
            if (offset >= sbyte.MinValue && offset <= sbyte.MaxValue)
                switch ((ILCode)_opCode.Value)
                {
                    case ILCode.Beq: _opCode = OpCodes.Beq_S; break;
                    case ILCode.Bge: _opCode = OpCodes.Bge_S; break;
                    case ILCode.Bge_Un: _opCode = OpCodes.Bge_Un_S; break;
                    case ILCode.Bgt: _opCode = OpCodes.Bgt_S; break;
                    case ILCode.Bgt_Un: _opCode = OpCodes.Bgt_Un_S; break;
                    case ILCode.Ble: _opCode = OpCodes.Ble_S; break;
                    case ILCode.Ble_Un: _opCode = OpCodes.Ble_Un_S; break;
                    case ILCode.Blt: _opCode = OpCodes.Blt_S; break;
                    case ILCode.Blt_Un: _opCode = OpCodes.Blt_Un_S; break;
                    case ILCode.Bne_Un: _opCode = OpCodes.Bne_Un_S; break;
                    case ILCode.Br: _opCode = OpCodes.Br_S; break;
                    case ILCode.Brfalse: _opCode = OpCodes.Brfalse_S; break;
                    case ILCode.Brtrue: _opCode = OpCodes.Brtrue_S; break;
                    case ILCode.Leave: _opCode = OpCodes.Leave_S; break;
                }
            else
                switch ((ILCode)_opCode.Value)
                {
                    case ILCode.Beq_S: _opCode = OpCodes.Beq; break;
                    case ILCode.Bge_S: _opCode = OpCodes.Bge; break;
                    case ILCode.Bge_Un_S: _opCode = OpCodes.Bge_Un; break;
                    case ILCode.Bgt_S: _opCode = OpCodes.Bgt; break;
                    case ILCode.Bgt_Un_S: _opCode = OpCodes.Bgt_Un; break;
                    case ILCode.Ble_S: _opCode = OpCodes.Ble; break;
                    case ILCode.Ble_Un_S: _opCode = OpCodes.Ble_Un; break;
                    case ILCode.Blt_S: _opCode = OpCodes.Blt; break;
                    case ILCode.Blt_Un_S: _opCode = OpCodes.Blt_Un; break;
                    case ILCode.Bne_Un_S: _opCode = OpCodes.Bne_Un; break;
                    case ILCode.Br_S: _opCode = OpCodes.Br; break;
                    case ILCode.Brfalse_S: _opCode = OpCodes.Brfalse; break;
                    case ILCode.Brtrue_S: _opCode = OpCodes.Brtrue; break;
                    case ILCode.Leave_S: _opCode = OpCodes.Leave; break;
                }

            var newSize = _opCode.Size + OperandSize;

            index += newSize;

            _offset = size - newSize + offset;

            return oldLine != _lineNumber;// || size != newSize;
        }

        internal unsafe override void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);

            if (_opCode.OperandType == OperandType.ShortInlineBrTarget)
                *ptr++ = (byte)_offset;
            else
            {
                *(int*)ptr = _offset;
                ptr += 4;
            }
        }
    }
}
