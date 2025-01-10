using LINGYUN.Abp.Dynamic.Queryable;
using PackageName.CompanyName.ProjectName.Localization;

namespace PackageName.CompanyName.ProjectName;
/// <summary>
/// 提供动态查询控制器实现
/// </summary>
/// <typeparam name="TEntityDto">实体dto类型</typeparam>
public abstract class ProjectNameDynamicQueryableControllerBase<TEntityDto> : DynamicQueryableControllerBase<TEntityDto>
{
    protected ProjectNameDynamicQueryableControllerBase(
        IDynamicQueryableAppService<TEntityDto> service)
        : base(service)
    {
        LocalizationResource = typeof(ProjectNameResource);
    }
}
