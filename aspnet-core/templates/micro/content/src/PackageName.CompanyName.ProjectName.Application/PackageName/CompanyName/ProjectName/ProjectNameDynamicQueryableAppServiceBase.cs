using LINGYUN.Abp.Dynamic.Queryable;
using PackageName.CompanyName.ProjectName.Localization;

namespace PackageName.CompanyName.ProjectName;
/// <summary>
/// 提供动态查询接口实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TEntityDto">实体dto类型</typeparam>
public abstract class ProjectNameDynamicQueryableAppServiceBase<TEntity, TEntityDto> :
    DynamicQueryableAppService<TEntity, TEntityDto>,
    IProjectNameDynamicQueryableAppService<TEntityDto>
{
    protected ProjectNameDynamicQueryableAppServiceBase()
    {
        LocalizationResource = typeof(ProjectNameResource);
        ObjectMapperContext = typeof(ProjectNameApplicationModule);
    }
}
