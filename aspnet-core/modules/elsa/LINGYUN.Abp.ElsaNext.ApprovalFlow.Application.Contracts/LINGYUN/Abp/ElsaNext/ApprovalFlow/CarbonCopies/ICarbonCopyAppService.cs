using System;
using System.Threading.Tasks;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;

public interface ICarbonCopyAppService : IApplicationService
{
    Task<PagedResultDto<CarbonCopyDto>> GetListAsync(CarbonCopyGetPagedListInput input);

    Task<CarbonCopyDto> MarkReadAsync(Guid id);
}
