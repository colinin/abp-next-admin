using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;
public class DataProtectionQueryExpressionInterceptor : IQueryExpressionInterceptor
{
    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        return new DataProtectionExpressionVisitor().Visit(queryExpression);
    }

    public class DataProtectionExpressionVisitor : ExpressionVisitor
    {
        private readonly static MethodInfo WhereMethodInfo = typeof(Queryable).GetMethod(nameof(Queryable.Where));
        private readonly IDataFilter _dataFilter;

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            //if (_dataFilter.IsEnabled<IDataProtection>())
            //{
            //    var methodInfo = node!.Method;
            //    if (methodInfo.DeclaringType == typeof(Queryable)
            //        && methodInfo.Name == nameof(Queryable.Select)
            //        && methodInfo.GetParameters().Length == 2)
            //    {
            //        var sourceType = node.Type.GetGenericArguments()[0];
            //        var lambdaExpression = (LambdaExpression)((UnaryExpression)node.Arguments[1]).Operand;
            //        var entityParameterExpression = lambdaExpression.Parameters[0];
            //        var test = Expression.Call(
            //            method: WhereMethodInfo.MakeGenericMethod(sourceType, typeof(bool)),
            //            arg0: base.VisitMethodCall(node),
            //            arg1: Expression.Lambda(typeof(Func<,>).MakeGenericType(entityParameterExpression.Type, typeof(bool)),
            //            Expression.Property(entityParameterExpression, nameof(IDataProtection.Owner)),
            //            true));
            //        return test;
            //    }
            //}
            return base.VisitMethodCall(node);
        }
    }
}
