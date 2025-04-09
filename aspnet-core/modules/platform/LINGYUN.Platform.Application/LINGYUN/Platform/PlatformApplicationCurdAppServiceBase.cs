using LINGYUN.Platform.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform;

public abstract class PlatformApplicationCurdAppServiceBase<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput> :
    CrudAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<TKey>
{
    protected PlatformApplicationCurdAppServiceBase(
        IRepository<TEntity, TKey> repository) : base(repository)
    {
        LocalizationResource = typeof(PlatformResource);
        ObjectMapperContext = typeof(PlatformApplicationModule);
    }
}
