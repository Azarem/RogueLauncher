using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class ILLocal
    {
        public Type Type { get; set; }
        public bool IsPinned { get; set; }
        public string Label { get; set; }
        public LocalBuilder LocalBuilder { get; set; }
    }
}
