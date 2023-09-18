using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Share.Extiontions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereHasValue<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            var hasValuePredicate = PredicateBuilder.HasValue(predicate);
            return source.Where(hasValuePredicate);
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> HasValue<T>(Expression<Func<T, bool>> predicate)
        {
            var parameter = predicate.Parameters[0];
            var nullCheck = Expression.NotEqual(parameter, Expression.Constant(null, typeof(T)));
            var hasValueExpression = Expression.AndAlso(nullCheck, predicate.Body);
            return Expression.Lambda<Func<T, bool>>(hasValueExpression, parameter);
        }
    }
}
