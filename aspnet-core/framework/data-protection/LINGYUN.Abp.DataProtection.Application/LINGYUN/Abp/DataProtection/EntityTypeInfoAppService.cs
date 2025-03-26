using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.DataProtection;

public abstract class EntityTypeInfoAppService<TEntity> : ApplicationService, IEntityTypeInfoAppService
{
    protected IDataAccessEntityTypeInfoProvider EntityTypeInfoProvider => LazyServiceProvider.GetRequiredService<IDataAccessEntityTypeInfoProvider>();

    [Authorize]
    public virtual async Task<EntityTypeInfoDto> GetEntityRuleAsync(EntityTypeInfoGetInput input)
    {
        var entityType = typeof(TEntity);
        var resourceType = LocalizationResource ?? typeof(DefaultResource);

        var context = new DataAccessEntitTypeInfoContext(
            entityType,
            resourceType,
            input.Operation,
            LazyServiceProvider);

        var model = await EntityTypeInfoProvider.GetEntitTypeInfoAsync(context);

        return new EntityTypeInfoDto
        {
            Name = model.Name,
            DisplayName = model.DisplayName,
            Properties = model.Properties.Select(prop => new EntityPropertyInfoDto
            {
                Name = prop.Name,
                DisplayName = prop.DisplayName,
                TypeFullName = prop.TypeFullName,
                JavaScriptType = prop.JavaScriptType,
                Operates = prop.Operates,
                Enums = prop.Enums.Select(em => new EntityEnumInfoDto
                {
                    Key = em.Key,
                    Value = em.Value,
                }).ToArray()
            }).ToArray(),
        };
    }
}
