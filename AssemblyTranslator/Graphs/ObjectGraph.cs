﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssemblyTranslator.Graphs
{
    public abstract class ObjectGraph<TSource, TBuilder>//, TParent> : IChildObject<TParent>
        where TSource : class
        where TBuilder : class
        //where TParent : class
    {
        //protected AssemblyTranslator _translator;
        protected TSource _sourceObject;
        protected TBuilder _builder;
        protected List<CustomAttributeGraph> _customAttributes = new List<CustomAttributeGraph>();

        //public AssemblyTranslator Translator { get { return _translator; } }
        public TSource Source { get { return _sourceObject; } }
        public TBuilder Builder { get { return _builder; } }

        public List<CustomAttributeGraph> CustomAttributes { get { return _customAttributes; } }

        public ObjectGraph() :this(null) { }
        public ObjectGraph(TSource source) { _sourceObject = source; }



        protected void SetAttributeData(IEnumerable<CustomAttributeData> attributes)
        {
            _customAttributes.AddRange(attributes.Select(x => new CustomAttributeGraph(x)));
        }


    }
}