using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.DataProtection;
public static class IDataAuthorizationServiceExtensions
{
    public static async Task CheckAsync<TEntity>(this IDataAuthorizationService dataAuthorizationService, DataAccessOperation operation, IEnumerable<TEntity> entities)
    {
        var result = await dataAuthorizationService.AuthorizeAsync(operation, entities);
        if (!result.Succeeded)
        {
            var entityKeys = entities.Select(x => x.ToString()).JoinAsString(";");
            throw new AbpAuthorizationException(
                $"The {operation} operation with entity type {typeof(Entity)} identified as {entityKeys} is not allowed!");
        }
    }
}
