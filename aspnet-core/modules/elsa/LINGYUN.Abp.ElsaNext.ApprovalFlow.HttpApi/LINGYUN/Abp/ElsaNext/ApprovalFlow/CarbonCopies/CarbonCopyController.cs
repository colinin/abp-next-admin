using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;

[Authorize]
[Controller]
[Area("app")]
[RemoteService]
[Route("api/approval-flow/carbon-copies")]
public class CarbonCopyController : AbpControllerBase, ICarbonCopyAppService
{
    protected ICarbonCopyAppService CarbonCopyAppService { get; }

    public CarbonCopyController(ICarbonCopyAppService carbonCopyAppService)
    {
        CarbonCopyAppService = carbonCopyAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<CarbonCopyDto>> GetListAsync(
        [FromQuery] CarbonCopyGetPagedListInput input)
    {
        return CarbonCopyAppService.GetListAsync(input);
    }

    [HttpPost("{id}/read")]
    public virtual Task<CarbonCopyDto> MarkReadAsync(Guid id)
    {
        return CarbonCopyAppService.MarkReadAsync(id);
    }
}
