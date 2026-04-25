using LINGYUN.Abp.BlobManagement.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.BlobManagement;

[Controller]
[Area(BlobManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = BlobManagementRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{BlobManagementRemoteServiceConsts.ModuleName}/containers")]
public class BlobContainerController : AbpControllerBase, IBlobContainerAppService
{
    private readonly IBlobContainerAppService _service;
    public BlobContainerController(IBlobContainerAppService service)
    {
        _service = service;
    }

    [HttpPost]
    public virtual Task<BlobContainerDto> CreateAsync(BlobContainerCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    public virtual Task<BlobContainerDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<BlobContainerDto>> GetListAsync(BlobContainerGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }
}
