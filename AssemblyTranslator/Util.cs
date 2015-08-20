using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace AssemblyTranslator
{
    internal static class Util
    {
        private static MethodInfo _fieldSetData = typeof(FieldBuilder).GetMethod("SetData", BindingFlags.NonPublic | BindingFlags.Instance);
        public static Action<FieldBuilder, byte[], int> FieldSetData = CreateWeakDelegate<Action<FieldBuilder, byte[], int>>(_fieldSetData);

        private static ConstructorInfo _typeBuildConstructor = typeof(TypeBuilder).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(string), typeof(TypeAttributes), typeof(Type), typeof(Type[]), typeof(ModuleBuilder), typeof(PackingSize), typeof(int), typeof(TypeBuilder) }, null);
        public static Func<string, TypeAttributes, Type, Type[], ModuleBuilder, PackingSize, int, TypeBuilder, TypeBuilder> DefineTypeBuilder = CreateWeakDelegate<Func<string, TypeAttributes, Type, Type[], ModuleBuilder, PackingSize, int, TypeBuilder, TypeBuilder>>(_typeBuildConstructor);


        private static readonly MethodInfo _endsUncondJmpBlk = typeof(OpCode).GetMethod("EndsUncondJmpBlk", BindingFlags.NonPublic | BindingFlags.Instance);
        public static readonly Func<OpCode, bool> EndsUncondJmpBlk = CreateWeakDelegate<Func<OpCode, bool>>(_endsUncondJmpBlk);

        private static readonly MethodInfo _stackChange = typeof(OpCode).GetMethod("StackChange", BindingFlags.NonPublic | BindingFlags.Instance);
        public static readonly Func<OpCode, int> StackChange = CreateWeakDelegate<Func<OpCode, int>>(_stackChange);

        private static readonly MethodInfo _getTypeTokenInternal = typeof(ModuleBuilder).GetMethod("GetTypeTokenInternal", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Type) }, null);
        public static readonly Func<ModuleBuilder, Type, TypeToken> GetTypeTokenInternal = CreateWeakDelegate<Func<ModuleBuilder, Type, TypeToken>>(_getTypeTokenInternal);// (Func<ModuleBuilder, Type, TypeToken>)Delegate.CreateDelegate(typeof(Func<ModuleBuilder, Type, TypeToken>), _getTypeTokenInternal);

        private static readonly PropertyInfo _argumentCount = typeof(SignatureHelper).GetProperty("ArgumentCount", BindingFlags.NonPublic | BindingFlags.Instance);
        public static readonly Func<SignatureHelper, int> ArgumentCount = CreateWeakDelegate<Func<SignatureHelper, int>>(_argumentCount.GetGetMethod(true));

        public static readonly Type _symbolMethodType = typeof(AssemblyBuilder).Assembly.GetType("System.Reflection.Emit.SymbolMethod");

        private static readonly MethodInfo _getMethodTokenInternal = typeof(ModuleBuilder).GetMethod("GetMethodTokenInternal", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(MethodBase), typeof(IEnumerable<Type>), typeof(bool) }, null);
        public static readonly Func<ModuleBuilder, MethodBase, IEnumerable<Type>, bool, int> GetMethodTokenInternal = CreateWeakDelegate<Func<ModuleBuilder, MethodBase, IEnumerable<Type>, bool, int>>(_getMethodTokenInternal);

        private static readonly MethodInfo _getParameterTypes = typeof(MethodBase).GetMethod("GetParameterTypes", BindingFlags.Instance | BindingFlags.NonPublic);
        public static readonly Func<MethodBase, Type[]> GetParameterTypes = CreateWeakDelegate<Func<MethodBase, Type[]>>(_getParameterTypes);


        //public static TypeBuilder DefineTypeBuilder(string name, TypeAttributes attr, Type parent, Type[] interfaces, ModuleBuilder module, PackingSize iPackingSize, int iTypeSize, TypeBuilder enclosingType)
        //{
        //    return _typeBuildConstructor.Invoke(new object[] { name, attr, parent, interfaces, module, iPackingSize, iTypeSize, enclosingType }) as TypeBuilder;
        //}

        public static T CreateWeakDelegate<T>(MethodBase info)
        {
            var type = typeof(T);
            var method = type.GetMethod("Invoke");
            var param = method.GetParameters();

            var parameters = param.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToArray();

            var call = info is ConstructorInfo
                ? (Expression)Expression.New(info as ConstructorInfo, parameters)
                : (Expression)Expression.Call(parameters[0], info as MethodInfo, parameters.Skip(1));

            return Expression.Lambda<T>(call, parameters).Compile();
        }
    }
}
