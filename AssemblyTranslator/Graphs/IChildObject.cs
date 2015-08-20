using System;
using System.Linq;

namespace AssemblyTranslator.Graphs
{
    public interface IChildObject<TParent> where TParent : class
    {
        void SetParentInternal(TParent parent);
    }
}
