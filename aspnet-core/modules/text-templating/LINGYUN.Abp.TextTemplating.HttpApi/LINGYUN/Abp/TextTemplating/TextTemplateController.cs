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
    [Route("{Name}")]
    [Route("{Culture}/{Name}")]
    public Task<TextTemplateDto> GetAsync(TextTemplateGetInput input)
    {
        return TextTemplateAppService.GetAsync(input);
    }

    [HttpGet]
    public Task<ListResultDto<TextTemplateDto>> GetListAsync()
    {
        return TextTemplateAppService.GetListAsync();
    }

    [HttpDelete]
    [Route("{Name}")]
    [Route("{Culture}/{Name}")]
    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Delete)]
    public Task ResetDefaultAsync(TextTemplateGetInput input)
    {
        return TextTemplateAppService.ResetDefaultAsync(input);
    }

    [HttpPost]
    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Update)]
    public Task<TextTemplateDto> UpdateAsync(TextTemplateUpdateInput input)
    {
        return TextTemplateAppService.UpdateAsync(input);
    }
}
