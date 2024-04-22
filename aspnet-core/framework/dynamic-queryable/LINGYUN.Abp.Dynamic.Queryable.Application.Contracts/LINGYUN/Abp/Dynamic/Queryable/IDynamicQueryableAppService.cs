using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Dynamic.Queryable;

public interface IDynamicQueryableAppService<TEntityDto>
{
    /// <summary>
    /// 获取可用字段列表
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<DynamicParamterDto>> GetAvailableFieldsAsync();
    /// <summary>
    /// 根据动态条件查询数据
    /// </summary>
    /// <param name="dynamicInput"></param>
    /// <returns></returns>
    Task<PagedResultDto<TEntityDto>> GetListAsync(GetListByDynamicQueryableInput dynamicInput);
}
