using System.Linq.Expressions;

namespace WebApplication.Infrastructure
{
    public interface ISearchExpressionProvider
    {
        ConstantExpression GetValue(string input);
        Expression GetComparison(MemberExpression left, string op, ConstantExpression right);
    }
}