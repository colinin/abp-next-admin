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

namespace LINGYUN.Abp.PermissionManagement.Definitions;

[Authorize(PermissionManagementPermissionNames.GroupDefinition.Default)]
public class PermissionGroupDefinitionAppService : PermissionManagementAppServiceBase, IPermissionGroupDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    private readonly IPermissionGroupDefinitionRecordRepository _groupDefinitionRepository;
    private readonly IRepository<PermissionGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public PermissionGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer, 
        IPermissionDefinitionManager permissionDefinitionManager, 
        IPermissionGroupDefinitionRecordRepository groupDefinitionRepository, 
        IRepository<PermissionGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _permissionDefinitionManager = permissionDefinitionManager;
        _groupDefinitionRepository = groupDefinitionRepository;
        _groupDefinitionBasicRepository = groupDefinitionBasicRepository;
    }

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Create)]
    public async virtual Task<PermissionGroupDefinitionDto> CreateAsync(PermissionGroupDefinitionCreateDto input)
    {
        if (await _permissionDefinitionManager.GetGroupOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.AlreayNameExists)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), input.Name);
        }

        var groupDefinitionRecord = new PermissionGroupDefinitionRecord(
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

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord == null)
        {
            return;
        }

        await _groupDefinitionRepository.DeleteAsync(groupDefinitionRecord);
    }

    public async virtual Task<PermissionGroupDefinitionDto> GetAsync(string name)
    {
        var groupDefinition = await _permissionDefinitionManager.GetGroupOrNullAsync(name);
        if (groupDefinition == null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), name);
        }

        var dto = await GroupDefinitionToDto(groupDefinition);

        return dto;
    }

    public async virtual Task<PagedResultDto<PermissionGroupDefinitionDto>> GetListAsync(PermissionGroupDefinitionGetListInput input)
    {
        var groups = new List<PermissionGroupDefinitionDto>();

        var groupDefinitions = await _permissionDefinitionManager.GetGroupsAsync();

        var groupDefinitionFilter = groupDefinitions.AsQueryable()
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter));

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(PermissionDefinitionRecord.Name);
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

        return new PagedResultDto<PermissionGroupDefinitionDto>(filterGroupDefinitionCount, groups);
    }

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Update)]
    public async virtual Task<PermissionGroupDefinitionDto> UpdateAsync(string name, PermissionGroupDefinitionUpdateDto input)
    {
        var groupDefinition = await _permissionDefinitionManager.GetGroupOrNullAsync(name);
        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord == null)
        {
            groupDefinitionRecord = new PermissionGroupDefinitionRecord(
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

    protected async virtual Task<PermissionGroupDefinitionRecord> FindByNameAsync(string name)
    {
        var groupDefinitionFilter = await _groupDefinitionBasicRepository.GetQueryableAsync();

        var groupDefinitionRecord = await AsyncExecuter.FirstOrDefaultAsync(
            groupDefinitionFilter.Where(x => x.Name == name));

        return groupDefinitionRecord;
    }

    protected async virtual Task<PermissionGroupDefinitionDto> GroupDefinitionRecordToDto(PermissionGroupDefinitionRecord groupDefinitionRecord)
    {
        var groupDto = new PermissionGroupDefinitionDto
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

    protected async virtual Task<PermissionGroupDefinitionDto> GroupDefinitionToDto(PermissionGroupDefinition groupDefinition)
    {
        var groupDto = new PermissionGroupDefinitionDto
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
