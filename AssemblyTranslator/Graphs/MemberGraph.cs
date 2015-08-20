using System;
using System.Linq;
using System.Reflection;

namespace AssemblyTranslator.Graphs
{
    public abstract class MemberGraph<TMember, TBuilder, TAttributes, TParent, TSelf> : ChildGraph<TMember, TBuilder, TParent, TSelf>
        where TMember : MemberInfo
        where TBuilder : class
        where TParent : class, IParentObject<TSelf>
        where TSelf : class, IChildObject<TParent>
    //where TSelf : MemberGraph<TMember, TBuilder, TAttributes, TSelf>
    {
        protected string _name;
        protected TAttributes _attributes;

        public virtual string Name { get { return _name; } set { _name = value; } }
        public TAttributes Attributes { get { return _attributes; } set { _attributes = value; } }

        public MemberGraph() : this(null) { }
        public MemberGraph(TMember member, TParent parent = null)
            : base(member, parent)
        {
            if (member != null)
            {
                _name = member.Name;

                SetAttributeData(member.GetCustomAttributesData());
            }
        }

        //public MemberGraph(AssemblyTranslator translator, Type type)
        //{
        //    _translator = translator;
        //    _declaringType = type;
        //}

        public override string ToString() { return _name; }
    }
}
