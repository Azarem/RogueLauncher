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

            throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
        }
    }
}
