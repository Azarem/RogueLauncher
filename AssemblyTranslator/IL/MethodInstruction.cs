using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTranslator.IL
{
    public class MethodInstruction : TokenInstruction<MethodBase>
    {

        internal override void EmitInstruction(ILGenerator generator)
        {
            if (_operand is ConstructorInfo)
                generator.Emit(_opCode, (ConstructorInfo)_operand);
            else
                generator.Emit(_opCode, (MethodInfo)_operand);
        }

        //internal override unsafe void WriteILBytes(ref byte* ptr, IList<int> fixups)
        //{

        //    bool useMethodDef = true;
        //    MethodInfo callInfo = null;
        //    int stackChange = 0;

        //    if (_operand is ConstructorInfo)
        //    {
        //        if (_opCode.StackBehaviourPush == StackBehaviour.Varpush)
        //            stackChange++;

        //        if (_opCode.StackBehaviourPop == StackBehaviour.Varpop)
        //            stackChange -= (ILInstructionList._getParameterTypes.Invoke(_operand, null) as Type[] ?? new Type[0]).Length;
        //    }
        //    else
        //    {
        //        if (_opCode == OpCodes.Call || _opCode == OpCodes.Callvirt || _opCode == OpCodes.Newobj)
        //        {
        //            callInfo = _operand as MethodInfo;
        //            useMethodDef = false;

        //            if (callInfo.ReturnType != typeof(void))
        //                stackChange++;

        //            stackChange -= (ILInstructionList._getParameterTypes.Invoke(callInfo, null) as Type[] ?? new Type[0]).Length;

        //            if (!ILInstructionList._symbolMethodType.IsAssignableFrom(callInfo.GetType()) && !callInfo.IsStatic && _opCode != OpCodes.Newobj)
        //                stackChange--;
        //        }
        //        else
        //            useMethodDef = _opCode == OpCodes.Ldtoken || _opCode == OpCodes.Ldftn || _opCode == OpCodes.Ldvirtftn;
        //    }

        //    _token = (int)ILInstructionList._getMethodTokenInternal.Invoke(_list._moduleBuilder, new object[] { _operand, null, useMethodDef });

        //    base.WriteILBytes(ref ptr, fixups);

        //    _list.UpdateStackSize(_opCode, stackChange);
        //}
    }
}
