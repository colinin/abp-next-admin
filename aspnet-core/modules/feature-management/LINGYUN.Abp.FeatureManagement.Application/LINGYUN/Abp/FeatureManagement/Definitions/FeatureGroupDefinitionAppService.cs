using LINGYUN.Abp.FeatureManagement.Permissions;
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
using Volo.Abp.FeatureManagement;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

[Authorize(FeatureManagementPermissionNames.GroupDefinition.Default)]
public class FeatureGroupDefinitionAppService : FeatureManagementAppServiceBase, IFeatureGroupDefinitionAppService
{
    private readonly IFeatureGroupDefinitionRecordRepository _groupDefinitionRepository;
    private readonly IRepository<FeatureGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public FeatureGroupDefinitionAppService(
        IFeatureGroupDefinitionRecordRepository groupDefinitionRepository, 
        IRepository<FeatureGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _groupDefinitionRepository = groupDefinitionRepository;
        _groupDefinitionBasicRepository = groupDefinitionBasicRepository;
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Create)]
    public async virtual Task<FeatureGroupDefinitionDto> CreateAsync(FeatureGroupDefinitionCreateDto input)
    {
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

        groupDefinitionRecord.SetProperty(nameof(FeatureGroupDefinitionDto.IsStatic), false);

        await _groupDefinitionRepository.InsertAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return GroupDefinitionRecordToDto(groupDefinitionRecord);
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name)
            ?? throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(groupDefinitionRecord);

        await _groupDefinitionRepository.DeleteAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<FeatureGroupDefinitionDto> GetAsync(string name)
    {
        var groupDefinitionRecord = await FindByNameAsync(name)
            ?? throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), name);

        return GroupDefinitionRecordToDto(groupDefinitionRecord);
    }

    public async virtual Task<ListResultDto<FeatureGroupDefinitionDto>> GetListAsync(FeatureGroupDefinitionGetListInput input)
    {
        var groupDtoList = new List<FeatureGroupDefinitionDto>();

        Expression<Func<FeatureGroupDefinitionRecord, bool>> predicate = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            predicate = predicate.And(x => x.Name.Contains(input.Filter));
        }
        var permissionGroupDefinitions = await _groupDefinitionBasicRepository.GetListAsync(predicate);

        groupDtoList.AddRange(permissionGroupDefinitions.Select(GroupDefinitionRecordToDto));

        return new ListResultDto<FeatureGroupDefinitionDto>(groupDtoList);
    }

    [Authorize(FeatureManagementPermissionNames.GroupDefinition.Update)]
    public async virtual Task<FeatureGroupDefinitionDto> UpdateAsync(string name, FeatureGroupDefinitionUpdateDto input)
    {
        var groupDefinitionRecord = await FindByNameAsync(name)
            ?? throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(groupDefinitionRecord);
        UpdateByInput(groupDefinitionRecord, input);
        groupDefinitionRecord = await _groupDefinitionBasicRepository.UpdateAsync(groupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return GroupDefinitionRecordToDto(groupDefinitionRecord);
    }

    protected virtual void CheckIsStaticDefinitionRecord(FeatureGroupDefinitionRecord record)
    {
        if (record.GetProperty(nameof(FeatureGroupDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
              .WithData("Name", record.Name);
        }
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
            IsStatic = groupDefinitionRecord.GetProperty(nameof(FeatureGroupDefinitionDto.IsStatic), true),
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
