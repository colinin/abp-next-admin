using LINGYUN.Abp.PermissionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

[Authorize(PermissionManagementPermissionNames.GroupDefinition.Default)]
public class PermissionGroupDefinitionAppService : PermissionManagementAppServiceBase, IPermissionGroupDefinitionAppService
{
    private readonly IPermissionGroupDefinitionRecordRepository _groupDefinitionRepository;
    private readonly IRepository<PermissionGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public PermissionGroupDefinitionAppService(
        IPermissionGroupDefinitionRecordRepository groupDefinitionRepository, 
        IRepository<PermissionGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _groupDefinitionRepository = groupDefinitionRepository;
        _groupDefinitionBasicRepository = groupDefinitionBasicRepository;
    }

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Create)]
    public async virtual Task<PermissionGroupDefinitionDto> CreateAsync(PermissionGroupDefinitionCreateDto input)
    {
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
        groupDefinitionRecord.SetProperty(nameof(PermissionGroupDefinitionDto.IsStatic), false);

        groupDefinitionRecord = await _groupDefinitionRepository.InsertAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(groupDefinitionRecord);
    }

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(groupDefinitionRecord);

        await _groupDefinitionRepository.DeleteAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<PermissionGroupDefinitionDto> GetAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), name);

        return DefinitionRecordToDto(groupDefinitionRecord);
    }

    public async virtual Task<ListResultDto<PermissionGroupDefinitionDto>> GetListAsync(PermissionGroupDefinitionGetListInput input)
    {
        var groupDtoList = new List<PermissionGroupDefinitionDto>();

        Expression<Func<PermissionGroupDefinitionRecord, bool>> predicate = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            predicate = predicate.And(x => x.Name.Contains(input.Filter));
        }
        var permissionGroupDefinitions = await _groupDefinitionBasicRepository.GetListAsync(predicate);

        groupDtoList.AddRange(permissionGroupDefinitions.Select(DefinitionRecordToDto));

        return new ListResultDto<PermissionGroupDefinitionDto>(groupDtoList);
    }

    [Authorize(PermissionManagementPermissionNames.GroupDefinition.Update)]
    public async virtual Task<PermissionGroupDefinitionDto> UpdateAsync(string name, PermissionGroupDefinitionUpdateDto input)
    {
        var groupDefinitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(groupDefinitionRecord);
        UpdateByInput(groupDefinitionRecord, input);

        groupDefinitionRecord = await _groupDefinitionBasicRepository.UpdateAsync(groupDefinitionRecord);

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

    protected virtual void CheckIsStaticDefinitionRecord(PermissionGroupDefinitionRecord record)
    {
        if (record.GetProperty(nameof(PermissionGroupDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
              .WithData("Name", record.Name);
        }
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
            IsStatic = groupDefinitionRecord.GetProperty(nameof(PermissionGroupDefinitionDto.IsStatic), true),
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
}
