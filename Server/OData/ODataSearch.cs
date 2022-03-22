using ClinicProject.Shared.Attributes;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.OData.UriParser;
using System.Linq.Expressions;
using System.Reflection;

namespace ClinicProject.Server.OData
{
    public class ODataSearch : QueryBinder, ISearchBinder
    {
        private static readonly Dictionary<BinaryOperatorKind, ExpressionType> BinaryOperatorMapping = new()
        {
            { BinaryOperatorKind.And, ExpressionType.AndAlso },
            { BinaryOperatorKind.Or, ExpressionType.OrElse },
        };

        public Expression BindSearch(SearchClause searchClause, QueryBinderContext context)
        {
            Expression exp = BindSingleValueNode(searchClause.Expression, context);

            Expression lambdaExp = Expression.Lambda(exp, context.CurrentParameter);

            return lambdaExp;
        }

        public Expression BindSearchTerm(SearchTermNode searchTermNode, QueryBinderContext context)
        {
            Expression source = context.CurrentParameter;

            Expression exFin = Expression.Constant(false);

            foreach (var property in context.ElementClrType.GetProperties())
            {
                var propType = property.PropertyType;

                var dataFieldAttribute = property.GetCustomAttributes()
                    .Where(a => a.GetType() == typeof(DataFieldAttribute))
                    .Cast<DataFieldAttribute>().FirstOrDefault();

                if (dataFieldAttribute != null && dataFieldAttribute.ServerSearchable == true)
                {
                    if (propType == typeof(int))
                    {
                        Expression exProp = Expression.Property(source, property.Name);
                        Expression exString = Expression.Call(exProp, "ToString", typeArguments: null, arguments: null);
                        Expression exContains = Expression.Call(exString, "Contains", typeArguments: null, arguments: Expression.Constant(searchTermNode.Text));

                        exFin = Expression.OrElse(exFin, exContains);
                    }
                    else if (propType == typeof(string))
                    {
                        Expression exProp = Expression.Property(source, property.Name);
                        Expression exContains = Expression.Call(exProp, "Contains", typeArguments: null, arguments: Expression.Constant(searchTermNode.Text));

                        exFin = Expression.OrElse(exFin, exContains);
                    }
                }
            }

            return exFin;
        }

        public override Expression BindSingleValueNode(SingleValueNode node, QueryBinderContext context)
        {
            switch (node.Kind)
            {
                case QueryNodeKind.BinaryOperator:
                    return BindBinaryOperatorNode(node as BinaryOperatorNode, context);

                case QueryNodeKind.SearchTerm:
                    return BindSearchTerm(node as SearchTermNode, context);

                case QueryNodeKind.UnaryOperator:
                    return BindUnaryOperatorNode(node as UnaryOperatorNode, context);
            }

            return null;
        }

        public override Expression BindBinaryOperatorNode(BinaryOperatorNode binaryOperatorNode, QueryBinderContext context)
        {
            Expression left = Bind(binaryOperatorNode.Left, context);

            Expression right = Bind(binaryOperatorNode.Right, context);

            if (BinaryOperatorMapping.TryGetValue(binaryOperatorNode.OperatorKind, out ExpressionType binaryExpressionType))
            {
                return Expression.MakeBinary(binaryExpressionType, left, right);
            }

            throw new NotImplementedException($"Binary operator '{binaryOperatorNode.OperatorKind}' is not supported!");
        }
    }
}
