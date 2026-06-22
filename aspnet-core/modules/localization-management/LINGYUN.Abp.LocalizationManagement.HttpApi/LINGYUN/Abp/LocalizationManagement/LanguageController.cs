using LINGYUN.Abp.LocalizationManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
namespace LINGYUN.Abp.LocalizationManagement;

[Authorize(LocalizationManagementPermissions.Language.Default)]
[RemoteService(Name = LocalizationRemoteServiceConsts.RemoteServiceName)]
[Area("localization")]
[Route("api/localization/languages")]
public class LanguageController : AbpControllerBase, ILanguageAppService
{
    private readonly ILanguageAppService _service;

    public LanguageController(ILanguageAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<LanguageDto> GetByNameAsync(string name)
    {
        return _service.GetByNameAsync(name);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<LanguageDto>> GetListAsync(LanguageGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(LocalizationManagementPermissions.Language.Create)]
    public virtual Task<LanguageDto> CreateAsync(LanguageCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(LocalizationManagementPermissions.Language.Delete)]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(LocalizationManagementPermissions.Language.Update)]
    public virtual Task<LanguageDto> UpdateAsync(string name, LanguageUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
