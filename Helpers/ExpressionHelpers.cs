using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Helpers
{
    public static class ExpressionHelpers
    {
        public static Expression<T> Combine<T>(
                Expression<T> firstExpression,
                Expression<T> secondExpression)
        {
            if (firstExpression is null)
            {
                return secondExpression;
            }

            if (secondExpression is null)
            {
                return firstExpression;
            }

            var invokedExpression = Expression.Invoke(
                secondExpression,
                firstExpression.Parameters);

            var combinedExpression = Expression.AndAlso(firstExpression.Body, invokedExpression);

            return Expression.Lambda<T>(combinedExpression, firstExpression.Parameters);
        }
        public static Expression<T> AddAlso<T>(this 
            Expression<T> firstExpression,
            Expression<T> secondExpression)
        {

            if (secondExpression is null)
            {
                return firstExpression;
            }

            var invokedExpression = Expression.Invoke(
                secondExpression,
                firstExpression.Parameters);

            var combinedExpression = Expression.AndAlso(firstExpression.Body, invokedExpression);

            return Expression.Lambda<T>(combinedExpression, firstExpression.Parameters);
        }
    }
}
