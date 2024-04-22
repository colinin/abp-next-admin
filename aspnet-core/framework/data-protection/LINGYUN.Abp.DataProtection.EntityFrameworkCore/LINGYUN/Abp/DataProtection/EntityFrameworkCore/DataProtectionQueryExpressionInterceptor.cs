using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;
public class DataProtectionQueryExpressionInterceptor : IQueryExpressionInterceptor
{
    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        var dataFilter = eventData.Context.GetService<IDataFilter>();
        var dbContextProvider = eventData.Context.GetService<IDbContextProvider<AbpDataProtectionDbContext>>();
        return new DataProtectionExpressionVisitor(dataFilter, dbContextProvider).Visit(queryExpression);
    }

    public class DataProtectionExpressionVisitor : ExpressionVisitor
    {
        private readonly static MethodInfo WhereMethodInfo = typeof(Queryable).GetMethod(nameof(Queryable.Where));


        private readonly IDataFilter _dataFilter;
        private readonly ICurrentUser _currentUser;
        private readonly IDbContextProvider<AbpDataProtectionDbContext> _dbContextProvider;

        public DataProtectionExpressionVisitor(
            IDataFilter dataFilter,
            IDbContextProvider<AbpDataProtectionDbContext> dbContextProvider)
        {
            _dataFilter = dataFilter;
            _dbContextProvider = dbContextProvider;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (_dataFilter.IsEnabled<IHasDataAccess>())
            {
                var methodInfo = node!.Method;
                if (methodInfo.DeclaringType == typeof(Queryable)
                    && methodInfo.Name == nameof(Queryable.Select)
                    && methodInfo.GetParameters().Length == 2)
                {
                    var sourceType = node.Type.GetGenericArguments()[0];
                    var lambdaExpression = (LambdaExpression)((UnaryExpression)node.Arguments[1]).Operand;
                    var entityParameterExpression = lambdaExpression.Parameters[0];

                    var rules = _currentUser.Roles;
                    var ous = _currentUser.FindOrganizationUnits();

                    var ownerParamter = Expression.PropertyOrField(entityParameterExpression, nameof(IHasDataAccess.Owner));


                    if (typeof(IEntity).IsAssignableFrom(sourceType))
                    {
                        // Join params[0]
                        // node

                        
                        

                        var test = Expression.Call(
                            method: WhereMethodInfo.MakeGenericMethod(sourceType, typeof(bool)),
                            arg0: base.VisitMethodCall(node),
                            arg1: Expression.Lambda(
                                typeof(Func<,>).MakeGenericType(entityParameterExpression.Type, typeof(bool)),
                                Expression.Property(entityParameterExpression, nameof(IHasDataAccess.Owner)),
                                true));
                        return test;
                    }
                    
                }
            }
            return base.VisitMethodCall(node);
        }
    }
}
