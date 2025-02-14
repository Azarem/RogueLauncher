﻿using AssemblyTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public unsafe class InstructionBase //: IILInstruction
    {
        internal int _lineNumber;
        internal OpCode _opCode;
        internal InstructionList _list;
        protected internal HashSet<InstructionBase> _referencedBy = new HashSet<InstructionBase>();
        protected internal Label? _referenceLabel;
        protected internal HashSet<ExceptionBlock> _exceptionBlocks = new HashSet<ExceptionBlock>();

        public OpCode OpCode { get { return _opCode; } set { _opCode = value; } }
        public ILCode ILCode { get { return (ILCode)_opCode.Value; } }
        public int LineNumber { get { return _lineNumber; } }
        public virtual int OperandSize
        {
            get
            {
                switch (_opCode.OperandType)
                {
                    case OperandType.InlineNone: return 0;
                    case OperandType.ShortInlineI:
                    case OperandType.ShortInlineVar:
                    case OperandType.ShortInlineBrTarget: return 1;
                    case OperandType.InlineVar: return 2;
                    case OperandType.InlineBrTarget:
                    case OperandType.InlineField:
                    case OperandType.InlineI:
                    case OperandType.InlineSig:
                    case OperandType.InlineString:
                    case OperandType.InlineTok:
                    case OperandType.InlineType:
                    case OperandType.ShortInlineR:
                    case OperandType.InlineMethod: return 4;
                    case OperandType.InlineR:
                    case OperandType.InlineI8: return 8;

                    case OperandType.InlineSwitch: return 0;

                    default: throw new InvalidOperationException();
                }
            }
        }

        public virtual object RawOperand
        {
            get
            {
                switch (ILCode)
                {
                    case ILCode.Ldc_I4_M1: return -1;
                    case ILCode.Ldc_I4_0: return 0;
                    case ILCode.Ldc_I4_1: return 1;
                    case ILCode.Ldc_I4_2: return 2;
                    case ILCode.Ldc_I4_3: return 3;
                    case ILCode.Ldc_I4_4: return 4;
                    case ILCode.Ldc_I4_5: return 5;
                    case ILCode.Ldc_I4_6: return 6;
                    case ILCode.Ldc_I4_7: return 7;
                    case ILCode.Ldc_I4_8: return 8;
                }
                return null;
            }
        }

        //internal InstructionBase() { }

        //public InstructionBase(InstructionList list, OpCode opCode, int lineNumber = 0)
        //{
        //    _list = list;
        //    _opCode = opCode;
        //    _lineNumber = lineNumber;
        //}

        protected virtual void ReadOperand(byte[] data, ref int offset) { }
        internal virtual void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode); }
        internal virtual void ParseOperand() { }
        internal virtual void TranslateOperand(GraphManager t, Type[] newGenerics = null) { }
        internal virtual void OptimizeInstruction() { }
        internal unsafe virtual void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            if (_opCode.Size == 2)
                *ptr++ = 0xfe;
            *ptr++ = (byte)_opCode.Value;

            _list.UpdateStackSize(_opCode, Util.StackChange(_opCode));// (int)ILInstructionList._stackChange.Invoke(_opCode, null));
        }

        public virtual void Replace(InstructionBase instr)
        {
            if (_list == null)
                return;

            var ix = _list._instructions.IndexOf(this);

            instr._list = _list;
            if (instr == null)
                _list.RemoveAt(ix);
            else
                _list[ix] = instr;

            foreach (var e in _exceptionBlocks)
            {
                if (e.BeginInstruction == this)
                    e.BeginInstruction = instr;
                if (e.HandlerInstruction == this)
                    e.HandlerInstruction = instr;
                if (e.FilterInstruction == this)
                    e.FilterInstruction = instr;
                if (e.EndInstruction == this)
                    e.EndInstruction = instr;
            }
            _exceptionBlocks.Clear();

            foreach (var l in _referencedBy)
            {
                if (l is SwitchInstruction)
                    ((SwitchInstruction)l).ReplaceAll(this, instr);
                else if (l is LabelInstruction)
                    ((LabelInstruction)l).JumpToInstruction = instr;
                else
                    throw new InvalidOperationException();
            }
            _referencedBy.Clear();

            _list = null;
        }

        internal virtual bool OptimizeLineNumber(ref int index)
        {
            int oldLine = _lineNumber;
            _lineNumber = index;
            index += _opCode.Size + OperandSize;
            return oldLine != _lineNumber;
        }

        public static InstructionBase Parse(byte[] data, ref int offset, InstructionList list)
        {
            int line = offset;
            int op = data[offset++];
            if (op == 0xfe)
                op = 0xfe00 | data[offset++];

            InstructionBase inst = null;

            var opCode = OpCodeList.ByValue[op];

            switch (opCode.OperandType)
            {
                case OperandType.ShortInlineBrTarget:
                case OperandType.InlineBrTarget: inst = new LabelInstruction(); break;
                case OperandType.InlineField: inst = new FieldInstruction(); break;
                case OperandType.InlineI: inst = new Int32Instruction(); break;
                case OperandType.InlineI8: inst = new Int64Instruction(); break;
                case OperandType.InlineMethod: inst = new MethodInstruction(); break;
                case OperandType.InlineNone:
                    switch ((ILCode)op)
                    {
                        case ILCode.Ldarg_0:
                        case ILCode.Ldarg_1:
                        case ILCode.Ldarg_2:
                        case ILCode.Ldarg_3:
                            inst = new ParameterInstruction();
                            break;

                        case ILCode.Stloc_0:
                        case ILCode.Ldloc_0:
                        case ILCode.Stloc_1:
                        case ILCode.Ldloc_1:
                        case ILCode.Stloc_2:
                        case ILCode.Ldloc_2:
                        case ILCode.Stloc_3:
                        case ILCode.Ldloc_3:
                            inst = new LocalInstruction();
                            break;

                        case ILCode.Ldc_I4_0:
                        case ILCode.Ldc_I4_1:
                        case ILCode.Ldc_I4_2:
                        case ILCode.Ldc_I4_3:
                        case ILCode.Ldc_I4_4:
                        case ILCode.Ldc_I4_5:
                        case ILCode.Ldc_I4_6:
                        case ILCode.Ldc_I4_7:
                        case ILCode.Ldc_I4_8:
                        case ILCode.Ldc_I4_M1:
                            inst = new Int32Instruction();
                            break;

                        default:
                            inst = new InstructionBase();
                            break;
                    } break;
                case OperandType.InlineR: inst = new DoubleInstruction(); break;
                case OperandType.InlineSig: inst = new SignatureInstruction(); break;
                case OperandType.InlineString: inst = new StringInstruction(); break;
                case OperandType.InlineSwitch: inst = new SwitchInstruction(); break;
                case OperandType.InlineTok: inst = new MemberInstruction(); break;
                case OperandType.InlineType: inst = new TypeInstruction(); break;
                case OperandType.InlineVar:
                    inst = (opCode == OpCodes.Ldarg || opCode == OpCodes.Ldarga || opCode == OpCodes.Starg) ? (InstructionBase)new ParameterInstruction() : new LocalInstruction();
                    break;
                case OperandType.ShortInlineVar:
                    inst = (opCode == OpCodes.Ldarga_S || opCode == OpCodes.Ldarg_S || opCode == OpCodes.Starg_S) ? (InstructionBase)new ParameterInstruction() : new LocalInstruction();
                    break;
                case OperandType.ShortInlineI: inst = new Int8Instruction(); break;
                case OperandType.ShortInlineR: inst = new SingleInstruction(); break;
                default: throw new InvalidOperationException();
            }

            inst._lineNumber = line;
            inst._opCode = opCode;
            inst._list = list;

            inst.ReadOperand(data, ref offset);

            return inst;
        }

        public override string ToString()
        {
            return _opCode.ToString();
        }
    }

    public abstract class Instruction<T> : InstructionBase
    {
        protected T _operand;
        public T Operand { get { return _operand; } set { _operand = value; } }

        public override object RawOperand { get { return _operand; } }
    }
}
