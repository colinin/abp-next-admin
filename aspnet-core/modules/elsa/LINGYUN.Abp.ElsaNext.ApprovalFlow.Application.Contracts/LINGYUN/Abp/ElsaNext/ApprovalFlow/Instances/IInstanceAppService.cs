using System;
using System.Threading.Tasks;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public interface IInstanceAppService : IApplicationService
{
    Task<InstanceDto> StartAsync(InstanceStartDto input);

    Task<InstanceDto> GetAsync(Guid id);

    Task<InstanceDetailDto> GetDetailAsync(Guid id);

    Task<PagedResultDto<InstanceDto>> GetPagedListAsync(InstanceGetPagedListInput input);

    Task<InstanceDecisionResultDto> DecideAsync(Guid id, InstanceDecisionDto input);

    Task<InstanceDto> ReturnAsync(Guid id, InstanceReturnDto input);

    Task<InstanceDto> TransferAsync(Guid id, InstanceTransferDto input);

    Task<InstanceDto> AddApprovalAsync(Guid id, InstanceAddApprovalDto input);

    Task<InstanceDto> WithdrawAsync(Guid id, InstanceWithdrawDto input);
}
