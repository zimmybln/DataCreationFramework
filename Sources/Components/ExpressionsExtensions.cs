using System;
using System.Linq.Expressions;

namespace DataCreationFramework.Components
{
    public static class ExpressionsExtensions
    {
        /// <summary>
        /// Liefert den Namen der Methode bzw. Eigenschaft
        /// </summary>
        public static string GetMemberInfoName(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr.Member.Name;
        }
    }
}
