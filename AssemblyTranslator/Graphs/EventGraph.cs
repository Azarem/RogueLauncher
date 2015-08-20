using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator.Graphs
{
    public class EventGraph : MemberGraph<EventInfo, EventBuilder, EventAttributes, TypeGraph, EventGraph>
    {
        protected MethodGraph _addMethod;
        protected MethodGraph _raiseMethod;
        protected MethodGraph _removeMethod;
        protected Type _eventHandlerType;

        public MethodGraph AddMethod { get { return _addMethod; } set { _addMethod = value; } }
        public MethodGraph RaiseMethod { get { return _raiseMethod; } set { _raiseMethod = value; } }
        public MethodGraph RemoveMethod { get { return _removeMethod; } set { _removeMethod = value; } }
        public Type EventHandlerType { get { return _eventHandlerType; } set { _eventHandlerType = value; } }

        public EventGraph() : this(null) { }
        public EventGraph(EventInfo eventInfo, TypeGraph parentType = null)
            : base(eventInfo, parentType)
        {
            if (eventInfo != null)
            {
                _attributes = eventInfo.Attributes;
                _eventHandlerType = eventInfo.EventHandlerType;

                MethodInfo oldMethod;

                if ((oldMethod = eventInfo.GetAddMethod(true)) != null)
                    _addMethod = parentType.Methods.Single(x => x.Source == oldMethod);

                if ((oldMethod = eventInfo.GetRaiseMethod(true)) != null)
                    _raiseMethod = parentType.Methods.Single(x => x.Source == oldMethod);

                if ((oldMethod = eventInfo.GetRemoveMethod(true)) != null)
                    _removeMethod = parentType.Methods.Single(x => x.Source == oldMethod);

            }
        }

        internal void DeclareMember(GraphManager translator)
        {
            _builder = _parentObject.Builder.DefineEvent(_name, _attributes, translator.GetType(_eventHandlerType));

            //translator.SetEvent(_sourceObject ?? _builder, _builder);
        }

        internal void DefineMember(GraphManager translator)
        {
            if (_addMethod != null)
                _builder.SetAddOnMethod(_addMethod.Builder);

            if (_raiseMethod != null)
                _builder.SetRaiseMethod(_raiseMethod.Builder);

            if (_removeMethod != null)
                _builder.SetRemoveOnMethod(_removeMethod.Builder);

            foreach (var att in _customAttributes)
                _builder.SetCustomAttribute(att.CreateBuilder(translator));
        }

    }
}
