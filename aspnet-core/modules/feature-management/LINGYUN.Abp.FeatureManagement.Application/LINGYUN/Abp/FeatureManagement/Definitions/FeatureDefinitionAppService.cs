using LINGYUN.Abp.FeatureManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Features;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

[Authorize(FeatureManagementPermissionNames.Definition.Default)]
public class FeatureDefinitionAppService : FeatureManagementAppServiceBase, IFeatureDefinitionAppService
{
    private readonly StringValueTypeSerializer _stringValueTypeSerializer;
    private readonly IFeatureValueProviderManager _featureValueProviderManager;
    private readonly IFeatureDefinitionRecordRepository _definitionRepository;
    private readonly IRepository<FeatureDefinitionRecord, Guid> _definitionBasicRepository;
    private readonly IRepository<FeatureGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public FeatureDefinitionAppService(
        StringValueTypeSerializer stringValueTypeSerializer,
        IFeatureValueProviderManager featureValueProviderManager,
        IFeatureDefinitionRecordRepository definitionRepository, 
        IRepository<FeatureDefinitionRecord, Guid> definitionBasicRepository,
        IRepository<FeatureGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _stringValueTypeSerializer = stringValueTypeSerializer;
        _featureValueProviderManager = featureValueProviderManager;
        _definitionRepository = definitionRepository;
        _definitionBasicRepository = definitionBasicRepository;
        _groupDefinitionBasicRepository = groupDefinitionBasicRepository;
    }

    public virtual Task<ListResultDto<NameValue<string>>> GetAssignableProvidersAsync()
    {
        var providerNames = _featureValueProviderManager.ValueProviders.Select(x => x.Name);

        return Task.FromResult(new ListResultDto<NameValue<string>>(
            providerNames.Select(name =>
            {
                var provider = new NameValue<string>(name, name);
                var displayName = L[$"FeatureProviders:{name}"];
                if (!displayName.ResourceNotFound)
                {
                    provider.Name = displayName.Value;
                }
                return provider;
            }).ToList()));
    }

    [Authorize(FeatureManagementPermissionNames.Definition.Create)]
    public async virtual Task<FeatureDefinitionDto> CreateAsync(FeatureDefinitionCreateDto input)
    {
        if (await _definitionRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(FeatureManagementErrorCodes.Definition.AlreayNameExists)
                .WithData(nameof(FeatureDefinitionRecord.Name), input.Name);
        }

        var groupDefinition = await _groupDefinitionBasicRepository.FindAsync(x => x.Name == input.GroupName)
            ?? throw new BusinessException(FeatureManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(FeatureGroupDefinitionRecord.Name), input.GroupName);
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

        UpdateByInput(definitionRecord, input);

        definitionRecord.SetProperty(nameof(FeatureDefinitionDto.IsStatic), false);

        await _definitionRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    [Authorize(FeatureManagementPermissionNames.Definition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindRecordByNameAsync(name)
            ?? throw new BusinessException(FeatureManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(FeatureDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);

        await _definitionRepository.DeleteAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<FeatureDefinitionDto> GetAsync(string name)
    {
        var definitionRecord = await FindRecordByNameAsync(name)
            ?? throw new BusinessException(FeatureManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(FeatureDefinitionRecord.Name), name);
        return DefinitionRecordToDto(definitionRecord);
    }

    public async virtual Task<ListResultDto<FeatureDefinitionDto>> GetListAsync(FeatureDefinitionGetListInput input)
    {
        var featureDtoList = new List<FeatureDefinitionDto>();

        Expression<Func<FeatureDefinitionRecord, bool>> predicate = _ => true;
        if (!input.GroupName.IsNullOrWhiteSpace())
        {
            predicate = predicate.And(x => x.Name == input.GroupName);
        }
        var definitionRecords = await _definitionBasicRepository.GetListAsync(predicate);
        featureDtoList.AddRange(definitionRecords.Select(DefinitionRecordToDto));

        return new ListResultDto<FeatureDefinitionDto>(featureDtoList);
    }

    [Authorize(FeatureManagementPermissionNames.Definition.Update)]
    public async virtual Task<FeatureDefinitionDto> UpdateAsync(string name, FeatureDefinitionUpdateDto input)
    {
        var definitionRecord = await FindRecordByNameAsync(name)
            ?? throw new BusinessException(FeatureManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(FeatureDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);
        UpdateByInput(definitionRecord, input);
        definitionRecord = await _definitionBasicRepository.UpdateAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected virtual void CheckIsStaticDefinitionRecord(FeatureDefinitionRecord record)
    {
        if (record.GetProperty(nameof(FeatureDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(FeatureManagementErrorCodes.Definition.StaticFeatureNotAllowedChanged)
              .WithData("Name", record.Name);
        }
    }

    protected virtual void UpdateByInput(FeatureDefinitionRecord record, FeatureDefinitionCreateOrUpdateDto input)
    {
        record.IsVisibleToClients = input.IsVisibleToClients;
        record.IsAvailableToHost = input.IsAvailableToHost;
        if (!string.Equals(record.ParentName, input.ParentName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.ParentName = input.ParentName;
        }
        if (!string.Equals(record.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DisplayName = input.DisplayName;
        }
        if (!string.Equals(record.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
        {
            record.Description = input.Description;
        }
        if (!string.Equals(record.DefaultValue, input.DefaultValue, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DefaultValue = input.DefaultValue;
        }
        string allowedProviders = null;
        if (!input.AllowedProviders.IsNullOrEmpty())
        {
            allowedProviders = input.AllowedProviders.JoinAsString(",");
        }
        if (!string.Equals(record.AllowedProviders, allowedProviders, StringComparison.InvariantCultureIgnoreCase))
        {
            record.AllowedProviders = allowedProviders;
        }
        record.ExtraProperties.Clear();
        foreach (var property in input.ExtraProperties)
        {
            record.SetProperty(property.Key, property.Value);
        }
        try
        {
            if (!string.Equals(record.ValueType, input.ValueType, StringComparison.InvariantCultureIgnoreCase))
            {
                var _ = _stringValueTypeSerializer.Deserialize(input.ValueType);
                record.ValueType = input.ValueType;
            }
        }
        catch
        {
            throw new AbpValidationException(
                new List<ValidationResult>
                {
                    new ValidationResult(
                        L["The field {0} is invalid", L["DisplayName:ValueType"]],
                        new string[1] { nameof(input.ValueType) })
                });
        }
    }

    protected async virtual Task<FeatureDefinitionRecord> FindRecordByNameAsync(string name)
    {
        var DefinitionFilter = await _definitionBasicRepository.GetQueryableAsync();

        var definitionRecord = await AsyncExecuter.FirstOrDefaultAsync(
            DefinitionFilter.Where(x => x.Name == name));
        
        return definitionRecord;
    }

    protected virtual FeatureDefinitionDto DefinitionRecordToDto(FeatureDefinitionRecord definitionRecord)
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
            AllowedProviders = definitionRecord.AllowedProviders?.Split(',').ToList() ?? [],
            Description = definitionRecord.Description,
            DisplayName = definitionRecord.DisplayName,
            ExtraProperties = new ExtraPropertyDictionary(),
            IsStatic = definitionRecord.GetProperty(nameof(FeatureDefinitionDto.IsStatic), true),
        };

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
