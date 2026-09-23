using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.LocalizationManagement;

[RemoteService(Name = LocalizationRemoteServiceConsts.RemoteServiceName)]
[Area("localization")]
[Route("api/localization/texts")]
public class TextController : AbpControllerBase, ITextAppService
{
    private readonly ITextAppService _service;

    public TextController(ITextAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("by-key")]
    public virtual Task<TextDto> GetByKeyAsync(TextGetByKeyInput input)
    {
        return _service.GetByKeyAsync(input);
    }

    [HttpGet]
    public virtual Task<ListResultDto<TextDifferenceDto>> GetDifferencesAsync(TextDifferenceGetListInput input)
    {
        return _service.GetDifferencesAsync(input);
    }

    [HttpPut]
    public virtual Task SetTextAsync(SetTextInput input)
    {
        return _service.SetTextAsync(input);
    }

    [HttpDelete]
    public virtual Task DeleteAsync([FromBody] TextDeleteInput input)
    {
        return _service.DeleteAsync(input);
    }

    [HttpDelete]
    [Route("restore-to-default")]
    [Obsolete("This interface will be removed in the next version. Please use DeleteAsync instead.")]
    public virtual Task RestoreToDefaultAsync([FromBody] RestoreDefaultTextInput input)
    {
        return _service.RestoreToDefaultAsync(input);
    }
}
