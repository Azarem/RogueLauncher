using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyTranslator.Graphs
{
    public class GraphList<TChild, TParent> : IList<TChild>
        where TChild : class, IChildObject<TParent>
        where TParent : class, IParentObject<TChild>
    {
        private List<TChild> _list = new List<TChild>();
        private TParent _parent;

        public GraphList(TParent owner) { _parent = owner; }

        public int IndexOf(TChild item) { return _list.IndexOf(item); }

        public void Insert(int index, TChild item)
        {
            if (item == null)
                return;

            var oldIndex = _list.IndexOf(item);
            if (oldIndex != index)
            {
                if (oldIndex >= 0)
                {
                    _list.RemoveAt(oldIndex);
                    if (oldIndex < index)
                        index--;
                }
                _list.Insert(index, item);
            }

            item.SetParentInternal(_parent);
            //item.DeclaringObject = _parent;
        }

        public void RemoveAt(int index)
        {
            var old = _list[index];
            _list.RemoveAt(index);
            //old.DeclaringObject = null;
            old.SetParentInternal(null);
        }

        public TChild this[int index]
        {
            get { return _list[index]; }
            set
            {
                if (value == null)
                    this.RemoveAt(index);
                else if (_list.Contains(value))
                    this.Insert(index, value);
                else
                {
                    var old = _list[index];
                    //old.DeclaringObject = null;
                    old.SetParentInternal(null);
                    _list[index] = value;
                    value.SetParentInternal(_parent);
                    //value.DeclaringObject = _parent;
                }
            }
        }

        public void Add(TChild item)
        {
            if (item == null || _list.Contains(item))
                return;

            _list.Add(item);
            item.SetParentInternal(_parent);
            //item.DeclaringObject = _parent;
        }

        public void Clear()
        {
            foreach (var e in _list)
                e.SetParentInternal(null);
                //e.DeclaringObject = null;
            _list.Clear();
        }

        public bool Contains(TChild item) { return _list.Contains(item); }

        public void CopyTo(TChild[] array, int arrayIndex) { _list.CopyTo(array, arrayIndex); }

        public int Count { get { return _list.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(TChild item)
        {
            if (item == null)
                return false;

            if (_list.Remove(item))
            {
                //item.DeclaringObject = null;
                item.SetParentInternal(null);
                return true;
            }
            return false;
        }

        public IEnumerator<TChild> GetEnumerator() { return _list.GetEnumerator(); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _list.GetEnumerator(); }
    }
}
