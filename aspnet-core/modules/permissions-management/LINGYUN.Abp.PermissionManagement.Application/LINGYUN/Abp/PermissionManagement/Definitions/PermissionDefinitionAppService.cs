using LINGYUN.Abp.PermissionManagement.Permissions;
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
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

[Authorize(PermissionManagementPermissionNames.Definition.Default)]
public class PermissionDefinitionAppService : PermissionManagementAppServiceBase, IPermissionDefinitionAppService
{
    private readonly ISimpleStateCheckerSerializer _simpleStateCheckerSerializer;
    private readonly IStaticPermissionDefinitionStore _staticPermissionDefinitionStore;
    private readonly IPermissionValueProviderManager _permissionValueProviderManager;
    private readonly IPermissionDefinitionRecordRepository _definitionRepository;
    private readonly IRepository<PermissionDefinitionRecord, Guid> _definitionBasicRepository;
    private readonly IRepository<PermissionGroupDefinitionRecord, Guid> _groupDefinitionBasicRepository;

    public PermissionDefinitionAppService(
        IStaticPermissionDefinitionStore staticPermissionDefinitionStore,
        IPermissionValueProviderManager permissionValueProviderManager,
        ISimpleStateCheckerSerializer simpleStateCheckerSerializer,
        IPermissionDefinitionRecordRepository definitionRepository,
        IRepository<PermissionDefinitionRecord, Guid> definitionBasicRepository,
        IRepository<PermissionGroupDefinitionRecord, Guid> groupDefinitionBasicRepository)
    {
        _staticPermissionDefinitionStore = staticPermissionDefinitionStore;
        _permissionValueProviderManager = permissionValueProviderManager;
        _simpleStateCheckerSerializer = simpleStateCheckerSerializer;
        _definitionRepository = definitionRepository;
        _definitionBasicRepository = definitionBasicRepository;
        _groupDefinitionBasicRepository = groupDefinitionBasicRepository;
    }

    public virtual Task<ListResultDto<NameValue<string>>> GetAssignableProvidersAsync()
    {
        var providerNames = _permissionValueProviderManager.ValueProviders.Select(x => x.Name);

        return Task.FromResult(new ListResultDto<NameValue<string>>(
            providerNames.Select(name =>
            {
                var provider = new NameValue<string>(name, name);
                var displayName = L[$"PermissionProviders:{name}"];
                if (!displayName.ResourceNotFound)
                {
                    provider.Name = displayName.Value;
                }
                return provider;
            }).ToList()));
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Create)]
    public async virtual Task<PermissionDefinitionDto> CreateAsync(PermissionDefinitionCreateDto input)
    {
        if (await _definitionRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(PermissionManagementErrorCodes.Definition.AlreayNameExists)
                .WithData(nameof(PermissionDefinitionRecord.Name), input.Name);
        }

        var groupDefinition = await _groupDefinitionBasicRepository.FindAsync(x => x.Name == input.GroupName) ??
            throw new BusinessException(PermissionManagementErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(PermissionGroupDefinitionRecord.Name), input.GroupName);
        var definitionRecord = new PermissionDefinitionRecord(
            GuidGenerator.Create(),
            groupDefinition.Name,
            input.Name,
            input.ResourceName,
            input.ManagementPermissionName,
            input.ParentName,
            input.DisplayName,
            input.IsEnabled);

        await UpdateByInput(definitionRecord, input);

        definitionRecord.SetProperty(nameof(PermissionDefinitionDto.IsStatic), false);

        definitionRecord = await _definitionRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(PermissionManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(PermissionDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);

        await _definitionRepository.DeleteAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<PermissionDefinitionDto> GetAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(PermissionManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(PermissionDefinitionRecord.Name), name);
        return DefinitionRecordToDto(definitionRecord);
    }

    public async virtual Task<ListResultDto<PermissionDefinitionDto>> GetListAsync(PermissionDefinitionGetListInput input)
    {
        var permissionDtoList = new List<PermissionDefinitionDto>();

        Expression<Func<PermissionDefinitionRecord, bool>> predicate = _ => true;
        if (!input.GroupName.IsNullOrWhiteSpace())
        {
            predicate = predicate.And(x => x.Name == input.GroupName);
        }
        var permissionDefinitions = await _definitionBasicRepository.GetListAsync(predicate);
        permissionDtoList.AddRange(permissionDefinitions.Select(DefinitionRecordToDto));

        return new ListResultDto<PermissionDefinitionDto>(permissionDtoList);
    }

    [Authorize(PermissionManagementPermissionNames.Definition.Update)]
    public async virtual Task<PermissionDefinitionDto> UpdateAsync(string name, PermissionDefinitionUpdateDto input)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(PermissionManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(PermissionDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);

        await UpdateByInput(definitionRecord, input);

        definitionRecord = await _definitionBasicRepository.UpdateAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected virtual void CheckIsStaticDefinitionRecord(PermissionDefinitionRecord record)
    {
        if (record.GetProperty(nameof(PermissionDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(PermissionManagementErrorCodes.Definition.StaticPermissionNotAllowedChanged)
              .WithData("Name", record.Name);
        }
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
        if (!string.Equals(record.ResourceName, input.ResourceName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.ResourceName = input.ResourceName;
        }
        if (!string.Equals(record.ManagementPermissionName, input.ManagementPermissionName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.ManagementPermissionName = input.ManagementPermissionName;
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

    protected virtual PermissionDefinitionDto DefinitionRecordToDto(PermissionDefinitionRecord definitionRecord)
    {
        var dto = new PermissionDefinitionDto
        {
            IsStatic = definitionRecord.GetProperty(nameof(PermissionDefinitionDto.IsStatic), true),
            Name = definitionRecord.Name,
            GroupName = definitionRecord.GroupName,
            ParentName = definitionRecord.ParentName,
            IsEnabled = definitionRecord.IsEnabled,
            DisplayName = definitionRecord.DisplayName,
            ResourceName = definitionRecord.ResourceName,
            ManagementPermissionName = definitionRecord.ManagementPermissionName,
            Providers = definitionRecord.Providers?.Split(',').ToList() ?? [],
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
}
