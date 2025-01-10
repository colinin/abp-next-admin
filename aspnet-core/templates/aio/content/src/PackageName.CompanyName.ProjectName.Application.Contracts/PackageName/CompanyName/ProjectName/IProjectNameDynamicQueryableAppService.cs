using LINGYUN.Abp.Dynamic.Queryable;

namespace PackageName.CompanyName.ProjectName;
/// <summary>
/// 提供动态查询接口定义
/// </summary>
/// <typeparam name="TEntityDto">实体dto类型</typeparam>
public interface IProjectNameDynamicQueryableAppService<TEntityDto> : IDynamicQueryableAppService<TEntityDto>
{
}
