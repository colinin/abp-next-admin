using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.DataProtectionManagement;
public interface IOrganizationUnitEntityRuleAppService : IApplicationService
{
    Task<OrganizationUnitEntityRuleDto> GetAsync(OrganizationUnitEntityRuleGetInput input);

    Task<OrganizationUnitEntityRuleDto> CreateAsync(OrganizationUnitEntityRuleCreateDto input);

    Task<OrganizationUnitEntityRuleDto> UpdateAsync(Guid id, OrganizationUnitEntityRuleUpdateDto input);
}
