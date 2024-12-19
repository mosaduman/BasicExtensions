using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BasicExtensions.Models.DataTable;

namespace BasicExtensions
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Applies DataTable filters, global search, sorting, and pagination to the given IQueryable<T> and returns a DataTableResponse.
        /// </summary>
        /// <param name="query">The IQueryable<T> to apply filters on.</param>
        /// <param name="request">The DataTableRequest containing filter, search, sort, and pagination information.</param>
        /// <returns>A DataTableResponse containing the filtered and paginated data.</returns>
        public static DataTableResponse ApplyDataTableFilters<T>(this IQueryable<T> query, DataTableRequest request)
        {

            var parameter = Expression.Parameter(typeof(T), "entity");

            query = query.ApplyColumnFilters(request, parameter);
            query = query.ApplyGlobalSearchFilter(request, parameter);
            query = query.ApplySorting(request);
            var filteredCount = query.Count();
            var paginatedData = query.Skip(request.Start).Take(request.Length).ToList();

            return new DataTableResponse
            {
                Draw = request.Draw,
                RecordsTotal = filteredCount,
                RecordsFiltered = filteredCount,
                Data = paginatedData
            };
        }

        /// <summary>
        /// Applies column filters to the given IQueryable<T> based on the filter values provided in the DataTableRequest.
        /// </summary>
        /// <param name="query">The IQueryable<T> to apply filters on.</param>
        /// <param name="request">The DataTableRequest containing column filter values.</param>
        /// <param name="parameter">The parameter expression representing the entity.</param>
        /// <returns>The IQueryable<T> after applying column filters.</returns>
        public static IQueryable<T> ApplyColumnFilters<T>(this IQueryable<T> query, DataTableRequest request, ParameterExpression parameter)
        {
            foreach (var column in request.Columns.Where(c => !string.IsNullOrWhiteSpace(c.Search.Value)))
            {
                var propertyInfo = typeof(T).GetProperty(
                    name: column.Data,
                    bindingAttr: BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    var condition = CreateConditionExpression(parameter, propertyInfo, column.Search.Value);
                    if (condition != null)
                    {
                        query = query.Where(Expression.Lambda<Func<T, bool>>(condition, parameter));
                    }
                }
            }
            return query;
        }

        /// <summary>
        /// Applies global search filter to the given IQueryable<T> based on the search value provided in the DataTableRequest.
        /// </summary>
        /// <param name="query">The IQueryable<T> to apply the global search filter on.</param>
        /// <param name="request">The DataTableRequest containing the global search value.</param>
        /// <param name="parameter">The parameter expression representing the entity.</param>
        /// <returns>The IQueryable<T> after applying the global search filter.</returns>
        public static IQueryable<T> ApplyGlobalSearchFilter<T>(this IQueryable<T> query, DataTableRequest request, ParameterExpression parameter)
        {
            if (string.IsNullOrEmpty(request.Search?.Value))
                return query;
            var searchValue = request.Search?.Value.Trim();
            var globalSearchConditions = new List<Expression>();

            foreach (var column in request.Columns.Where(c => c.Searchable && !string.IsNullOrWhiteSpace(c.Data)))
            {
                var propertyInfo = typeof(T).GetProperty(
                    name: column.Data,
                    bindingAttr: BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    var condition = CreateConditionExpression(parameter, propertyInfo, searchValue);
                    if (condition != null)
                    {
                        globalSearchConditions.Add(condition);
                    }
                }
            }

            if (globalSearchConditions.Any())
            {
                var combinedCondition = globalSearchConditions.Aggregate((current, next) => Expression.OrElse(current, next));
                query = query.Where(Expression.Lambda<Func<T, bool>>(combinedCondition, parameter));
            }

            return query;
        }

        /// <summary>
        /// Applies sorting to the given IQueryable<T> based on the sorting parameters from the DataTableRequest.
        /// </summary>
        /// <param name="query">The IQueryable<T> to apply sorting on.</param>
        /// <param name="request">The DataTableRequest containing sorting order and column information.</param>
        /// <returns>The IQueryable<T> after applying the sorting.</returns>
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, DataTableRequest request)
        {
            if (!request.Order.Any())
                return query;

            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "x");
            Expression resultExpression = query.Expression;

            foreach (var order in request.Order)
            {
                var columnName = request.Columns[order.Column].Data;
                if (string.IsNullOrWhiteSpace(columnName))
                    continue;
                var property = Expression.Property(parameter, columnName);
                var lambda = Expression.Lambda(property, parameter);
                var methodName = order.Dir == "asc" ? "OrderBy" : "OrderByDescending";

                resultExpression = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { entityType, property.Type },
                    resultExpression,
                    Expression.Quote(lambda)
                );
            }
            return query.Provider.CreateQuery<T>(resultExpression);
        }

        /// <summary>
        /// Creates an expression representing a condition based on the property of the entity and the search value. It handles different types like string, DateTime, etc.
        /// </summary>
        /// <param name="parameter">The parameter expression representing the entity.</param>
        /// <param name="propertyInfo">The property information of the entity.</param>
        /// <param name="searchValue">The value to search for in the entity property.</param>
        /// <returns>An expression representing the condition for filtering, or null if no condition is created.</returns>
        private static Expression CreateConditionExpression(ParameterExpression parameter, PropertyInfo propertyInfo, string searchValue)
        {
            try
            {
                if (string.IsNullOrEmpty(searchValue)) return null;

                var property = Expression.Property(parameter, propertyInfo);
                var propertyType = propertyInfo.PropertyType;

                if (propertyType == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    if (containsMethod != null)
                        return Expression.Call(property, containsMethod, Expression.Constant(searchValue));
                }
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    var dateProperty = Expression.Property(property, "Date"); 
                    if (DateTime.TryParse(searchValue, out DateTime parsedDate))
                    {
                        return Expression.Equal(dateProperty, Expression.Constant(parsedDate.Date)); 
                    }
                }

                if (Nullable.GetUnderlyingType(propertyType) != null)
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                if (propertyType != null)
                {
                    var convertedValue = Convert.ChangeType(searchValue, propertyType);
                    return Expression.Equal(property, Expression.Constant(convertedValue));
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}
