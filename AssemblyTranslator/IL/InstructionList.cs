﻿using AssemblyTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public unsafe class InstructionList : IList<InstructionBase>
    {
        internal List<InstructionBase> _instructions = new List<InstructionBase>();
        internal Module _module;
        internal Type _type;
        internal MethodBase _method;
        internal List<ILLocal> _locals = new List<ILLocal>();
        internal List<ExceptionBlock> _exceptionBlocks = new List<ExceptionBlock>();
        internal Type[] _typeGenerics, _methodGenerics;
        internal ModuleBuilder _moduleBuilder;
        internal byte[] _oldData;

        public InstructionList(MethodBase method)
        {
            var body = method.GetMethodBody();
            if (body == null)
                return;

            //Set fields
            _method = method;
            _type = method.DeclaringType;
            _module = method.Module;

            //Get generic arguments for symbol lookup
            _typeGenerics = _type.IsGenericType ? _type.GetGenericArguments() : null;
            _methodGenerics = _method.IsGenericMethod ? _method.GetGenericArguments() : null;

            //Create locals
            foreach (var l in body.LocalVariables)
                _locals.Add(new ILLocal() { Type = l.LocalType, IsPinned = l.IsPinned, Label = "V_" + l.LocalIndex });

            //Parse instructions
            var data = _oldData = body.GetILAsByteArray();
            int index = 0;
            while (index < data.Length)
            {
                var instr = InstructionBase.Parse(data, ref index, this);

                _instructions.Add(instr);
            }

            //Parse exception blocks
            _exceptionBlocks.AddRange(body.ExceptionHandlingClauses.Select(ex => new ExceptionBlock()
            {
                Flags = ex.Flags,
                CatchType = ex.Flags == ExceptionHandlingClauseOptions.Clause ? ex.CatchType : null,
                BeginInstruction = _instructions.First(x => x.LineNumber == ex.TryOffset),
                HandlerInstruction = _instructions.First(x => x.LineNumber == ex.HandlerOffset),
                FilterInstruction = ex.Flags == ExceptionHandlingClauseOptions.Filter ? _instructions.First(x => x.LineNumber == ex.FilterOffset) : null,
                EndInstruction = _instructions.First(x => x.LineNumber == ex.HandlerOffset + ex.HandlerLength)
            }));

            //Create label links
            foreach (var i in _instructions)
                i.ParseOperand();
        }

        public MethodBase ResolveMethod(int token) { return _module.ResolveMethod(token, _typeGenerics, _methodGenerics); }
        public FieldInfo ResolveField(int token) { return _module.ResolveField(token, _typeGenerics, _methodGenerics); }
        public Type ResolveType(int token) { return _module.ResolveType(token, _typeGenerics, _methodGenerics); }
        public string ResolveString(int token) { return _module.ResolveString(token); }
        public MemberInfo ResolveMember(int token) { return _module.ResolveMember(token, _typeGenerics, _methodGenerics); }
        public byte[] ResolveSignature(int token) { return _module.ResolveSignature(token); }
        public T ResolveToken<T>(int token) where T : class
        {
            if (typeof(T) == typeof(MethodBase))
                return ResolveMethod(token) as T;
            if (typeof(T) == typeof(FieldInfo))
                return ResolveField(token) as T;
            if (typeof(T) == typeof(Type))
                return ResolveType(token) as T;
            if (typeof(T) == typeof(Type))
                return ResolveType(token) as T;
            if (typeof(T) == typeof(byte[]))
                return ResolveSignature(token) as T;
            if (typeof(T) == typeof(string))
                return ResolveString(token) as T;
            return ResolveMember(token) as T;
        }

        public void TranslateTypes(GraphManager t, Type[] newGenerics = null)
        {
            if (_method == null)
                return;

            //Translate local variables
            foreach (var l in _locals)
                l.Type = t.GetType(l.Type, newGenerics);

            //Translate exception types
            foreach (var e in _exceptionBlocks)
                e.CatchType = t.GetType(e.CatchType, newGenerics);

            //Translate instruction types
            foreach (var i in _instructions)
                i.TranslateOperand(t, newGenerics);
        }

        public void Rebuild(MethodBuilder builder)
        {
            if (_method == null)
                return;

            if (_type.Name == "DemoEndScreen" && _method.Name == "OnEnter")
            {

            }

            _moduleBuilder = (ModuleBuilder)builder.Module;

            //var generator = builder.GetILGenerator();

            ////Create locals
            //foreach (var l in _locals)
            //    l.LocalBuilder = generator.DeclareLocal(l.Type, l.IsPinned);

            ////Create labels
            //foreach (var i in _instructions.Where(x => x._referencedBy.Count > 0))
            //    i._referenceLabel = generator.DefineLabel();

            //Prepare / optimize instructions
            foreach (var i in _instructions)
                i.OptimizeInstruction();

            //Optimize jumps
            bool changed = true;
            while (changed)
            {
                changed = false;
                int lineNumber = 0;
                foreach (var i in _instructions)
                    changed |= i.OptimizeLineNumber(ref lineNumber);
            }


            var last = _instructions[_instructions.Count - 1];
            var length = last._lineNumber + last._opCode.Size + last.OperandSize;
            byte[] data = new byte[length];
            var fixups = new List<int>();
            fixed (byte* ptr = data)
            {
                byte* p = ptr;

                int exceptionDepth = 0;

                //Create code
                foreach (var i in _instructions)
                {
                    //if (i._referenceLabel != null)
                    //    generator.MarkLabel(i._referenceLabel.Value);

                    //foreach (var e in i._exceptionBlocks)
                    //{
                    //    if (e.EndInstruction == i)
                    //    {
                    //        generator.EndExceptionBlock();
                    //        exceptionDepth--;
                    //    }
                    //    if (e.BeginInstruction == i)
                    //    {
                    //        generator.BeginExceptionBlock();
                    //        exceptionDepth++;
                    //    }
                    //    if (e.HandlerInstruction == i)
                    //        if (e.Flags == ExceptionHandlingClauseOptions.Clause)
                    //            generator.BeginCatchBlock(e.CatchType);
                    //        else if (e.Flags == ExceptionHandlingClauseOptions.Fault)
                    //            generator.BeginFaultBlock();
                    //        else if (e.Flags == ExceptionHandlingClauseOptions.Finally)
                    //            generator.BeginFinallyBlock();
                    //        else
                    //            throw new NotSupportedException();
                    //    else if (e.FilterInstruction == i)
                    //        generator.BeginExceptFilterBlock();
                    //}

                    //i.EmitInstruction(generator);

                    i.WriteILBytes(ref p, fixups);
                }

            }

            SignatureHelper localSig = SignatureHelper.GetLocalVarSigHelper(_moduleBuilder);
            foreach (var l in _locals)
                localSig.AddArgument(l.Type, l.IsPinned);

            var exceptions = _exceptionBlocks.Select(x => new ExceptionHandler(
                x.BeginInstruction._lineNumber,
                x.HandlerInstruction._lineNumber - x.BeginInstruction._lineNumber,
                x.FilterInstruction != null ? x.FilterInstruction._lineNumber : 0,
                x.HandlerInstruction._lineNumber,
                x.EndInstruction._lineNumber - x.HandlerInstruction._lineNumber,
                x.Flags,
                x.CatchType != null ? _moduleBuilder.GetTypeToken(x.CatchType).Token : 0
            )).ToArray();


            builder.SetMethodBody(data, m_maxStackSize, localSig.GetSignature(), exceptions, fixups);

            //if (_type.Name == "RoomObj" && _method.Name == "Update")
            //{

            //}

        }

        private int m_maxMidStackCur, m_maxMidStack, m_maxStackSize;

        internal void UpdateStackSize(OpCode opcode, int stackchange)
        {
            m_maxMidStackCur += stackchange;

            if (m_maxMidStackCur > m_maxMidStack)
                m_maxMidStack = m_maxMidStackCur;
            else if (m_maxMidStackCur < 0)
                m_maxMidStackCur = 0;

            if (Util.EndsUncondJmpBlk(opcode))
            {
                m_maxStackSize += m_maxMidStack;
                m_maxMidStack = 0;
                m_maxMidStackCur = 0;
            }
        }


        public int IndexOf(InstructionBase item) { return _instructions.IndexOf(item); }
        public void Insert(int index, InstructionBase item) { _instructions.Insert(index, item); }
        public void RemoveAt(int index) { _instructions.RemoveAt(index); }
        public InstructionBase this[int index]
        {
            get { return _instructions[index]; }
            set { _instructions[index] = value; }
        }
        public void Add(InstructionBase item) { _instructions.Add(item); }
        public void Clear() { _instructions.Clear(); }
        public bool Contains(InstructionBase item) { return _instructions.Contains(item); }
        public void CopyTo(InstructionBase[] array, int arrayIndex) { _instructions.CopyTo(array, arrayIndex); }
        public int Count { get { return _instructions.Count; } }
        public bool IsReadOnly { get { return false; } }
        public bool Remove(InstructionBase item) { return _instructions.Remove(item); }
        public IEnumerator<InstructionBase> GetEnumerator() { return _instructions.GetEnumerator(); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _instructions.GetEnumerator(); }
    }
}