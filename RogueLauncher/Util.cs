using AssemblyTranslator.IL;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RogueLauncher
{
    internal static class Util
    {
        public static MethodBase GetMethodInfo(Expression<Action> expression) { return GetMethodInfo((LambdaExpression)expression); }

        public static MethodBase GetMethodInfo<T>(Expression<Action<T>> expression) { return GetMethodInfo((LambdaExpression)expression); }

        public static MethodBase GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression) { return GetMethodInfo((LambdaExpression)expression); }

        public static MethodBase GetMethodInfo(LambdaExpression expression)
        {
            var method = expression.Body as MethodCallExpression;
            if (method != null)
                return method.Method;

            var newEx = expression.Body as NewExpression;
            if (newEx != null)
                return newEx.Constructor;

            var propEx = expression.Body as MemberExpression;
            if (propEx != null)
            {
                if (propEx.Member is PropertyInfo)
                    return ((PropertyInfo)propEx.Member).GetGetMethod();
            }

            throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
        }

        public static T ParseOperand<T>(byte id, InstructionList instr, InstructionBase i, Action<byte, T, bool> setHandler, bool condition = false)
        {
            object value = null;

            if (typeof(T) == typeof(string))
            {
                string accum = "";
                //string accum = (string)value;
                var ix3 = instr.IndexOf(i);
                while (true)
                {
                    var i2 = instr[ix3++];
                    if (i2 is LabelInstruction)
                    {
                        var cond = instr[ix3 - 2];
                        if (cond is ParameterInstruction)
                        {
                            //We can only use bool here
                            if (i2.ILCode == ILCode.Brfalse || i2.ILCode == ILCode.Brfalse_S)
                                condition = true;
                            else if (i2.ILCode != ILCode.Brtrue && i2.ILCode != ILCode.Brtrue_S)
                                throw new InvalidOperationException("Can't handle this condition properly!");

                            ParseOperand(id, instr, ((LabelInstruction)i2).JumpToInstruction, setHandler, !condition);
                        }

                    }
                    else if (i2.ILCode == ILCode.Ret)
                        break;
                    else if (instr[ix3].ILCode == ILCode.Newarr)
                    {
                        ix3++;
                        continue;
                    }
                    else if (i2 is StringInstruction || instr[ix3].ILCode == ILCode.Box)
                        accum += i2.RawOperand;

                }
                value = accum;
            }
            else if (typeof(T) == typeof(byte[]))
            {
                if (i.ILCode != ILCode.Ldnull)
                {
                    var ix3 = instr.IndexOf(i) + 1;
                    int length = (int)i.RawOperand;
                    byte[] data = new byte[length];
                    value = data;
                    while (true)
                    {
                        var i2 = instr[ix3++];

                        if (i2.ILCode == ILCode.Ldtoken)
                        {
                            System.Runtime.CompilerServices.RuntimeHelpers.InitializeArray(data, ((FieldInfo)((MemberInstruction)i2).Operand).FieldHandle);
                            break;
                        }
                        else if (i2.ILCode == ILCode.Stelem_I1)
                        {
                            var ind = (int)instr[ix3 - 3].RawOperand;
                            var val = Convert.ToByte(instr[ix3 - 2].RawOperand);
                            data[ind] = val;
                        }
                        else if (i2.ILCode == ILCode.Ret)
                            break;
                    }
                }
            }
            else
                value = i.RawOperand;

            if (value != null && !(value is T))
                value = Convert.ChangeType(value, typeof(T));

            setHandler(id, (T)value, condition);

            return (T)value;
        }

        internal static void ReadSwitches<TValue>(InstructionList instr, Action<byte, TValue, bool> setHandler, Action<byte, TValue, bool> defaultHandler)
        {
            bool first = true;
            int ix = 0, count = instr.Count, ix2, offset = 0;

            while (ix < count)
            {
                var i1 = instr[ix++];

                if (i1.ILCode == ILCode.Ret)
                    break;

                if (i1 is SwitchInstruction)
                {
                    //Get offset
                    var oi = instr[ix - 2];
                    if (oi.ILCode == ILCode.Sub)
                    {
                        oi = instr[ix - 3];
                        {
                            offset = (int)oi.RawOperand;
                        }
                    }

                    //Read string values from switch statement
                    var switchJumps = ((SwitchInstruction)i1).JumpToInstructions;
                    var jCount = switchJumps.Count;
                    ix2 = 0;
                    while (ix2 < jCount)
                        ParseOperand<TValue>((byte)(ix2 + offset), instr, switchJumps[ix2++], setHandler);
                }
                else if (i1 is LabelInstruction)
                {
                    bool useDefault = true;
                    if (i1.ILCode == ILCode.Beq || i1.ILCode == ILCode.Beq_S)
                    {
                        var def = instr[ix - 2];
                        var opnd = def.RawOperand;
                        if (opnd != null)
                        {
                            ix2 = Convert.ToInt32(opnd);
                            useDefault = false;
                        }
                        else
                            ix2 = offset - 1;
                    }
                    else
                    {
                        ix2 = offset - 1;
                        ix = count;
                    }

                    if (useDefault)
                        ParseOperand<TValue>((byte)ix2, instr, ((LabelInstruction)i1).JumpToInstruction, defaultHandler);
                    else
                        ParseOperand<TValue>((byte)ix2, instr, ((LabelInstruction)i1).JumpToInstruction, setHandler);
                }
            }
        }
    }
}
