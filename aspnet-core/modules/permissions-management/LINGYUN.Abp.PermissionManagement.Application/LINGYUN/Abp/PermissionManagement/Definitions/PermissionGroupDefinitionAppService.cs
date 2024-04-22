using LINGYUN.Abp.PermissionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    private readonly IStaticPermissionDefinitionStore _staticPermissionDefinitionStore;
    private readonly IDynamicPermissionDefinitionStore _dynamicPermissionDefinitionStore;
    private readonly IPermissionGroupDefinitionRecordRepository _groupDefinitionRepository;
    private readonly IRepository<PermissionGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public PermissionGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer, 
        IPermissionDefinitionManager permissionDefinitionManager, 
        IStaticPermissionDefinitionStore staticPermissionDefinitionStore,
        IDynamicPermissionDefinitionStore dynamicPermissionDefinitionStore,
        IPermissionGroupDefinitionRecordRepository groupDefinitionRepository, 
        IRepository<PermissionGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _permissionDefinitionManager = permissionDefinitionManager;
        _staticPermissionDefinitionStore = staticPermissionDefinitionStore;
        _dynamicPermissionDefinitionStore = dynamicPermissionDefinitionStore;
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

        var groupDefinitionRecord = await _groupDefinitionBasicRepository.FindAsync(x => x.Name == input.Name);
        if (groupDefinitionRecord != null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.AlreayNameExists)
               .WithData(nameof(PermissionGroupDefinitionRecord.Name), input.Name);
        }

        groupDefinitionRecord = new PermissionGroupDefinitionRecord(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName);

        foreach (var property in input.ExtraProperties)
        {
            groupDefinitionRecord.SetProperty(property.Key, property.Value);
        }

        groupDefinitionRecord = await _groupDefinitionRepository.InsertAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(groupDefinitionRecord);
    }

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord != null)
        {
            await _groupDefinitionRepository.DeleteAsync(groupDefinitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<PermissionGroupDefinitionDto> GetAsync(string name)
    {
        var staticGroups = await _staticPermissionDefinitionStore.GetGroupsAsync();
        var groupDefinition = staticGroups.FirstOrDefault(x => x.Name == name);
        if (groupDefinition != null)
        {
            return DefinitionToDto(groupDefinition, true);
        }

        var dynamicGroups = await _dynamicPermissionDefinitionStore.GetGroupsAsync();

        groupDefinition = dynamicGroups.FirstOrDefault(x => x.Name == name);
        if (groupDefinition == null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), name);
        }

        return DefinitionToDto(groupDefinition);
    }

    public async virtual Task<ListResultDto<PermissionGroupDefinitionDto>> GetListAsync(PermissionGroupDefinitionGetListInput input)
    {
        var groupDtoList = new List<PermissionGroupDefinitionDto>();

        var staticGroups = await _staticPermissionDefinitionStore.GetGroupsAsync();
        var staticGroupsNames = staticGroups
           .Select(p => p.Name)
           .ToImmutableHashSet();
        groupDtoList.AddRange(staticGroups.Select(d => DefinitionToDto(d, true)));

        var dynamicGroups = await _dynamicPermissionDefinitionStore.GetGroupsAsync();
        groupDtoList.AddRange(dynamicGroups
           .Where(d => !staticGroupsNames.Contains(d.Name))
           .Select(d => DefinitionToDto(d)));

        return new ListResultDto<PermissionGroupDefinitionDto>(
            groupDtoList
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter))
                .ToList());
    }

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Update)]
    public async virtual Task<PermissionGroupDefinitionDto> UpdateAsync(string name, PermissionGroupDefinitionUpdateDto input)
    {
        var groupDefinition = await _permissionDefinitionManager.GetGroupOrNullAsync(name);
        if (groupDefinition != null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
              .WithData("Name", name);
        }

        var groupDefinitionRecord = await FindByNameAsync(name);

        if (groupDefinitionRecord == null)
        {
            groupDefinitionRecord = new PermissionGroupDefinitionRecord(
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

        return DefinitionRecordToDto(groupDefinitionRecord);
    }

    protected async virtual Task<PermissionGroupDefinitionRecord> FindByNameAsync(string name)
    {
        var groupDefinitionFilter = await _groupDefinitionBasicRepository.GetQueryableAsync();

        var groupDefinitionRecord = await AsyncExecuter.FirstOrDefaultAsync(
            groupDefinitionFilter.Where(x => x.Name == name));

        return groupDefinitionRecord;
    }

    protected virtual void UpdateByInput(PermissionGroupDefinitionRecord record, PermissionGroupDefinitionCreateOrUpdateDto input)
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

    protected virtual PermissionGroupDefinitionDto DefinitionRecordToDto(PermissionGroupDefinitionRecord groupDefinitionRecord)
    {
        var groupDto = new PermissionGroupDefinitionDto
        {
            IsStatic = false,
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

    protected virtual PermissionGroupDefinitionDto DefinitionToDto(PermissionGroupDefinition groupDefinition, bool isStatic = false)
    {
        var groupDto = new PermissionGroupDefinitionDto
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
