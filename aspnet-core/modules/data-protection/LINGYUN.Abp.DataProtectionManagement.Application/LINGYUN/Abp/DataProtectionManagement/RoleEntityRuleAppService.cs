﻿using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.DataProtectionManagement;

[Authorize(DataProtectionManagementPermissionNames.RoleEntityRule.Default)]
public class RoleEntityRuleAppService : DataProtectionManagementApplicationServiceBase, IRoleEntityRuleAppService
{
    protected IDistributedEventBus EventBus => LazyServiceProvider.LazyGetRequiredService<IDistributedEventBus>();

    private readonly IEntityTypeInfoRepository _entityTypeInfoRepository;
    private readonly IRoleEntityRuleRepository _roleEntityRuleRepository;

    public RoleEntityRuleAppService(
        IEntityTypeInfoRepository entityTypeInfoRepository, 
        IRoleEntityRuleRepository roleEntityRuleRepository)
    {
        _entityTypeInfoRepository = entityTypeInfoRepository;
        _roleEntityRuleRepository = roleEntityRuleRepository;
    }

    public async virtual Task<RoleEntityRuleDto> GetAsync(RoleEntityRuleGetInput input)
    {
        var entityTypeInfo = await _entityTypeInfoRepository.GetAsync(input.EntityTypeId);
        var entityRule = await _roleEntityRuleRepository.FindEntityRuleAsync(input.RoleName, entityTypeInfo.TypeFullName, input.Operation);
        return ObjectMapper.Map<RoleEntityRule, RoleEntityRuleDto>(entityRule);
    }

    [Authorize(DataProtectionManagementPermissionNames.RoleEntityRule.Create)]
    public async virtual Task<RoleEntityRuleDto> CreateAsync(RoleEntityRuleCreateDto input)
    {
        var entityTypeInfo = await _entityTypeInfoRepository.GetAsync(input.EntityTypeId);
        if (await _roleEntityRuleRepository.FindEntityRuleAsync(input.RoleName, entityTypeInfo.TypeFullName, input.Operation) != null)
        {
            throw new BusinessException(DataProtectionManagementErrorCodes.RoleEntityRule.DuplicateEntityRule)
                .WithData(nameof(RoleEntityRule.RoleName), input.RoleName)
                .WithData(nameof(EntityTypeInfo.Name), entityTypeInfo.Name)
                .WithData(nameof(EntityRuleBase.Operation), input.Operation.ToString());
        }

        var entityRule = new RoleEntityRule(
            GuidGenerator.Create(),
            input.RoleId,
            input.RoleName,
            entityTypeInfo.Id,
            entityTypeInfo.TypeFullName,
            input.Operation,
            input.AccessedProperties?.JoinAsString(","),
            input.FilterGroup,
            CurrentTenant.Id)
        {
            IsEnabled = input.IsEnabled,
        };

        entityRule = await _roleEntityRuleRepository.InsertAsync(entityRule);

        await EventBus.PublishAsync(
            new DataAccessResourceChangeEvent(
                entityRule.IsEnabled,
                new DataAccessResource(
                    RolePermissionValueProvider.ProviderName,
                    entityRule.RoleName,
                    entityRule.EntityTypeFullName,
                    entityRule.Operation,
                    entityRule.FilterGroup)
                {
                    AccessedProperties = input.AccessedProperties?.ToList()
                }));

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<RoleEntityRule, RoleEntityRuleDto>(entityRule);
    }

    [Authorize(DataProtectionManagementPermissionNames.RoleEntityRule.Update)]
    public async virtual Task<RoleEntityRuleDto> UpdateAsync(Guid id, RoleEntityRuleUpdateDto input)
    {
        var entityRule = await _roleEntityRuleRepository.GetAsync(id);
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

        entityRule = await _roleEntityRuleRepository.UpdateAsync(entityRule);

        await EventBus.PublishAsync(
            new DataAccessResourceChangeEvent(
                entityRule.IsEnabled,
                new DataAccessResource(
                    RolePermissionValueProvider.ProviderName,
                    entityRule.RoleName,
                    entityRule.EntityTypeFullName,
                    entityRule.Operation,
                    entityRule.FilterGroup)
                {
                    AccessedProperties = input.AccessedProperties?.ToList()
                }));

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<RoleEntityRule, RoleEntityRuleDto>(entityRule);
    }
}
