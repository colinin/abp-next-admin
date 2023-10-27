using LINGYUN.Abp.Notifications.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.Definitions.Groups;

[Authorize(NotificationsPermissions.GroupDefinition.Default)]
public class NotificationGroupDefinitionAppService : AbpNotificationsApplicationServiceBase, INotificationGroupDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly INotificationDefinitionManager _definitionManager;
    private readonly IStaticNotificationDefinitionStore _staticDefinitionStore;
    private readonly IDynamicNotificationDefinitionStore _dynamicDefinitionStore;
    private readonly INotificationDefinitionGroupRecordRepository _definitionRecordRepository;

    public NotificationGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer, 
        INotificationDefinitionManager definitionManager,
        IStaticNotificationDefinitionStore staticDefinitionStore,
        IDynamicNotificationDefinitionStore dynamicDefinitionStore,
        INotificationDefinitionGroupRecordRepository definitionRecordRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _definitionManager = definitionManager;
        _staticDefinitionStore = staticDefinitionStore;
        _dynamicDefinitionStore = dynamicDefinitionStore;
        _definitionRecordRepository = definitionRecordRepository;
    }

    [Authorize(NotificationsPermissions.GroupDefinition.Create)]
    public async virtual Task<NotificationGroupDefinitionDto> CreateAsync(NotificationGroupDefinitionCreateDto input)
    {
        if (await _definitionManager.GetGroupOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.AlreayNameExists)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), input.Name);
        }
        var definitionRecord = await FindByNameAsync(input.Name);
        if (definitionRecord != null)
        {
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.AlreayNameExists)
               .WithData(nameof(NotificationDefinitionGroupRecord.Name), input.Name);
        }

        definitionRecord = new NotificationDefinitionGroupRecord(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName);

        UpdateByInput(definitionRecord, input);

        await _definitionRecordRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    [Authorize(NotificationsPermissions.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name);

        if (definitionRecord != null)
        {
            await _definitionRecordRepository.DeleteAsync(definitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<NotificationGroupDefinitionDto> GetAsync(string name)
    {
        var definition = await _staticDefinitionStore.GetGroupOrNullAsync(name);
        if (definition != null)
        {
            return DefinitionToDto(definition, true);
        }

        definition = await _dynamicDefinitionStore.GetGroupOrNullAsync(name);
        if (definition == null)
        {
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), name);
        }

        return DefinitionToDto(definition);
    }

    public async virtual Task<ListResultDto<NotificationGroupDefinitionDto>> GetListAsync(NotificationGroupDefinitionGetListInput input)
    {
        var definitionDtoList = new List<NotificationGroupDefinitionDto>();

        var staticGroups = await _staticDefinitionStore.GetGroupsAsync();
        var staticGroupsNames = staticGroups
           .Select(p => p.Name)
           .ToImmutableHashSet();
        definitionDtoList.AddRange(staticGroups.Select(d => DefinitionToDto(d, true)));

        var dynamicGroups = await _dynamicDefinitionStore.GetGroupsAsync();
        definitionDtoList.AddRange(dynamicGroups
           .Where(d => !staticGroupsNames.Contains(d.Name))
           .Select(d => DefinitionToDto(d)));

        return new ListResultDto<NotificationGroupDefinitionDto>(
            definitionDtoList
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter))
                .ToList());
    }

    [Authorize(NotificationsPermissions.GroupDefinition.Update)]
    public async virtual Task<NotificationGroupDefinitionDto> UpdateAsync(string name, NotificationGroupDefinitionUpdateDto input)
    {
        var definition = await _staticDefinitionStore.GetGroupOrNullAsync(name);
        if (definition != null)
        {
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
              .WithData(nameof(NotificationDefinitionGroupRecord.Name), name);
        }

        var definitionRecord = await FindByNameAsync(name);

        if (definitionRecord == null)
        {
            definitionRecord = new NotificationDefinitionGroupRecord(
                GuidGenerator.Create(),
                name,
                input.DisplayName);
            UpdateByInput(definitionRecord, input);

            definitionRecord = await _definitionRecordRepository.InsertAsync(definitionRecord);
        }
        else
        {
            UpdateByInput(definitionRecord, input);
            definitionRecord = await _definitionRecordRepository.UpdateAsync(definitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected virtual void UpdateByInput(NotificationDefinitionGroupRecord record, NotificationGroupDefinitionCreateOrUpdateDto input)
    {
        record.AllowSubscriptionToClients = input.AllowSubscriptionToClients;
        record.ExtraProperties.Clear();
        foreach (var property in input.ExtraProperties)
        {
            record.SetProperty(property.Key, property.Value);
        }

        if (!string.Equals(record.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DisplayName = input.DisplayName;
        }
        if (!string.Equals(record.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
        {
            record.Description = input.Description;
        }
    }

    protected async virtual Task<NotificationDefinitionGroupRecord> FindByNameAsync(string name)
    {
        var definitionRecord = await _definitionRecordRepository.FindByNameAsync(name);

        return definitionRecord;
    }

    protected virtual NotificationGroupDefinitionDto DefinitionRecordToDto(NotificationDefinitionGroupRecord definitionRecord)
    {
        var dto = new NotificationGroupDefinitionDto
        {
            IsStatic = false,
            Name = definitionRecord.Name,
            DisplayName = definitionRecord.DisplayName,
            Description = definitionRecord.Description,
            AllowSubscriptionToClients = definitionRecord.AllowSubscriptionToClients,
        };

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }

    protected virtual NotificationGroupDefinitionDto DefinitionToDto(NotificationGroupDefinition definition, bool isStatic = false)
    {
        var dto = new NotificationGroupDefinitionDto
        {
            IsStatic = isStatic,
            Name = definition.Name,
            AllowSubscriptionToClients = definition.AllowSubscriptionToClients,
            DisplayName = _localizableStringSerializer.Serialize(definition.DisplayName),
        };

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
