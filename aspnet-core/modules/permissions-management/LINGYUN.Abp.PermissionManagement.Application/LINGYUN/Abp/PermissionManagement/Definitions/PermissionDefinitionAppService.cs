using LINGYUN.Abp.PermissionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

[Authorize(PermissionManagementPermissionNames.Definition.Default)]
public class PermissionDefinitionAppService : PermissionManagementAppServiceBase, IPermissionDefinitionAppService
{
    private readonly ISimpleStateCheckerSerializer _simpleStateCheckerSerializer;
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    private readonly IPermissionDefinitionRecordRepository _definitionRepository;
    private readonly IRepository<PermissionDefinitionRecord, Guid> _definitionBasicRepository;

    public PermissionDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer, 
        IPermissionDefinitionManager permissionDefinitionManager,
        ISimpleStateCheckerSerializer simpleStateCheckerSerializer,
        IPermissionDefinitionRecordRepository definitionRepository, 
        IRepository<PermissionDefinitionRecord, Guid> definitionBasicRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _permissionDefinitionManager = permissionDefinitionManager;
        _simpleStateCheckerSerializer = simpleStateCheckerSerializer;
        _definitionRepository = definitionRepository;
        _definitionBasicRepository = definitionBasicRepository;
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Create)]
    public async virtual Task<PermissionDefinitionDto> CreateAsync(PermissionDefinitionCreateDto input)
    {
        if (await _permissionDefinitionManager.GetOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.Definition.AlreayNameExists)
                .WithData(nameof(PermissionDefinitionRecord.Name), input.Name);
        }

        var groupDefinition = await _permissionDefinitionManager.GetGroupOrNullAsync(input.GroupName);
        if (groupDefinition == null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), input.GroupName);
        }

        var definitionRecord = new PermissionDefinitionRecord(
            GuidGenerator.Create(),
            groupDefinition.Name,
            input.Name,
            input.ParentName,
            input.DisplayName,
            input.IsEnabled);

        if (input.MultiTenancySide.HasValue)
        {
            definitionRecord.MultiTenancySide = input.MultiTenancySide.Value;
        }

        if (input.Providers.Any())
        {
            definitionRecord.Providers = input.Providers.JoinAsString(",");
        }

        if (input.StateCheckers.Any())
        {
            definitionRecord.StateCheckers = input.StateCheckers.JoinAsString(",");
        }

        foreach (var property in input.ExtraProperties)
        {
            definitionRecord.SetProperty(property.Key, property.Value);
        }

        await _definitionRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await DefinitionRecordToDto(definitionRecord);

        return dto;
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name);

        if (definitionRecord == null)
        {
            return;
        }

        await _definitionRepository.DeleteAsync(definitionRecord);
    }

    public async virtual Task<PermissionDefinitionDto> GetAsync(string name)
    {
        var definition = await _permissionDefinitionManager.GetOrNullAsync(name);
        if (definition == null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(PermissionDefinitionRecord.Name), name);
        }

        var groupDefinition = await GetGroupDefinition(definition);

        var dto = await DefinitionToDto(groupDefinition, definition);

        return dto;
    }

    public async virtual Task<PagedResultDto<PermissionDefinitionDto>> GetListAsync(PermissionDefinitionGetListInput input)
    {
        var permissions = new List<PermissionDefinitionDto>();

        IReadOnlyList<PermissionDefinition> definitionPermissions;

        if (!input.GroupName.IsNullOrWhiteSpace())
        {
            var group = await _permissionDefinitionManager.GetGroupOrNullAsync(input.GroupName);
            if (group == null)
            {
                return new PagedResultDto<PermissionDefinitionDto>(0, permissions);
            }

            definitionPermissions = group.GetPermissionsWithChildren();
        }
        else
        {
            definitionPermissions = await _permissionDefinitionManager.GetPermissionsAsync();
        }

        var definitionFilter = definitionPermissions.AsQueryable()
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) ||
                (x.Parent != null && x.Parent.Name.Contains(input.Filter)));

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(PermissionDefinitionRecord.Name);
        }

        var filterDefinitionCount = definitionFilter.Count();
        var filterDefinitions = definitionFilter
            .OrderBy(sorting)
            .PageBy(input.SkipCount, input.MaxResultCount);

        foreach (var definition in filterDefinitions)
        {
            var groupDefinition = await GetGroupDefinition(definition);
            var Dto = await DefinitionToDto(groupDefinition, definition);

            permissions.Add(Dto);
        }

        return new PagedResultDto<PermissionDefinitionDto>(filterDefinitionCount, permissions);
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Update)]
    public async virtual Task<PermissionDefinitionDto> UpdateAsync(string name, PermissionDefinitionUpdateDto input)
    {
        var definition = await _permissionDefinitionManager.GetOrNullAsync(name);
        var definitionRecord = await FindByNameAsync(name);

        if (definitionRecord == null)
        {
            var groupDefinition = await GetGroupDefinition(definition);
            definitionRecord = new PermissionDefinitionRecord(
                GuidGenerator.Create(),
                groupDefinition.Name,
                name,
                input.ParentName,
                input.DisplayName,
                input.IsEnabled);

            if (input.MultiTenancySide.HasValue)
            {
                definitionRecord.MultiTenancySide = input.MultiTenancySide.Value;
            }

            if (input.Providers.Any())
            {
                definitionRecord.Providers = input.Providers.JoinAsString(",");
            }

            if (input.StateCheckers.Any())
            {
                definitionRecord.StateCheckers = input.StateCheckers.JoinAsString(",");
            }

            foreach (var property in input.ExtraProperties)
            {
                definitionRecord.SetProperty(property.Key, property.Value);
            }

            definitionRecord = await _definitionBasicRepository.InsertAsync(definitionRecord);
        }
        else
        {
            definitionRecord.ExtraProperties.Clear();
            foreach (var property in input.ExtraProperties)
            {
                definitionRecord.SetProperty(property.Key, property.Value);
            }

            if (input.MultiTenancySide.HasValue)
            {
                definitionRecord.MultiTenancySide = input.MultiTenancySide.Value;
            }

            if (input.Providers.Any())
            {
                definitionRecord.Providers = input.Providers.JoinAsString(",");
            }

            if (input.StateCheckers.Any())
            {
                definitionRecord.StateCheckers = input.StateCheckers.JoinAsString(",");
            }

            if (!string.Equals(definitionRecord.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                definitionRecord.DisplayName = input.DisplayName;
            }

            definitionRecord = await _definitionBasicRepository.UpdateAsync(definitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await DefinitionRecordToDto(definitionRecord);

        return dto;
    }

    protected async virtual Task<PermissionDefinitionRecord> FindByNameAsync(string name)
    {
        var DefinitionFilter = await _definitionBasicRepository.GetQueryableAsync();

        var definitionRecord = await AsyncExecuter.FirstOrDefaultAsync(
            DefinitionFilter.Where(x => x.Name == name));

        return definitionRecord;
    }

    protected async virtual Task<PermissionGroupDefinition> GetGroupDefinition(PermissionDefinition definition)
    {
        var groups = await _permissionDefinitionManager.GetGroupsAsync();

        foreach (var group in groups)
        {
            if (group.GetPermissionOrNull(definition.Name) != null)
            {
                return group;
            }
        }

        throw new BusinessException(PermissionManagementErrorCodes.Definition.FailedGetGroup)
            .WithData(nameof(PermissionDefinitionRecord.Name), definition.Name);
    }

    protected async virtual Task<PermissionDefinitionDto> DefinitionRecordToDto(PermissionDefinitionRecord definitionRecord)
    {
        var dto = new PermissionDefinitionDto
        {
            Name = definitionRecord.Name,
            GroupName = definitionRecord.GroupName,
            ParentName = definitionRecord.ParentName,
            IsEnabled = definitionRecord.IsEnabled,
            FormatedDisplayName = definitionRecord.DisplayName,
            Providers = definitionRecord.Providers.Split(',').ToList(),
            StateCheckers = definitionRecord.StateCheckers.Split(',').ToList(),
            MultiTenancySide = definitionRecord.MultiTenancySide,
        };

        var displayName = _localizableStringSerializer.Deserialize(definitionRecord.DisplayName);
        dto.DisplayName = await displayName.LocalizeAsync(StringLocalizerFactory);

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }

    protected async virtual Task<PermissionDefinitionDto> DefinitionToDto(PermissionGroupDefinition groupDefinition, PermissionDefinition definition)
    {
        var dto = new PermissionDefinitionDto
        {
            Name = definition.Name,
            GroupName = groupDefinition.Name,
            ParentName = definition.Parent?.Name,
            IsEnabled = definition.IsEnabled,
            Providers = definition.Providers,
            MultiTenancySide = definition.MultiTenancySide,
        };

        if (definition.StateCheckers.Any())
        {
            var stateCheckers = _simpleStateCheckerSerializer.Serialize(definition.StateCheckers);
            dto.StateCheckers = stateCheckers.Split(',').ToList();
        }

        if (definition.DisplayName != null)
        {
            dto.DisplayName = await definition.DisplayName.LocalizeAsync(StringLocalizerFactory);
            dto.FormatedDisplayName = _localizableStringSerializer.Serialize(definition.DisplayName);
        }

        foreach (var property in definition.Properties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
