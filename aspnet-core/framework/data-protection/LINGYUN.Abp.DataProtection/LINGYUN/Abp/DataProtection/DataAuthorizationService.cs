using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection;

public class DataAuthorizationService : IDataAuthorizationService, ITransientDependency
{
    private readonly IEntityTypeFilterBuilder _entityTypeFilterBuilder;

    public DataAuthorizationService(IEntityTypeFilterBuilder entityTypeFilterBuilder)
    {
        _entityTypeFilterBuilder = entityTypeFilterBuilder;
    }
    
    public async virtual Task<AuthorizationResult> AuthorizeAsync<TEntity>(DataAccessOperation operation, IEnumerable<TEntity> entities)
    {
        if (!entities.Any())
        {
            return AuthorizationResult.Success();
        }

        var exp = await _entityTypeFilterBuilder.Build<TEntity>(operation);

        return entities.All(exp.Compile()) ? AuthorizationResult.Success() : AuthorizationResult.Failed();
    }
}