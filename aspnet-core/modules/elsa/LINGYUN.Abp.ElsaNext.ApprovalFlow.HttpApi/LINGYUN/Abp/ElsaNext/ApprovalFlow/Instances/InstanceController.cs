using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

[Authorize]
[Controller]
[Area("app")]
[RemoteService]
[Route("api/approval-flow/instances")]
public class InstanceController : AbpControllerBase, IInstanceAppService
{
    protected IInstanceAppService InstanceAppService { get; }

    public InstanceController(IInstanceAppService instanceAppService)
    {
        InstanceAppService = instanceAppService;
    }

    [HttpPost]
    public virtual Task<InstanceDto> StartAsync([FromBody] InstanceStartDto input)
    {
        return InstanceAppService.StartAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<InstanceDto> GetAsync(Guid id)
    {
        return InstanceAppService.GetAsync(id);
    }

    [HttpGet("{id}/detail")]
    public virtual Task<InstanceDetailDto> GetDetailAsync(Guid id)
    {
        return InstanceAppService.GetDetailAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<InstanceDto>> GetPagedListAsync([FromQuery] InstanceGetPagedListInput input)
    {
        return InstanceAppService.GetPagedListAsync(input);
    }

    [HttpPost("{id}/decide")]
    public virtual Task<InstanceDecisionResultDto> DecideAsync(Guid id, [FromBody] InstanceDecisionDto input)
    {
        return InstanceAppService.DecideAsync(id, input);
    }

    [HttpPost("{id}/return")]
    public virtual Task<InstanceDto> ReturnAsync(Guid id, [FromBody] InstanceReturnDto input)
    {
        return InstanceAppService.ReturnAsync(id, input);
    }

    [HttpPost("{id}/transfer")]
    public virtual Task<InstanceDto> TransferAsync(Guid id, [FromBody] InstanceTransferDto input)
    {
        return InstanceAppService.TransferAsync(id, input);
    }

    [HttpPost("{id}/add-approval")]
    public virtual Task<InstanceDto> AddApprovalAsync(Guid id, [FromBody] InstanceAddApprovalDto input)
    {
        return InstanceAppService.AddApprovalAsync(id, input);
    }

    [HttpPost("{id}/withdraw")]
    public virtual Task<InstanceDto> WithdrawAsync(Guid id, [FromBody] InstanceWithdrawDto input)
    {
        return InstanceAppService.WithdrawAsync(id, input);
    }
}
