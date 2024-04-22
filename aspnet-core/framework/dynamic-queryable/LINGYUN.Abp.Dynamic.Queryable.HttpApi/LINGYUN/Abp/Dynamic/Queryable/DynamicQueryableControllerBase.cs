using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Dynamic.Queryable;

public abstract class DynamicQueryableControllerBase<TEntityDto> : AbpControllerBase, IDynamicQueryableAppService<TEntityDto>
{
    protected IDynamicQueryableAppService<TEntityDto> DynamicQueryableAppService { get; }

    protected DynamicQueryableControllerBase(
        IDynamicQueryableAppService<TEntityDto> dynamicQueryableAppService)
    {
        DynamicQueryableAppService = dynamicQueryableAppService;
    }

    [HttpGet]
    [Route("available-fields")]
    public Task<ListResultDto<DynamicParamterDto>> GetAvailableFieldsAsync()
    {
        return DynamicQueryableAppService.GetAvailableFieldsAsync();
    }

    [HttpPost]
    [Route("search")]
    public Task<PagedResultDto<TEntityDto>> GetListAsync(GetListByDynamicQueryableInput dynamicInput)
    {
        return DynamicQueryableAppService.GetListAsync(dynamicInput);
    }
}
