using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.TextTemplating;

[Controller]
[Authorize(AbpTextTemplatingPermissions.TextTemplateContent.Default)]
[RemoteService(Name = AbpTextTemplatingRemoteServiceConsts.RemoteServiceName)]
[Area(AbpTextTemplatingRemoteServiceConsts.ModuleName)]
[Route("api/text-templating/templates/content")]
public class TextTemplateContentController : AbpTextTemplatingControllerBase, ITextTemplateContentAppService
{
    private readonly ITextTemplateContentAppService _service;

    public TextTemplateContentController(ITextTemplateContentAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{Name}")]
    [Route("{Culture}/{Name}")]
    public virtual Task<TextTemplateContentDto> GetAsync(TextTemplateContentGetInput input)
    {
        return _service.GetAsync(input);
    }

    [HttpPut]
    [Route("{name}/restore-to-default")]
    [Authorize(AbpTextTemplatingPermissions.TextTemplateContent.Delete)]
    public virtual Task RestoreToDefaultAsync(string name, TextTemplateRestoreInput input)
    {
        return _service.RestoreToDefaultAsync(name, input);
    }

    [HttpPut]
    [Authorize(AbpTextTemplatingPermissions.TextTemplateContent.Update)]
    [Route("{name}")]
    public virtual Task<TextTemplateContentDto> UpdateAsync(string name, TextTemplateContentUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
