using LINGYUN.Abp.Authorization.Permissions;
using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.DataProtectionManagement;

[Authorize(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Default)]
public class OrganizationUnitEntityRuleAppService : DataProtectionManagementApplicationServiceBase, IOrganizationUnitEntityRuleAppService
{
    protected IDistributedEventBus EventBus => LazyServiceProvider.LazyGetRequiredService<IDistributedEventBus>();

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
                .WithData(nameof(EntityTypeInfo.Name), entityTypeInfo.Name)
                .WithData(nameof(EntityRuleBase.Operation), input.Operation.ToString());
        }

        var entityRule = new OrganizationUnitEntityRule(
            GuidGenerator.Create(),
            input.OrgId,
            input.OrgCode,
            entityTypeInfo.Id,
            entityTypeInfo.TypeFullName,
            input.Operation,
            input.AccessedProperties?.JoinAsString(","),
            input.FilterGroup,
            CurrentTenant.Id)
        {
            IsEnabled = input.IsEnabled,
        };

        entityRule = await _organizationUnitEntityRuleRepository.InsertAsync(entityRule);

        await EventBus.PublishAsync(
            new DataAccessResourceChangeEvent(
                entityRule.IsEnabled,
                new DataAccessResource(
                    OrganizationUnitPermissionValueProvider.ProviderName,
                    entityRule.OrgCode,
                    entityRule.EntityTypeFullName,
                    entityRule.Operation,
                    entityRule.FilterGroup)
                {
                    AccessedProperties = input.AccessedProperties.ToList()
                }));

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
        var allowPropertites = input.AccessedProperties?.JoinAsString(",");
        if (!string.Equals(entityRule.AccessedProperties, allowPropertites, StringComparison.InvariantCultureIgnoreCase))
        {
            entityRule.AccessedProperties = allowPropertites;
        }
        entityRule.FilterGroup = input.FilterGroup;

        entityRule = await _organizationUnitEntityRuleRepository.UpdateAsync(entityRule);

        await EventBus.PublishAsync(
            new DataAccessResourceChangeEvent(
                entityRule.IsEnabled,
                new DataAccessResource(
                    OrganizationUnitPermissionValueProvider.ProviderName,
                    entityRule.OrgCode,
                    entityRule.EntityTypeFullName,
                    entityRule.Operation,
                    entityRule.FilterGroup)
                {
                    AccessedProperties = input.AccessedProperties.ToList()
                }));

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<OrganizationUnitEntityRule, OrganizationUnitEntityRuleDto>(entityRule);
    }
}
