using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.Keywords;
public class DataAccessCurrentUserContributor : IDataAccessKeywordContributor
{
    public const string Name = "@CurrentUser";
    public string Keyword => Name;

    public Expression Contribute(DataAccessKeywordContributorContext context)
    {
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

        var userId = CastTo(currentUser.Id, context.ConversionType);

        // entity.Where(x => x.CreatorId == CurrentUser.Id);
        return Expression.Constant(userId, context.ConversionType);
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
