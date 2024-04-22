using LINGYUN.Abp.PermissionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

[Authorize(PermissionManagementPermissionNames.Definition.Default)]
public class PermissionDefinitionAppService : PermissionManagementAppServiceBase, IPermissionDefinitionAppService
{
    private readonly ISimpleStateCheckerSerializer _simpleStateCheckerSerializer;
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;
    private readonly IStaticPermissionDefinitionStore _staticPermissionDefinitionStore;
    private readonly IDynamicPermissionDefinitionStore _dynamicPermissionDefinitionStore;
    private readonly IPermissionDefinitionRecordRepository _definitionRepository;
    private readonly IRepository<PermissionDefinitionRecord, Guid> _definitionBasicRepository;

    public PermissionDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer, 
        IPermissionDefinitionManager permissionDefinitionManager,
        IStaticPermissionDefinitionStore staticPermissionDefinitionStore,
        IDynamicPermissionDefinitionStore dynamicPermissionDefinitionStore,
        ISimpleStateCheckerSerializer simpleStateCheckerSerializer,
        IPermissionDefinitionRecordRepository definitionRepository, 
        IRepository<PermissionDefinitionRecord, Guid> definitionBasicRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _permissionDefinitionManager = permissionDefinitionManager;
        _staticPermissionDefinitionStore = staticPermissionDefinitionStore;
        _dynamicPermissionDefinitionStore = dynamicPermissionDefinitionStore;
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
        var staticGroups = await _staticPermissionDefinitionStore.GetGroupsAsync();
        if (staticGroups.Any(g => g.Name == input.GroupName))
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
                .WithData(nameof(PermissionDefinitionRecord.Name), input.GroupName);
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

        await UpdateByInput(definitionRecord, input);

        definitionRecord = await _definitionRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name);

        if (definitionRecord != null)
        {
            await _definitionRepository.DeleteAsync(definitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<PermissionDefinitionDto> GetAsync(string name)
    {
        var definition = await _staticPermissionDefinitionStore.GetOrNullAsync(name);
        if (definition != null)
        {
            return DefinitionToDto(await GetGroupDefinition(definition), definition, true);
        }
        definition = await _dynamicPermissionDefinitionStore.GetOrNullAsync(name);
        return DefinitionToDto(await GetGroupDefinition(definition), definition);
    }

    public async virtual Task<ListResultDto<PermissionDefinitionDto>> GetListAsync(PermissionDefinitionGetListInput input)
    {
        var permissionDtoList = new List<PermissionDefinitionDto>();

        var staticPermissions = new List<PermissionDefinition>();

        var staticGroups = await _staticPermissionDefinitionStore.GetGroupsAsync();
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();
        foreach (var group in staticGroups.WhereIf(!input.GroupName.IsNullOrWhiteSpace(), x => x.Name == input.GroupName))
        {
            var permissions = group.GetPermissionsWithChildren();
            staticPermissions.AddRange(permissions);
            permissionDtoList.AddRange(permissions.Select(f => DefinitionToDto(group, f, true)));
        }
        var staticPermissionNames = staticPermissions
            .Select(p => p.Name)
            .ToImmutableHashSet();
        var dynamicGroups = await _dynamicPermissionDefinitionStore.GetGroupsAsync();
        foreach (var group in dynamicGroups
            .Where(d => !staticGroupNames.Contains(d.Name))
            .WhereIf(!input.GroupName.IsNullOrWhiteSpace(), x => x.Name == input.GroupName))
        {
            var permissions = group.GetPermissionsWithChildren();
            permissionDtoList.AddRange(permissions
                .Where(d => !staticPermissionNames.Contains(d.Name))
                .Select(f => DefinitionToDto(group, f)));
        }

        return new ListResultDto<PermissionDefinitionDto>(permissionDtoList
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter))
            .ToList());
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Update)]
    public async virtual Task<PermissionDefinitionDto> UpdateAsync(string name, PermissionDefinitionUpdateDto input)
    {
        if (await _staticPermissionDefinitionStore.GetOrNullAsync(name) != null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.Definition.StaticPermissionNotAllowedChanged)
              .WithData("Name", name);
        }

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

            await UpdateByInput(definitionRecord, input);

            definitionRecord = await _definitionBasicRepository.InsertAsync(definitionRecord);
        }
        else
        {
            await UpdateByInput(definitionRecord, input);

            definitionRecord = await _definitionBasicRepository.UpdateAsync(definitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected async virtual Task UpdateByInput(PermissionDefinitionRecord record, PermissionDefinitionCreateOrUpdateDto input)
    {
        record.IsEnabled = input.IsEnabled;
        record.MultiTenancySide = input.MultiTenancySide;
        if (!string.Equals(record.ParentName, input.ParentName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.ParentName = input.ParentName;
        }
        if (!string.Equals(record.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DisplayName = input.DisplayName;
        }
        string providers = null;
        if (!input.Providers.IsNullOrEmpty())
        {
            providers = input.Providers.JoinAsString(",");
        }
        if (!string.Equals(record.Providers, providers, StringComparison.InvariantCultureIgnoreCase))
        {
            record.Providers = providers;
        }
        
        record.ExtraProperties.Clear();
        foreach (var property in input.ExtraProperties)
        {
            record.SetProperty(property.Key, property.Value);
        }

        try
        {
            if (!string.Equals(record.StateCheckers, input.StateCheckers, StringComparison.InvariantCultureIgnoreCase))
            {
                // 校验格式
                var permissionDefinition = await _staticPermissionDefinitionStore.GetOrNullAsync(PermissionManagementPermissionNames.Definition.Default);
                var _ = _simpleStateCheckerSerializer.DeserializeArray(input.StateCheckers, permissionDefinition);

                record.StateCheckers = input.StateCheckers;
            }
        }
        catch
        {
            throw new AbpValidationException(
                new List<ValidationResult>
                {
                    new ValidationResult(
                        L["The field {0} is invalid", L["DisplayName:StateCheckers"]],
                        new string[1] { nameof(input.StateCheckers) })
                });
        }
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

    protected virtual PermissionDefinitionDto DefinitionRecordToDto(PermissionDefinitionRecord definitionRecord)
    {
        var dto = new PermissionDefinitionDto
        {
            IsStatic = false,
            Name = definitionRecord.Name,
            GroupName = definitionRecord.GroupName,
            ParentName = definitionRecord.ParentName,
            IsEnabled = definitionRecord.IsEnabled,
            DisplayName = definitionRecord.DisplayName,
            Providers = definitionRecord.Providers?.Split(',').ToList(),
            StateCheckers = definitionRecord.StateCheckers,
            MultiTenancySide = definitionRecord.MultiTenancySide,
            ExtraProperties = new ExtraPropertyDictionary(),
        };

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }

    protected virtual PermissionDefinitionDto DefinitionToDto(PermissionGroupDefinition groupDefinition, PermissionDefinition definition, bool isStatic = false)
    {
        var dto = new PermissionDefinitionDto
        {
            IsStatic = isStatic,
            Name = definition.Name,
            GroupName = groupDefinition.Name,
            ParentName = definition.Parent?.Name,
            IsEnabled = definition.IsEnabled,
            Providers = definition.Providers,
            MultiTenancySide = definition.MultiTenancySide,
            DisplayName = _localizableStringSerializer.Serialize(definition.DisplayName),
            ExtraProperties = new ExtraPropertyDictionary(),
        };

        if (definition.StateCheckers.Any())
        {
            dto.StateCheckers = _simpleStateCheckerSerializer.Serialize(definition.StateCheckers);
        }

        foreach (var property in definition.Properties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
