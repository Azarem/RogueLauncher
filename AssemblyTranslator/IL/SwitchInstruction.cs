using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class SwitchInstruction : Instruction<Label[]>, IList<InstructionBase>
    {
        internal int[] _offsetList;
        internal List<InstructionBase> _jumpToInstructions = new List<InstructionBase>();

        public override int OperandSize { get { return _jumpToInstructions.Count * 4 + 4; } }

        //public ILSwitchInstruction(byte[] data, ref int offset) : base(data, ref offset) { }
        protected override void ReadOperand(byte[] data, ref int offset)
        {
            int count = BitConverter.ToInt32(data, offset);
            offset += 4;

            _offsetList = new int[count];

            for (int ix = 0, end = offset + count * 4; ix < count; ix++, offset += 4)
                _offsetList[ix] = end + BitConverter.ToInt32(data, offset);
        }
        internal override void ParseOperand()
        {
            foreach (var i in _offsetList)
                Add(_list._instructions.First(x => x.LineNumber == i));
        }
        internal override void OptimizeInstruction()
        {
            //_operand = _jumpToInstructions.Select(x => x._referenceLabel.Value).ToArray(); 
        }

        internal override void EmitInstruction(ILGenerator generator) { generator.Emit(_opCode, _operand); }

        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            base.WriteILBytes(ref ptr, fixups);

            var count = _jumpToInstructions.Count;
            *(int*)ptr = count;
            ptr += 4;

            var end = count * 4 + _lineNumber + _opCode.Size + 4;
            foreach (var i in _jumpToInstructions)
            {
                *(int*)ptr = i._lineNumber - end;
                ptr += 4;
            }
        }

        public void Add(InstructionBase item)
        {
            if (item != null)
            {
                _jumpToInstructions.Add(item);
                item._referencedBy.Add(this);
            }
        }

        public void Clear()
        {
            foreach (var i in _jumpToInstructions)
                i._referencedBy.Remove(this);
            _jumpToInstructions.Clear();
        }

        public bool Contains(InstructionBase item) { return _jumpToInstructions.Contains(item); }
        public void CopyTo(InstructionBase[] array, int arrayIndex) { _jumpToInstructions.CopyTo(array, arrayIndex); }
        public int Count { get { return _jumpToInstructions.Count; } }
        public bool IsReadOnly { get { return false; } }

        public bool Remove(InstructionBase item)
        {
            if (item == null)
                return false;

            if (_jumpToInstructions.Remove(item))
            {
                if (!_jumpToInstructions.Contains(item))
                    item._referencedBy.Remove(this);
                return true;
            }
            return false;
        }

        public IEnumerator<InstructionBase> GetEnumerator() { return _jumpToInstructions.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _jumpToInstructions.GetEnumerator(); }

        public int IndexOf(InstructionBase item) { return _jumpToInstructions.IndexOf(item); }
        public void Insert(int index, InstructionBase item)
        {
            if (item != null)
            {
                _jumpToInstructions.Insert(index, item);
                item._referencedBy.Add(this);
            }
        }

        public void RemoveAt(int index)
        {
            var item = _jumpToInstructions[index];

            _jumpToInstructions.RemoveAt(index);

            if (!_jumpToInstructions.Contains(item))
                item._referencedBy.Remove(this);
        }

        public InstructionBase this[int index]
        {
            get
            {
                return _jumpToInstructions[index];
            }
            set
            {
                if (value == null)
                    RemoveAt(index);
                else
                {
                    var item = _jumpToInstructions[index];

                    _jumpToInstructions[index] = value;
                    value._referencedBy.Add(this);

                    if (!_jumpToInstructions.Contains(item))
                        item._referencedBy.Remove(this);
                }
            }
        }

        public void ReplaceAll(InstructionBase item, InstructionBase newItem)
        {
            int index;
            while ((index = _jumpToInstructions.IndexOf(item)) >= 0)
                this[index] = newItem;
        }
    }
}
