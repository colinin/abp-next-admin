using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.DataProtectionManagement;
public interface IRoleEntityRuleAppService : IApplicationService
{
    Task<RoleEntityRuleDto> GetAsync(RoleEntityRuleGetInput input);

    Task<RoleEntityRuleDto> CreateAsync(RoleEntityRuleCreateDto input);

    Task<RoleEntityRuleDto> UpdateAsync(Guid id, RoleEntityRuleUpdateDto input);
}
