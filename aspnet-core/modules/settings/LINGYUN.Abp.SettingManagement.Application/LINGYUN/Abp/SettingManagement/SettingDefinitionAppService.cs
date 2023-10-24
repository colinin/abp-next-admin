using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    private readonly ISettingDefinitionManager _settingDefinitionManager;
    private readonly IStaticSettingDefinitionStore _staticSettingDefinitionStore;
    private readonly IDynamicSettingDefinitionStore _dynamicSettingDefinitionStore;
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IRepository<SettingDefinitionRecord, Guid> _settingRepository;

    public SettingDefinitionAppService(
        IStringEncryptionService stringEncryptionService, 
        ISettingDefinitionManager settingDefinitionManager,
        IStaticSettingDefinitionStore staticSettingDefinitionStore,
        IDynamicSettingDefinitionStore dynamicSettingDefinitionStore,
        ILocalizableStringSerializer localizableStringSerializer,
        IRepository<SettingDefinitionRecord, Guid> settingRepository)
    {
        _stringEncryptionService = stringEncryptionService;
        _settingDefinitionManager = settingDefinitionManager;
        _staticSettingDefinitionStore = staticSettingDefinitionStore;
        _dynamicSettingDefinitionStore = dynamicSettingDefinitionStore;
        _localizableStringSerializer = localizableStringSerializer;
        _settingRepository = settingRepository;
    }

    [Authorize(SettingManagementPermissions.Definition.Create)]
    public async virtual Task<SettingDefinitionDto> CreateAsync(SettingDefinitionCreateDto input)
    {
        if (await _staticSettingDefinitionStore.GetOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(SettingManagementErrorCodes.Definition.DuplicateName)
                .WithData("Name", input.Name);
        }

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

        settingDefinitionRecord = await _settingRepository.InsertAsync(settingDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(settingDefinitionRecord);
    }

    [Authorize(SettingManagementPermissions.Definition.DeleteOrRestore)]
    public async virtual Task DeleteOrRestoreAsync(string name)
    {
        var settingDefinitionRecord = await _settingRepository.FindAsync(x => x.Name == name);
        if (settingDefinitionRecord != null)
        {
            await _settingRepository.DeleteAsync(settingDefinitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<SettingDefinitionDto> GetAsync(string name)
    {
        var settingDefinition = await _staticSettingDefinitionStore.GetOrNullAsync(name);
        if (settingDefinition != null)
        {
            return DefinitionToDto(settingDefinition, true);
        }
        settingDefinition = await _dynamicSettingDefinitionStore.GetOrNullAsync(name);
        var dto = DefinitionToDto(settingDefinition);
        if (dto.IsEncrypted && !string.IsNullOrWhiteSpace(dto.DefaultValue))
        {
            dto.DefaultValue = _stringEncryptionService.Decrypt(dto.DefaultValue);
        }
        return dto;
    }

    public async virtual Task<ListResultDto<SettingDefinitionDto>> GetListAsync(SettingDefinitionGetListInput input)
    {
        var settingDtoList = new List<SettingDefinitionDto>();
        var staticSettings = await _staticSettingDefinitionStore.GetAllAsync();
        var staticSettingNames = staticSettings
            .Select(p => p.Name)
            .ToImmutableHashSet();
        settingDtoList.AddRange(staticSettings.Select(d => DefinitionToDto(d, true)));

        var dynamicSettings = await _dynamicSettingDefinitionStore.GetAllAsync();
        settingDtoList.AddRange(dynamicSettings
            .Where(d => !staticSettingNames.Contains(d.Name))
            .Select(d => DefinitionToDto(d)));

        return new ListResultDto<SettingDefinitionDto>(settingDtoList
            .WhereIf(!input.ProviderName.IsNullOrWhiteSpace(), x => x.Providers.Contains(input.ProviderName))
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter))
            .ToList());
    }

    [Authorize(SettingManagementPermissions.Definition.Update)]
    public async virtual Task<SettingDefinitionDto> UpdateAsync(string name, SettingDefinitionUpdateDto input)
    {
        if (await _staticSettingDefinitionStore.GetOrNullAsync(name) != null)
        {
            throw new BusinessException(SettingManagementErrorCodes.Definition.StaticSettingNotAllowedChanged)
              .WithData("Name", name);
        }

        var settingDefinitionRecord = await _settingRepository.FindAsync(x => x.Name == name);
        if (settingDefinitionRecord == null)
        {
            settingDefinitionRecord = new SettingDefinitionRecord(
               GuidGenerator.Create(),
               name,
               input.DisplayName,
               input.Description,
               input.DefaultValue,
               input.IsVisibleToClients,
               input.Providers?.JoinAsString(","),
               input.IsInherited,
               input.IsEncrypted);
            UpdateByInput(settingDefinitionRecord, input);
            settingDefinitionRecord = await _settingRepository.InsertAsync(settingDefinitionRecord);
        }
        else
        {
            UpdateByInput(settingDefinitionRecord, input);
            settingDefinitionRecord = await _settingRepository.UpdateAsync(settingDefinitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(settingDefinitionRecord);
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
        var dto = new SettingDefinitionDto
        {
            Name = definitionRecord.Name,
            IsVisibleToClients = definitionRecord.IsVisibleToClients,
            DefaultValue = definitionRecord.DefaultValue,
            Description = definitionRecord.Description,
            DisplayName = definitionRecord.DisplayName,
            IsEncrypted = definitionRecord.IsEncrypted,
            IsInherited = definitionRecord.IsInherited,
            Providers = definitionRecord.Providers?.Split(',').ToList(),
        };

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }

    protected virtual SettingDefinitionDto DefinitionToDto(SettingDefinition definition, bool isStatic = false)
    {
        var dto = new SettingDefinitionDto
        {
            IsStatic = isStatic,
            Name = definition.Name,
            IsVisibleToClients = definition.IsVisibleToClients,
            DefaultValue = definition.DefaultValue,
            IsEncrypted = definition.IsEncrypted,
            IsInherited = definition.IsInherited,
            Providers = definition.Providers,
        };

        if (definition.DisplayName != null)
        {
            dto.DisplayName = _localizableStringSerializer.Serialize(definition.DisplayName);
        }

        if (definition.Description != null)
        {
            dto.Description = _localizableStringSerializer.Serialize(definition.Description);
        }

        foreach (var property in definition.Properties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
