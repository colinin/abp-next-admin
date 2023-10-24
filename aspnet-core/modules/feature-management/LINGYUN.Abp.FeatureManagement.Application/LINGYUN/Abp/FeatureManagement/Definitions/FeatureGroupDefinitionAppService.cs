using LINGYUN.Abp.FeatureManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

[Authorize(FeatureManagementPermissionNames.GroupDefinition.Default)]
public class FeatureGroupDefinitionAppService : FeatureManagementAppServiceBase, IFeatureGroupDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IFeatureDefinitionManager _featureDefinitionManager;
    private readonly IStaticFeatureDefinitionStore _staticFeatureDefinitionStore;
    private readonly IDynamicFeatureDefinitionStore _dynamicFeatureDefinitionStore;
    private readonly IFeatureGroupDefinitionRecordRepository _groupDefinitionRepository;
    private readonly IRepository<FeatureGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public FeatureGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        IFeatureDefinitionManager featureDefinitionManager, 
        IStaticFeatureDefinitionStore featureDefinitionStore,
        IDynamicFeatureDefinitionStore dynamicFeatureDefinitionStore,
        IFeatureGroupDefinitionRecordRepository groupDefinitionRepository, 
        IRepository<FeatureGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _featureDefinitionManager = featureDefinitionManager;
        _staticFeatureDefinitionStore = featureDefinitionStore;
        _dynamicFeatureDefinitionStore = dynamicFeatureDefinitionStore;
        _groupDefinitionRepository = groupDefinitionRepository;
        _groupDefinitionBasicRepository = groupDefinitionBasicRepository;
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Create)]
    public async virtual Task<FeatureGroupDefinitionDto> CreateAsync(FeatureGroupDefinitionCreateDto input)
    {
        if (await _featureDefinitionManager.GetGroupOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.AlreayNameExists)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), input.Name);
        }
        var groupDefinitionRecord = await _groupDefinitionBasicRepository.FindAsync(x => x.Name == input.Name);
        if (groupDefinitionRecord != null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.AlreayNameExists)
               .WithData("Name", input.Name);
        }

        groupDefinitionRecord = new FeatureGroupDefinitionRecord(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName);

        UpdateByInput(groupDefinitionRecord, input);

        await _groupDefinitionRepository.InsertAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return GroupDefinitionRecordToDto(groupDefinitionRecord);
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord != null)
        {
            await _groupDefinitionRepository.DeleteAsync(groupDefinitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<FeatureGroupDefinitionDto> GetAsync(string name)
    {
        var staticGroups = await _staticFeatureDefinitionStore.GetGroupsAsync();
        var groupDefinition = staticGroups.FirstOrDefault(x => x.Name == name);
        if (groupDefinition != null)
        {
            return GroupDefinitionToDto(groupDefinition, true);
        }

        var dynamicGroups = await _dynamicFeatureDefinitionStore.GetGroupsAsync();

        groupDefinition = dynamicGroups.FirstOrDefault(x => x.Name == name);
        if (groupDefinition == null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), name);
        }

        return GroupDefinitionToDto(groupDefinition);
    }

    public async virtual Task<ListResultDto<FeatureGroupDefinitionDto>> GetListAsync(FeatureGroupDefinitionGetListInput input)
    {
        var groupDtoList = new List<FeatureGroupDefinitionDto>();

        var staticGroups = await _staticFeatureDefinitionStore.GetGroupsAsync();
        var staticGroupsNames = staticGroups
           .Select(p => p.Name)
           .ToImmutableHashSet();
        groupDtoList.AddRange(staticGroups.Select(d => GroupDefinitionToDto(d, true)));

        var dynamicGroups = await _dynamicFeatureDefinitionStore.GetGroupsAsync();
        groupDtoList.AddRange(dynamicGroups
           .Where(d => !staticGroupsNames.Contains(d.Name))
           .Select(d => GroupDefinitionToDto(d)));

        return new ListResultDto<FeatureGroupDefinitionDto>(
            groupDtoList
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter))
                .ToList());
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Update)]
    public async virtual Task<FeatureGroupDefinitionDto> UpdateAsync(string name, FeatureGroupDefinitionUpdateDto input)
    {
        var staticGroups = await _staticFeatureDefinitionStore.GetGroupsAsync();
        if (staticGroups.FirstOrDefault(x => x.Name == name) != null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
              .WithData("Name", name);
        }

        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord == null)
        {
            groupDefinitionRecord = new FeatureGroupDefinitionRecord(
                GuidGenerator.Create(),
                name,
                input.DisplayName);
            UpdateByInput(groupDefinitionRecord, input);

            groupDefinitionRecord = await _groupDefinitionBasicRepository.InsertAsync(groupDefinitionRecord);
        }
        else
        {
            UpdateByInput(groupDefinitionRecord, input);
            groupDefinitionRecord = await _groupDefinitionBasicRepository.UpdateAsync(groupDefinitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return GroupDefinitionRecordToDto(groupDefinitionRecord);
    }

    protected virtual void UpdateByInput(FeatureGroupDefinitionRecord record, FeatureGroupDefinitionCreateOrUpdateDto input)
    {
        record.ExtraProperties.Clear();
        foreach (var property in input.ExtraProperties)
        {
            record.SetProperty(property.Key, property.Value);
        }
        if (!string.Equals(record.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DisplayName = input.DisplayName;
        }
    }

    protected async virtual Task<FeatureGroupDefinitionRecord> FindByNameAsync(string name)
    {
        var groupDefinitionRecord = await _groupDefinitionBasicRepository.FindAsync(x => x.Name == name);

        return groupDefinitionRecord;
    }

    protected virtual FeatureGroupDefinitionDto GroupDefinitionRecordToDto(FeatureGroupDefinitionRecord groupDefinitionRecord)
    {
        var groupDto = new FeatureGroupDefinitionDto
        {
            Name = groupDefinitionRecord.Name,
            DisplayName = groupDefinitionRecord.DisplayName,
            ExtraProperties = new ExtraPropertyDictionary(),
        };

        foreach (var property in groupDefinitionRecord.ExtraProperties)
        {
            groupDto.SetProperty(property.Key, property.Value);
        }

        return groupDto;
    }

    protected virtual FeatureGroupDefinitionDto GroupDefinitionToDto(FeatureGroupDefinition groupDefinition, bool isStatic = false)
    {
        var groupDto = new FeatureGroupDefinitionDto
        {
            IsStatic = isStatic,
            Name = groupDefinition.Name,
            DisplayName = _localizableStringSerializer.Serialize(groupDefinition.DisplayName),
            ExtraProperties = new ExtraPropertyDictionary(),
        };

        foreach (var property in groupDefinition.Properties)
        {
            groupDto.SetProperty(property.Key, property.Value);
        }

        return groupDto;
    }
}
