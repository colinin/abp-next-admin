using LINGYUN.Abp.LocalizationManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.LocalizationManagement;

[Authorize(LocalizationManagementPermissions.Resource.Default)]
[RemoteService(Name = LocalizationRemoteServiceConsts.RemoteServiceName)]
[Area("localization")]
[Route("api/localization/resources")]
public class ResourceController : AbpControllerBase, IResourceAppService
{
    private readonly IResourceAppService _service;

    public ResourceController(IResourceAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<ResourceDto> GetByNameAsync(string name)
    {
        return _service.GetByNameAsync(name);
    }

    [HttpPost]
    [Authorize(LocalizationManagementPermissions.Resource.Create)]
    public virtual Task<ResourceDto> CreateAsync(ResourceCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(LocalizationManagementPermissions.Resource.Delete)]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(LocalizationManagementPermissions.Resource.Update)]
    public virtual Task<ResourceDto> UpdateAsync(string name, ResourceUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
