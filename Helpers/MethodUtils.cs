using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Helpers
{
    public static class MethodUtils
    {
        public static (MethodInfo, object[]) ExtractMethodInfoAndArgs<T>(Expression<Func<T, Task>> expression)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                var methodInfo = methodCall.Method;
                var arguments = methodCall.Arguments.Select(arg => Expression.Lambda(arg).Compile().DynamicInvoke()).ToArray();
                return (methodInfo, arguments);
            }

            throw new ArgumentException("Invalid expression format");
        }
    }
}
