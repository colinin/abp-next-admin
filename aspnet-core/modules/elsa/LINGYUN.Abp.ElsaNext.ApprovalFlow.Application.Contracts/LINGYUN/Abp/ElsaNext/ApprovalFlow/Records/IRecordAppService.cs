using System;
using System.Threading.Tasks;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

public interface IRecordAppService : IApplicationService
{
    /// <summary>
    /// 获取审批实例的流转记录列表
    /// </summary>
    /// <param name="instanceId">审批实例标识</param>
    Task<ListResultDto<RecordDto>> GetListAsync(Guid instanceId);
}
