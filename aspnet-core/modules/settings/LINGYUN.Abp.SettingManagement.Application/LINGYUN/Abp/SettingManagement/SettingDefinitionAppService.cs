using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.Security.Encryption;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.SettingManagement;

[Authorize(SettingManagementPermissions.Definition.Default)]
public class SettingDefinitionAppService : SettingManagementAppServiceBase, ISettingDefinitionAppService
{
    private readonly IStringEncryptionService _stringEncryptionService;
    private readonly ISettingValueProviderManager _settingValueProviderManager;
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IRepository<SettingDefinitionRecord, Guid> _settingRepository;

    public SettingDefinitionAppService(
        IStringEncryptionService stringEncryptionService, 
        ILocalizableStringSerializer localizableStringSerializer,
        IRepository<SettingDefinitionRecord, Guid> settingRepository,
        ISettingValueProviderManager settingValueProviderManager)
    {
        _stringEncryptionService = stringEncryptionService;
        _localizableStringSerializer = localizableStringSerializer;
        _settingRepository = settingRepository;
        _settingValueProviderManager = settingValueProviderManager;
    }

    [Authorize(SettingManagementPermissions.Definition.Create)]
    public async virtual Task<SettingDefinitionDto> CreateAsync(SettingDefinitionCreateDto input)
    {
        var settingDefinitionRecord = await _settingRepository.FindAsync(x => x.Name == input.Name);
        if (settingDefinitionRecord != null)
        {
            throw new BusinessException(SettingManagementErrorCodes.Definition.DuplicateName)
               .WithData("Name", input.Name);
        }

        var defaultValue = input.DefaultValue;
        if (input.IsEncrypted)
        {
            defaultValue = _stringEncryptionService.Encrypt(defaultValue);
        }
        settingDefinitionRecord = new SettingDefinitionRecord(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.Description,
            defaultValue,
            input.IsVisibleToClients,
            input.Providers?.JoinAsString(","),
            input.IsInherited,
            input.IsEncrypted);
        foreach (var property in input.ExtraProperties)
        {
            settingDefinitionRecord.ExtraProperties.Add(property.Key, property.Value);
        }
        settingDefinitionRecord.SetProperty(nameof(SettingDefinitionDto.IsStatic), false);

        settingDefinitionRecord = await _settingRepository.InsertAsync(settingDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(settingDefinitionRecord);
    }

    [Authorize(SettingManagementPermissions.Definition.DeleteOrRestore)]
    public async virtual Task DeleteOrRestoreAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(SettingManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(SettingDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);

        await _settingRepository.DeleteAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<SettingDefinitionDto> GetAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(SettingManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(SettingDefinitionRecord.Name), name);
        var dto = DefinitionRecordToDto(definitionRecord);
        if (dto.IsEncrypted && !string.IsNullOrWhiteSpace(dto.DefaultValue))
        {
            dto.DefaultValue = _stringEncryptionService.Decrypt(dto.DefaultValue);
        }
        return dto;
    }

    public async virtual Task<ListResultDto<SettingDefinitionDto>> GetListAsync(SettingDefinitionGetListInput input)
    {
        var settingDtoList = new List<SettingDefinitionDto>();

        Expression<Func<SettingDefinitionRecord, bool>> predicate = _ => true;
        if (!input.ProviderName.IsNullOrWhiteSpace())
        {
            predicate = predicate.And(x => x.Providers.Contains(input.ProviderName));
        }
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            predicate = predicate.And(x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter));
        }
        var settingDefinitionRecords = await _settingRepository.GetListAsync();
        settingDtoList.AddRange(settingDefinitionRecords.Select(DefinitionRecordToDto).Where(dto => dto != null));

        return new ListResultDto<SettingDefinitionDto>(settingDtoList);
    }

    public virtual Task<ListResultDto<NameValue<string>>> GetAssignableProvidersAsync()
    {
        var providerNames = _settingValueProviderManager.Providers.Select(x => x.Name);

        return Task.FromResult(new ListResultDto<NameValue<string>>(
            providerNames.Select(name =>
            {
                var provider = new NameValue<string>(name, name);
                var displayName = L[$"SettingProviders:{name}"];
                if (!displayName.ResourceNotFound)
                {
                    provider.Name = displayName.Value;
                }
                return provider;
            }).ToList()));
    }

    [Authorize(SettingManagementPermissions.Definition.Update)]
    public async virtual Task<SettingDefinitionDto> UpdateAsync(string name, SettingDefinitionUpdateDto input)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(SettingManagementErrorCodes.Definition.NameNotFount)
                .WithData(nameof(SettingDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);
        UpdateByInput(definitionRecord, input);
        definitionRecord = await _settingRepository.UpdateAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected async virtual Task<SettingDefinitionRecord> FindByNameAsync(string name)
    {
        return await _settingRepository.FindAsync(x => x.Name == name);
    }

    protected virtual void CheckIsStaticDefinitionRecord(SettingDefinitionRecord record)
    {
        if (record.GetProperty(nameof(SettingDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(SettingManagementErrorCodes.Definition.StaticSettingNotAllowedChanged)
              .WithData("Name", record.Name);
        }
    }

    protected virtual void UpdateByInput(SettingDefinitionRecord record, SettingDefinitionCreateOrUpdateDto input)
    {
        if (!string.Equals(record.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DisplayName = input.DisplayName;
        }
        if (!string.Equals(record.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
        {
            record.Description = input.Description;
        }

        var defaultValue = input.DefaultValue;
        if (input.IsEncrypted)
        {
            defaultValue = _stringEncryptionService.Encrypt(defaultValue);
        }
        if (!string.Equals(record.DefaultValue, defaultValue, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DefaultValue = defaultValue;
        }
        record.IsInherited = input.IsInherited;
        record.IsEncrypted = input.IsEncrypted;
        record.IsVisibleToClients = input.IsVisibleToClients;
        record.Providers = input.Providers?.JoinAsString(",");
        record.ExtraProperties.Clear();

        foreach (var property in input.ExtraProperties)
        {
            record.ExtraProperties.Add(property.Key, property.Value);
        }
    }

    protected virtual SettingDefinitionDto DefinitionRecordToDto(SettingDefinitionRecord definitionRecord)
    {
        if (definitionRecord == null)
        {
            return null;
        }
        var dto = new SettingDefinitionDto
        {
            Name = definitionRecord.Name,
            IsVisibleToClients = definitionRecord.IsVisibleToClients,
            DefaultValue = definitionRecord.DefaultValue,
            Description = definitionRecord.Description,
            DisplayName = definitionRecord.DisplayName,
            IsEncrypted = definitionRecord.IsEncrypted,
            IsInherited = definitionRecord.IsInherited,
            Providers = definitionRecord.Providers?.Split(',').ToList() ?? [],
            IsStatic = definitionRecord.GetProperty(nameof(SettingDefinitionDto.IsStatic), true),
        };

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
