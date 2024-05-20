using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.DataProtectionManagement;

[Authorize(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Default)]
public class OrganizationUnitEntityRuleAppService : DataProtectionManagementApplicationServiceBase, IOrganizationUnitEntityRuleAppService
{
    private readonly IEntityTypeInfoRepository _entityTypeInfoRepository;
    private readonly IOrganizationUnitEntityRuleRepository _organizationUnitEntityRuleRepository;

    public OrganizationUnitEntityRuleAppService(
        IEntityTypeInfoRepository entityTypeInfoRepository, 
        IOrganizationUnitEntityRuleRepository organizationUnitEntityRuleRepository)
    {
        _entityTypeInfoRepository = entityTypeInfoRepository;
        _organizationUnitEntityRuleRepository = organizationUnitEntityRuleRepository;
    }

    public async virtual Task<OrganizationUnitEntityRuleDto> GetAsync(OrganizationUnitEntityRuleGetInput input)
    {
        var entityTypeInfo = await _entityTypeInfoRepository.GetAsync(input.EntityTypeId);
        var entityRule = await _organizationUnitEntityRuleRepository.FindEntityRuleAsync(input.OrgCode, entityTypeInfo.TypeFullName, input.Operation);
        return ObjectMapper.Map<OrganizationUnitEntityRule, OrganizationUnitEntityRuleDto>(entityRule);
    }

    [Authorize(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Create)]
    public async virtual Task<OrganizationUnitEntityRuleDto> CreateAsync(OrganizationUnitEntityRuleCreateDto input)
    {
        var entityTypeInfo = await _entityTypeInfoRepository.GetAsync(input.EntityTypeId);
        if (await _organizationUnitEntityRuleRepository.FindEntityRuleAsync(input.OrgCode, entityTypeInfo.TypeFullName, input.Operation) != null)
        {
            throw new BusinessException(DataProtectionManagementErrorCodes.OrganizationUnitEntityRule.DuplicateEntityRule)
                .WithData(nameof(OrganizationUnitEntityRule.OrgCode), input.OrgCode)
                .WithData(nameof(EntityTypeInfo.DisplayName), entityTypeInfo.DisplayName)
                .WithData(nameof(EntityRuleBase.Operation), input.Operation);
        }

        var entityRule = new OrganizationUnitEntityRule(
            GuidGenerator.Create(),
            input.OrgId,
            input.OrgCode,
            entityTypeInfo.Id,
            entityTypeInfo.TypeFullName,
            input.Operation,
            input.AllowProperties?.JoinAsString(","),
            input.FilterGroup,
            CurrentTenant.Id)
        {
            IsEnabled = input.IsEnabled,
        };

        entityRule = await _organizationUnitEntityRuleRepository.InsertAsync(entityRule);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<OrganizationUnitEntityRule, OrganizationUnitEntityRuleDto>(entityRule);
    }

    [Authorize(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Update)]
    public async virtual Task<OrganizationUnitEntityRuleDto> UpdateAsync(Guid id, OrganizationUnitEntityRuleUpdateDto input)
    {
        var entityRule = await _organizationUnitEntityRuleRepository.GetAsync(id);
        if (entityRule.IsEnabled != input.IsEnabled)
        {
            entityRule.IsEnabled = input.IsEnabled;
        }
        if (entityRule.Operation != input.Operation)
        {
            entityRule.Operation = input.Operation;
        }
        var allowPropertites = input.AllowProperties?.JoinAsString(",");
        if (!string.Equals(entityRule.AllowProperties, allowPropertites, StringComparison.InvariantCultureIgnoreCase))
        {
            entityRule.AllowProperties = allowPropertites;
        }

        entityRule = await _organizationUnitEntityRuleRepository.UpdateAsync(entityRule);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<OrganizationUnitEntityRule, OrganizationUnitEntityRuleDto>(entityRule);
    }
}
