using LINGYUN.Abp.FeatureManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

[Authorize(FeatureManagementPermissionNames.Definition.Default)]
public class FeatureDefinitionAppService : FeatureManagementAppServiceBase, IFeatureDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IFeatureDefinitionManager _featureDefinitionManager;

    private readonly IFeatureDefinitionRecordRepository _definitionRepository;
    private readonly IRepository<FeatureDefinitionRecord, Guid> _definitionBasicRepository;

    public FeatureDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        IFeatureDefinitionManager featureDefinitionManager,
        IFeatureDefinitionRecordRepository definitionRepository, 
        IRepository<FeatureDefinitionRecord, Guid> definitionBasicRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _featureDefinitionManager = featureDefinitionManager;
        _definitionRepository = definitionRepository;
        _definitionBasicRepository = definitionBasicRepository;
    }

    [Authorize(FeatureManagementPermissionNames.Definition.Create)]
    public async virtual Task<FeatureDefinitionDto> CreateAsync(FeatureDefinitionCreateDto input)
    {
        if (await _featureDefinitionManager.GetOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.Definition.AlreayNameExists)
                .WithData(nameof(FeatureDefinitionRecord.Name), input.Name);
        }

        var groupDefinition = await _featureDefinitionManager.GetGroupOrNullAsync(input.GroupName);
        if (groupDefinition == null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), input.GroupName);
        }

        var definitionRecord = new FeatureDefinitionRecord(
            GuidGenerator.Create(),
            groupDefinition.Name,
            input.Name,
            input.ParentName,
            input.DisplayName,
            input.Description,
            input.DefaultValue,
            input.IsVisibleToClients,
            input.IsAvailableToHost,
            valueType: input.ValueType);

        if (input.AllowedProviders.Any())
        {
            definitionRecord.AllowedProviders = input.AllowedProviders.JoinAsString(",");
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

    [Authorize(FeatureManagementPermissionNames.Definition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name);

        if (definitionRecord == null)
        {
            return;
        }

        await _definitionRepository.DeleteAsync(definitionRecord);
    }

    public async virtual Task<FeatureDefinitionDto> GetAsync(string name)
    {
        var definition = await _featureDefinitionManager.GetOrNullAsync(name);
        if (definition == null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(FeatureDefinitionRecord.Name), name);
        }

        var groupDefinition = await GetGroupDefinition(definition);

        var dto = await DefinitionToDto(groupDefinition, definition);

        return dto;
    }

    public async virtual Task<PagedResultDto<FeatureDefinitionDto>> GetListAsync(FeatureDefinitionGetListInput input)
    {
        var permissions = new List<FeatureDefinitionDto>();

        IReadOnlyList<FeatureDefinition> definitionPermissions;

        if (!input.GroupName.IsNullOrWhiteSpace())
        {
            var group = await _featureDefinitionManager.GetGroupOrNullAsync(input.GroupName);
            if (group == null)
            {
                return new PagedResultDto<FeatureDefinitionDto>(0, permissions);
            }

            definitionPermissions = group.GetFeaturesWithChildren();
        }
        else
        {
            definitionPermissions = await _featureDefinitionManager.GetAllAsync();
        }

        var definitionFilter = definitionPermissions.AsQueryable()
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) ||
                (x.Parent != null && x.Parent.Name.Contains(input.Filter)));

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(FeatureDefinitionRecord.Name);
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

        return new PagedResultDto<FeatureDefinitionDto>(filterDefinitionCount, permissions);
    }

    [Authorize(FeatureManagementPermissionNames.Definition.Update)]
    public async virtual Task<FeatureDefinitionDto> UpdateAsync(string name, FeatureDefinitionUpdateDto input)
    {
        var definition = await _featureDefinitionManager.GetAsync(name);
        var definitionRecord = await FindByNameAsync(name);

        if (definitionRecord == null)
        {
            var groupDefinition = await GetGroupDefinition(definition);
            definitionRecord = new FeatureDefinitionRecord(
                GuidGenerator.Create(),
                groupDefinition.Name,
                name,
                input.ParentName,
                input.DisplayName,
                input.Description,
                input.DefaultValue,
                input.IsVisibleToClients,
                input.IsAvailableToHost,
                valueType: input.ValueType);

            if (input.AllowedProviders.Any())
            {
                definitionRecord.AllowedProviders = input.AllowedProviders.JoinAsString(",");
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

            if (input.AllowedProviders.Any())
            {
                definitionRecord.AllowedProviders = input.AllowedProviders.JoinAsString(",");
            }

            if (!string.Equals(definitionRecord.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                definitionRecord.DisplayName = input.DisplayName;
            }

            if (!string.Equals(definitionRecord.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                definitionRecord.Description = input.Description;
            }

            if (!string.Equals(definitionRecord.DefaultValue, input.DefaultValue, StringComparison.InvariantCultureIgnoreCase))
            {
                definitionRecord.DefaultValue = input.DefaultValue;
            }

            if (!string.Equals(definitionRecord.ValueType, input.ValueType, StringComparison.InvariantCultureIgnoreCase))
            {
                definitionRecord.ValueType = input.ValueType;
            }

            definitionRecord.IsAvailableToHost = input.IsAvailableToHost;
            definitionRecord.IsVisibleToClients = input.IsVisibleToClients;

            definitionRecord = await _definitionBasicRepository.UpdateAsync(definitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await DefinitionRecordToDto(definitionRecord);

        return dto;
    }

    protected async virtual Task<FeatureDefinitionRecord> FindByNameAsync(string name)
    {
        var DefinitionFilter = await _definitionBasicRepository.GetQueryableAsync();

        var definitionRecord = await AsyncExecuter.FirstOrDefaultAsync(
            DefinitionFilter.Where(x => x.Name == name));
        
        return definitionRecord;
    }

    protected async virtual Task<FeatureGroupDefinition> GetGroupDefinition(FeatureDefinition definition)
    {
        var groups = await _featureDefinitionManager.GetGroupsAsync();

        foreach (var group in groups)
        {
            if (group.GetFeatureOrNull(definition.Name) != null)
            {
                return group;
            }
        }

        throw new BusinessException(FeatureManagementErrorCodes.Definition.FailedGetGroup)
            .WithData(nameof(FeatureDefinitionRecord.Name), definition.Name);
    }

    protected async virtual Task<FeatureDefinitionDto> DefinitionRecordToDto(FeatureDefinitionRecord definitionRecord)
    {
        var dto = new FeatureDefinitionDto
        {
            Name = definitionRecord.Name,
            GroupName = definitionRecord.GroupName,
            ParentName = definitionRecord.ParentName,
            IsAvailableToHost = definitionRecord.IsAvailableToHost,
            IsVisibleToClients = definitionRecord.IsVisibleToClients,
            DefaultValue = definitionRecord.DefaultValue,
            ValueType = definitionRecord.ValueType,
            AllowedProviders = definitionRecord.AllowedProviders.Split(',').ToList(),
            FormatedDescription = definitionRecord.Description,
            FormatedDisplayName = definitionRecord.DisplayName,
        };

        var displayName = _localizableStringSerializer.Deserialize(definitionRecord.DisplayName);
        dto.DisplayName = await displayName.LocalizeAsync(StringLocalizerFactory);

        if (!definitionRecord.Description.IsNullOrWhiteSpace())
        {
            var description = _localizableStringSerializer.Deserialize(definitionRecord.Description);
            dto.Description = await description.LocalizeAsync(StringLocalizerFactory);
        }

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }

    protected async virtual Task<FeatureDefinitionDto> DefinitionToDto(FeatureGroupDefinition groupDefinition, FeatureDefinition definition)
    {
        var dto = new FeatureDefinitionDto
        {
            Name = definition.Name,
            GroupName = groupDefinition.Name,
            ParentName = definition.Parent?.Name,
            DefaultValue = definition.DefaultValue,
            AllowedProviders = definition.AllowedProviders,
            IsAvailableToHost = definition.IsAvailableToHost,
            IsVisibleToClients = definition.IsVisibleToClients,
            ValueType = definition.ValueType.Name,
        };

        if (definition.DisplayName != null)
        {
            dto.DisplayName = await definition.DisplayName.LocalizeAsync(StringLocalizerFactory);
            dto.FormatedDisplayName = _localizableStringSerializer.Serialize(definition.DisplayName);
        }

        if (definition.Description != null)
        {
            dto.Description = await definition.Description.LocalizeAsync(StringLocalizerFactory);
            dto.FormatedDescription = _localizableStringSerializer.Serialize(definition.Description);
        }

        foreach (var property in definition.Properties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
