using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    /// <summary>
    /// Represents the helper (method extender) for the sorting lambda expressions.
    /// </summary>
    internal static class SortExpressionHelper
    {
        #region Private Static Methods
        private static IOrderedQueryable<TAggregateRoot> InvokeOrderBy<TAggregateRoot>(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
            where TAggregateRoot : class, IAggregateRoot
        {
            var param = sortPredicate.Parameters[0];
            string propertyName = null;
            Type propertyType = null;
            Expression bodyExpression = null;
            if (sortPredicate.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = sortPredicate.Body as UnaryExpression;
                bodyExpression = unaryExpression.Operand;
            }
            else if (sortPredicate.Body is MemberExpression)
            {
                bodyExpression = sortPredicate.Body;
            }
            else
                throw new ArgumentException("The body of the sort predicate expression should be either UnaryExpression or MemberExpression.", "sortPredicate");
            MemberExpression memberExpression = (MemberExpression)bodyExpression;
            propertyName = memberExpression.Member.Name;
            if (memberExpression.Member.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
                propertyType = propertyInfo.PropertyType;
            }
            else
                throw new InvalidOperationException("Cannot evaluate the type of property since the member expression represented by the sort predicate expression does not contain a PropertyInfo object.");

            Type funcType = typeof(Func<,>).MakeGenericType(typeof(TAggregateRoot), propertyType);
            LambdaExpression convertedExpression = Expression.Lambda(funcType,
                Expression.Convert(Expression.Property(param, propertyName), propertyType), param);

            var sortingMethods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);
            var sortingMethodName = GetSortingMethodName(sortOrder);
            var sortingMethod = sortingMethods.Where(sm => sm.Name == sortingMethodName &&
                sm.GetParameters() != null &&
                sm.GetParameters().Length == 2).First();
            return (IOrderedQueryable<TAggregateRoot>)sortingMethod
                .MakeGenericMethod(typeof(TAggregateRoot), propertyType)
                .Invoke(null, new object[] { query, convertedExpression });
        }

        private static string GetSortingMethodName(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "OrderBy";
                case SortOrder.Descending:
                    return "OrderByDescending";
                default:
                    throw new ArgumentException("Sort Order must be specified as either Ascending or Descending.", "sortOrder");
            }
        }
        #endregion

        #region Internal Method Extensions
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a lambda expression.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="sortPredicate">The lambda expression which indicates the property for sorting.</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to the lambda expression.</returns>
        internal static IOrderedQueryable<TAggregateRoot> OrderBy<TAggregateRoot>(this IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, dynamic>> sortPredicate)
            where TAggregateRoot : class, IAggregateRoot
        {
            return InvokeOrderBy(query, sortPredicate, SortOrder.Ascending);
        }
        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a lambda expression.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="sortPredicate">The lambda expression which indicates the property for sorting.</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to the lambda expression.</returns>
        internal static IOrderedQueryable<TAggregateRoot> OrderByDescending<TAggregateRoot>(this IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, dynamic>> sortPredicate)
            where TAggregateRoot : class, IAggregateRoot
        {
            return InvokeOrderBy(query, sortPredicate, SortOrder.Descending);
        }
        #endregion
    }
}
