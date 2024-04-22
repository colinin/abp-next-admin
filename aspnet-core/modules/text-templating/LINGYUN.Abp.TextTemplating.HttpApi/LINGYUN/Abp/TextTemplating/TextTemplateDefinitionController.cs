using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TextTemplating;

[Controller]
[Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Default)]
[RemoteService(Name = AbpTextTemplatingRemoteServiceConsts.RemoteServiceName)]
[Area(AbpTextTemplatingRemoteServiceConsts.ModuleName)]
[Route("api/text-templating/template/definitions")]
public class TextTemplateDefinitionController : AbpTextTemplatingControllerBase, ITextTemplateDefinitionAppService
{
    private readonly ITextTemplateDefinitionAppService _service;

    public TextTemplateDefinitionController(ITextTemplateDefinitionAppService service)
    {
        _service = service;
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Create)]
    [HttpPost]
    public virtual Task<TextTemplateDefinitionDto> CreateAsync(TextTemplateDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Delete)]
    [HttpDelete]
    [Route("{name}")]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<TextTemplateDefinitionDto> GetByNameAsync(string name)
    {
        return _service.GetByNameAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Update)]
    [HttpPut]
    [Route("{name}")]
    public virtual Task<TextTemplateDefinitionDto> UpdateAsync(string name, TextTemplateDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
