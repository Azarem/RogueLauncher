using AssemblyTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public abstract class TokenInstruction<T> : Instruction<T> where T : class
    {
        internal int _token;
        protected override void ReadOperand(byte[] data, ref int offset) { _token = BitConverter.ToInt32(data, offset); offset += 4; }
        internal override void ParseOperand() { _operand = _list.ResolveToken<T>(_token); }
        internal override void TranslateOperand(GraphManager t, Type[] newGenerics = null) { _operand = t.GetMember<T>(_operand, newGenerics); }
        internal override void OptimizeInstruction()
        {
            //_token = (_operand as MemberInfo).MetadataToken; 
        }
        internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        {
            var stackChange = 0;

            if (_operand is FieldInfo)
                _token = _list._moduleBuilder.GetFieldToken(_operand as FieldInfo).Token;
            else if (_operand is Type)
                _token = (_opCode != OpCodes.Ldtoken || _operand == null || !(_operand as Type).IsGenericTypeDefinition
                ? Util.GetTypeTokenInternal(_list._moduleBuilder, _operand as Type).Token
                : _list._moduleBuilder.GetTypeToken(_operand as Type).Token);
            else if (_operand is MethodBase)
            {
                bool useMethodDef = true;

                if (_operand is ConstructorInfo)
                {
                    if (_opCode.StackBehaviourPush == StackBehaviour.Varpush)
                        stackChange++;

                    if (_opCode.StackBehaviourPop == StackBehaviour.Varpop)
                        stackChange -= (Util.GetParameterTypes(_operand as MethodBase) ?? new Type[0]).Length;
                }
                else
                {
                    if (_opCode == OpCodes.Call || _opCode == OpCodes.Callvirt || _opCode == OpCodes.Newobj)
                    {
                        var callInfo = _operand as MethodInfo;
                        useMethodDef = false;

                        if (callInfo.ReturnType != typeof(void))
                            stackChange++;

                        stackChange -= (Util.GetParameterTypes(callInfo) ?? new Type[0]).Length;

                        if (!Util._symbolMethodType.IsAssignableFrom(callInfo.GetType()) && !callInfo.IsStatic && _opCode != OpCodes.Newobj)
                            stackChange--;
                    }
                    else
                        useMethodDef = _opCode == OpCodes.Ldtoken || _opCode == OpCodes.Ldftn || _opCode == OpCodes.Ldvirtftn;
                }

                _token = Util.GetMethodTokenInternal(_list._moduleBuilder, _operand as MethodBase, null, useMethodDef);// (int)ILInstructionList._getMethodTokenInternal.Invoke(_list._moduleBuilder, new object[] { _operand, null, useMethodDef });
            }
            else if (_operand is string)
                _token = _list._moduleBuilder.GetStringConstant(_operand as string).Token;
            else
                throw new InvalidOperationException();

            base.WriteILBytes(ref ptr, fixups);

            fixups.Add(_lineNumber + _opCode.Size);

            *(int*)ptr = _token;
            ptr += 4;

            if (stackChange != 0)
                _list.UpdateStackSize(_opCode, stackChange);
        }
    }
}
