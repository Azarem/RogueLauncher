using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class RewriteAttribute : Attribute
    {
        internal readonly RewriteAction action;
        internal readonly string typeName, targetName, newName, oldName;
        internal readonly StubAction stubAction;
        internal readonly string contentHandler;

        public RewriteAttribute(string typeName, string targetName = null, RewriteAction action = RewriteAction.None, string newName = null, string oldName = null, StubAction stubAction = StubAction.UseOld, string contentHandler = null)
        {
            this.typeName = typeName;
            this.targetName = targetName;
            this.action = action;
            this.newName = newName;
            this.oldName = oldName;
            this.stubAction = stubAction;
            this.contentHandler = contentHandler;
        }
    }

}
