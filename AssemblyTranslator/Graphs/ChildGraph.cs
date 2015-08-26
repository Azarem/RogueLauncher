using System;
using System.Linq;

namespace AssemblyTranslator.Graphs
{
    public abstract class ChildGraph<TSource, TBuilder, TParent, TSelf> : ObjectGraph<TSource, TBuilder>, IChildObject<TParent>
        where TSource : class
        where TBuilder : class
        where TSelf : class, IChildObject<TParent>
        where TParent : class, IParentObject<TSelf>
    {
        internal TParent _parentObject;

        public virtual TParent DeclaringObject
        {
            get { return _parentObject; }
            set
            {
                if (_parentObject == value)
                    return;
                if (_parentObject != null)
                    _parentObject.ChildList.Remove(this as TSelf);
                if (value != null)
                    value.ChildList.Add(this as TSelf);
                _parentObject = value;
            }
        }

        public ChildGraph() : this(null) { }
        public ChildGraph(TSource source, TParent parent = null)
            : base(source)
        {
            if (parent != null)
                DeclaringObject = parent;
        }

        public virtual void SetParentInternal(TParent parent) { _parentObject = parent; }
    }
}
