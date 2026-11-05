using System;
using System.Threading.Tasks;
using LINGYUN.Abp.OperationRateLimiting.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OperationRateLimiting;

public interface IOperationRateLimitingPolicyAppService : IApplicationService
{
    Task<OperationRateLimitingPolicyDto> GetAsync(Guid id);

    Task<OperationRateLimitingPolicyDto> GetByNameAsync(string name);

    Task<PagedResultDto<OperationRateLimitingPolicyDto>> GetListAsync(OperationRateLimitingPolicyGetListInput input);

    Task<OperationRateLimitingPolicyDto> CreateAsync(CreateOperationRateLimitingPolicyDto input);

    Task<OperationRateLimitingPolicyDto> UpdateAsync(Guid id, UpdateOperationRateLimitingPolicyDto input);

    Task DeleteAsync(Guid id);
}
