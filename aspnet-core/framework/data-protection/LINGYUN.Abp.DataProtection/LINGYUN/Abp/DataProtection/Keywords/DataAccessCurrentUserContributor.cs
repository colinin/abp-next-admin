using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.Keywords;
/// <summary>
/// 适用于过滤当前用户数据
/// </summary>
public class DataAccessCurrentUserContributor : IDataAccessKeywordContributor
{
    public const string Name = "@CurrentUser";
    public string Keyword => Name;
    public bool IsExternal => false;

    public Expression Contribute(DataAccessKeywordContributorContext context)
    {
        var conversionType = context.Expression.Body.Type;
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

        var userId = CastTo(currentUser.Id, conversionType);

        // entity.Where(x => x.CreatorId == CurrentUser.Id);
        return Expression.Constant(userId, conversionType);
    }

    private static object CastTo(object value, Type conversionType)
    {
        if (conversionType == typeof(Guid) || conversionType == typeof(Guid?))
        {
            return TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(value.ToString()!)!;
        }
        return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
    }
}
