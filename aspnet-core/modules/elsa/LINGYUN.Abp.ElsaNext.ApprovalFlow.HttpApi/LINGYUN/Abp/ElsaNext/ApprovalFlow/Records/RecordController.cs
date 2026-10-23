using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

[Authorize]
[Controller]
[Area("app")]
[RemoteService]
[Route("api/approval-flow/instances/{instanceId}/records")]
public class RecordController : AbpControllerBase, IRecordAppService
{
    protected IRecordAppService RecordAppService { get; }

    public RecordController(IRecordAppService recordAppService)
    {
        RecordAppService = recordAppService;
    }

    [HttpGet]
    public virtual Task<ListResultDto<RecordDto>> GetListAsync(Guid instanceId)
    {
        return RecordAppService.GetListAsync(instanceId);
    }
}
