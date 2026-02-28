using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Datas;

[Area("platform")]
[Route("api/platform/global-datas")]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
public class GlobalDataController : AbpControllerBase, IGlobalDataAppService
{
    protected IGlobalDataAppService DataAppService { get; }

    public GlobalDataController(
        IGlobalDataAppService dataAppService)
    {
        DataAppService = dataAppService;
    }

    [HttpGet]
    [Route("by-name/{name}")]
    public async virtual Task<DataDto> GetByNameAsync(string name)
    {
        return await DataAppService.GetByNameAsync(name);
    }

    [HttpGet]
    [Route("{id}")]
    public async virtual Task<DataDto> GetAsync(Guid id)
    {
        return await DataAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("all")]
    public async virtual Task<ListResultDto<DataDto>> GetAllAsync()
    {
        return await DataAppService.GetAllAsync();
    }
}
