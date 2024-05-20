using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection;

public class DataAuthorizationService : IDataAuthorizationService, ITransientDependency
{
    private readonly static MethodInfo AllMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.All), BindingFlags.Public | BindingFlags.Static);
    private readonly IEntityTypeFilterBuilder _entityTypeFilterBuilder;

    public DataAuthorizationService(IEntityTypeFilterBuilder entityTypeFilterBuilder)
    {
        _entityTypeFilterBuilder = entityTypeFilterBuilder;
    }
    
    public virtual Task<AuthorizationResult> AuthorizeAsync<TEntity>(DataAccessOperation operation, IEnumerable<TEntity> entities)
    {
        if (!entities.Any())
        {
            return Task.FromResult(AuthorizationResult.Success());
        }

        var exp = _entityTypeFilterBuilder.Build<TEntity>(operation);

        return Task.FromResult(entities.All(exp.Compile()) ? AuthorizationResult.Success() : AuthorizationResult.Failed());
    }
}