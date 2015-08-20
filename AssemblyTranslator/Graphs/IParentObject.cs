using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyTranslator.Graphs
{
    public interface IParentObject<TChild> where TChild : class
    {
        IList<TChild> ChildList { get; }
    }
}
