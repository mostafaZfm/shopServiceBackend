using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ShopServiceApi.Domain.Utility
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string propertyName,
            bool isAscending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");

            var property = Expression.PropertyOrField(parameter, propertyName);

            var lambda = Expression.Lambda(property, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(lambda)
            );

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
