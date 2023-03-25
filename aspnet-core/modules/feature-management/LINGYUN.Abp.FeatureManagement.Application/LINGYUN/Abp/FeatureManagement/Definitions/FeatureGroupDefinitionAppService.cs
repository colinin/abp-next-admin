using LINGYUN.Abp.FeatureManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
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

[Authorize(FeatureManagementPermissionNames.GroupDefinition.Default)]
public class FeatureGroupDefinitionAppService : FeatureManagementAppServiceBase, IFeatureGroupDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IFeatureDefinitionManager _featureDefinitionManager;

    private readonly IFeatureGroupDefinitionRecordRepository _groupDefinitionRepository;
    private readonly IRepository<FeatureGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public FeatureGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        IFeatureDefinitionManager featureDefinitionManager, 
        IFeatureGroupDefinitionRecordRepository groupDefinitionRepository, 
        IRepository<FeatureGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _featureDefinitionManager = featureDefinitionManager;
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

        var groupDefinitionRecord = new FeatureGroupDefinitionRecord(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName);

        foreach (var property in input.ExtraProperties)
        {
            groupDefinitionRecord.SetProperty(property.Key, property.Value);
        }

        await _groupDefinitionRepository.InsertAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await GroupDefinitionRecordToDto(groupDefinitionRecord);

        return dto;
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord == null)
        {
            return;
        }

        await _groupDefinitionRepository.DeleteAsync(groupDefinitionRecord);
    }

    public async virtual Task<FeatureGroupDefinitionDto> GetAsync(string name)
    {
        var groupDefinition = await _featureDefinitionManager.GetGroupOrNullAsync(name);
        if (groupDefinition == null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), name);
        }

        var dto = await GroupDefinitionToDto(groupDefinition);

        return dto;
    }

    public async virtual Task<PagedResultDto<FeatureGroupDefinitionDto>> GetListAsync(FeatureGroupDefinitionGetListInput input)
    {
        var groups = new List<FeatureGroupDefinitionDto>();

        var groupDefinitions = await _featureDefinitionManager.GetGroupsAsync();

        var groupDefinitionFilter = groupDefinitions.AsQueryable()
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter));

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(FeatureDefinitionRecord.Name);
        }

        var filterGroupDefinitionCount = groupDefinitionFilter.Count();
        var filterGroupDefinitions = groupDefinitionFilter
            .OrderBy(sorting)
            .PageBy(input.SkipCount, input.MaxResultCount);

        foreach (var groupDefinition in filterGroupDefinitions)
        {
            var groupDto = await GroupDefinitionToDto(groupDefinition);

            groups.Add(groupDto);
        }

        return new PagedResultDto<FeatureGroupDefinitionDto>(filterGroupDefinitionCount, groups);
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Update)]
    public async virtual Task<FeatureGroupDefinitionDto> UpdateAsync(string name, FeatureGroupDefinitionUpdateDto input)
    {
        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord == null)
        {
            groupDefinitionRecord = new FeatureGroupDefinitionRecord(
                GuidGenerator.Create(),
                name,
                input.DisplayName);

            foreach (var property in input.ExtraProperties)
            {
                groupDefinitionRecord.SetProperty(property.Key, property.Value);
            }

            groupDefinitionRecord = await _groupDefinitionBasicRepository.InsertAsync(groupDefinitionRecord);
        }
        else
        {
            groupDefinitionRecord.ExtraProperties.Clear();
            foreach (var property in input.ExtraProperties)
            {
                groupDefinitionRecord.SetProperty(property.Key, property.Value);
            }

            if (!string.Equals(groupDefinitionRecord.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                groupDefinitionRecord.DisplayName = input.DisplayName;
            }

            groupDefinitionRecord = await _groupDefinitionBasicRepository.UpdateAsync(groupDefinitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await GroupDefinitionRecordToDto(groupDefinitionRecord);

        return dto;
    }

    protected async virtual Task<FeatureGroupDefinitionRecord> FindByNameAsync(string name)
    {
        var groupDefinitionFilter = await _groupDefinitionBasicRepository.GetQueryableAsync();

        var groupDefinitionRecord = await AsyncExecuter.FirstOrDefaultAsync(
            groupDefinitionFilter.Where(x => x.Name == name));

        return groupDefinitionRecord;
    }

    protected async virtual Task<FeatureGroupDefinitionDto> GroupDefinitionRecordToDto(FeatureGroupDefinitionRecord groupDefinitionRecord)
    {
        var groupDto = new FeatureGroupDefinitionDto
        {
            Name = groupDefinitionRecord.Name,
            FormatedDisplayName = groupDefinitionRecord.DisplayName,
        };

        var displayName = _localizableStringSerializer.Deserialize(groupDefinitionRecord.DisplayName);
        groupDto.DisplayName = await displayName.LocalizeAsync(StringLocalizerFactory);

        foreach (var property in groupDefinitionRecord.ExtraProperties)
        {
            groupDto.SetProperty(property.Key, property.Value);
        }

        return groupDto;
    }

    protected async virtual Task<FeatureGroupDefinitionDto> GroupDefinitionToDto(FeatureGroupDefinition groupDefinition)
    {
        var groupDto = new FeatureGroupDefinitionDto
        {
            Name = groupDefinition.Name
        };

        if (groupDefinition.DisplayName != null)
        {
            groupDto.DisplayName = await groupDefinition.DisplayName.LocalizeAsync(StringLocalizerFactory);
            groupDto.FormatedDisplayName = _localizableStringSerializer.Serialize(groupDefinition.DisplayName);
        }

        foreach (var property in groupDefinition.Properties)
        {
            groupDto.SetProperty(property.Key, property.Value);
        }

        return groupDto;
    }
}
