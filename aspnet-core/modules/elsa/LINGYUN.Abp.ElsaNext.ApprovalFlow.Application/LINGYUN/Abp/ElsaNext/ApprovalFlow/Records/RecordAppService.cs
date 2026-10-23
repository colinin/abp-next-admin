using LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

[Authorize]
public class RecordAppService : ApprovalFlowAppServiceBase, IRecordAppService
{
    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<ListResultDto<RecordDto>> GetListAsync(Guid instanceId)
    {
        var records = await GetRecordDtosAsync(instanceId);
        return new ListResultDto<RecordDto>(records);
    }
}
