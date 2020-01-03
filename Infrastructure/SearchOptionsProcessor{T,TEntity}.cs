using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace WebApplication.Infrastructure
{
    public class SearchOptionsProcessor <T,TEntity>
    {
        private readonly string[] _searchQuery;
        public SearchOptionsProcessor(string[] searchQuery)
        {
            _searchQuery = searchQuery;
        }

        public IEnumerable<SearchTerm> GetAllTerms()
        {
            if(_searchQuery == null) yield break;
            foreach (var expression in _searchQuery)
            {
                if (string.IsNullOrEmpty(expression)) continue;
                var tokens = expression.Split(" ");
                
                //each expression looks like this
                //"fieldname Operator Value"
                if (tokens.Length == 0)
                {
                    yield return new SearchTerm
                    {
                        Name = expression, ValidSyntax = false
                    };
                    continue;
                }
                if (tokens.Length < 3)
                {
                    yield return new SearchTerm
                    {
                        Name = tokens[0], ValidSyntax = false
                    };
                    continue;
                }
                yield return new SearchTerm
                {
                    Name = tokens[0], ValidSyntax = true, 
                    operators = tokens[1], 
                    Value = string.Join(" ", tokens.Skip(2))
                };
            }
        }

        public IEnumerable<SearchTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms()
                .Where(x => x.ValidSyntax).ToArray();
            if (!queryTerms.Any()) yield break;
            var declaredTerms = GetValidTerms();
            foreach (var term in queryTerms)
            {
                var declaredTerm = declaredTerms
                    .SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase));
                if (declaredTerm == null) continue;
                yield return new SearchTerm
                {
                    ValidSyntax = term.ValidSyntax,
                    Name = term.Name,
                    operators = term.operators,
                    Value = term.Value
                };
            }
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var terms = GetValidTerms().ToArray();
            if (!terms.Any()) return query;
            var modifiedQuery = query;
            foreach (var term in terms)
            {
                var propertyInfo = ExpressionHelper.GetPropertyInfo<TEntity>(term.Name);
                var obj = ExpressionHelper.Parameter<TEntity>();
                //build  the linq expression backwards
                //query = query.where(x => x.Property == "Value");
                
                //x.property
                var left = ExpressionHelper.GetPropertyExpression(obj, propertyInfo);
                //"Value"
                var right = Expression.Constant(term.Value);
                //x.property == "value"
                var ExpressionProvider = new DecimalToIntSearchExpressionProvider();
                var comparisonExpression = ExpressionProvider.GetComparison(left, term.operators, right);
//                var comparisonExpression = Expression.Equal(left, right);
                //x=> x.property == value
                var lambdaExpression = ExpressionHelper.GetLambda<TEntity, bool>(obj, comparisonExpression);
                //query = query.Where
                modifiedQuery = ExpressionHelper.CallWhere(modifiedQuery, lambdaExpression);
            }

            return modifiedQuery;
        }
        private static IEnumerable<SearchTerm> GetTermsFromModel()
            => typeof(T).GetTypeInfo().DeclaredProperties
                .Where(p => p.GetCustomAttributes<SearchableAttribute>().Any())
                .Select(p => new SearchTerm {Name = p.Name});
        
    }
}