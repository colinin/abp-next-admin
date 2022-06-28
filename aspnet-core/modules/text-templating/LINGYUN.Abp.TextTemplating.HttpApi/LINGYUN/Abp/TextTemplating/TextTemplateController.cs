using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TextTemplating;

[Controller]
[Authorize(AbpTextTemplatingPermissions.TextTemplate.Default)]
[RemoteService(Name = AbpTextTemplatingRemoteServiceConsts.RemoteServiceName)]
[Area(AbpTextTemplatingRemoteServiceConsts.ModuleName)]
[Route("api/text-templating/templates")]
public class TextTemplateController : AbpTextTemplatingControllerBase, ITextTemplateAppService
{
    protected ITextTemplateAppService TextTemplateAppService { get; }

    public TextTemplateController(
        ITextTemplateAppService textTemplateAppService)
    {
        TextTemplateAppService = textTemplateAppService;
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<TextTemplateDefinitionDto> GetAsync(string name)
    {
        return TextTemplateAppService.GetAsync(name);
    }

    [HttpGet]
    [Route("content/{Name}")]
    [Route("content/{Culture}/{Name}")]
    public virtual Task<TextTemplateContentDto> GetContentAsync(TextTemplateContentGetInput input)
    {
        return TextTemplateAppService.GetContentAsync(input);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input)
    {
        return TextTemplateAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("restore-to-default")]
    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Delete)]
    public virtual Task RestoreToDefaultAsync(TextTemplateRestoreInput input)
    {
        return TextTemplateAppService.RestoreToDefaultAsync(input);
    }

    [HttpPost]
    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Update)]
    public virtual Task<TextTemplateDefinitionDto> UpdateAsync(TextTemplateUpdateInput input)
    {
        return TextTemplateAppService.UpdateAsync(input);
    }
}
