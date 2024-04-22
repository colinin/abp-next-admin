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

namespace LINGYUN.Abp.Notifications.Definitions.Notifications;

[Authorize(NotificationsPermissions.Definition.Default)]
public class NotificationDefinitionAppService : AbpNotificationsApplicationServiceBase, INotificationDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly INotificationDefinitionManager _definitionManager;
    private readonly IStaticNotificationDefinitionStore _staticDefinitionStore;
    private readonly IDynamicNotificationDefinitionStore _dynamicDefinitionStore;
    private readonly INotificationDefinitionRecordRepository _definitionRecordRepository;

    public NotificationDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        INotificationDefinitionManager definitionManager,
        IStaticNotificationDefinitionStore staticDefinitionStore,
        IDynamicNotificationDefinitionStore dynamicDefinitionStore, 
        INotificationDefinitionRecordRepository definitionRecordRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _definitionManager = definitionManager;
        _staticDefinitionStore = staticDefinitionStore;
        _dynamicDefinitionStore = dynamicDefinitionStore;
        _definitionRecordRepository = definitionRecordRepository;
    }

    [Authorize(NotificationsPermissions.Definition.Create)]
    public async virtual Task<NotificationDefinitionDto> CreateAsync(NotificationDefinitionCreateDto input)
    {
        if (await _staticDefinitionStore.GetGroupOrNullAsync(input.GroupName) != null)
        {
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), input.GroupName);
        }

        if (await _staticDefinitionStore.GetOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(NotificationsErrorCodes.Definition.AlreayNameExists)
                .WithData(nameof(NotificationDefinitionRecord.Name), input.Name);
        }

        if (await _definitionRecordRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(NotificationsErrorCodes.Definition.AlreayNameExists)
                .WithData(nameof(NotificationDefinitionRecord.Name), input.Name);
        }

        var groupDefinition = await _definitionManager.GetGroupOrNullAsync(input.GroupName);
        if (groupDefinition == null)
        {
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), input.GroupName);
        }

        var definitionRecord = new NotificationDefinitionRecord(
            GuidGenerator.Create(),
            input.Name,
            groupDefinition.Name,
            input.DisplayName,
            input.Description,
            input.Template,
            input.NotificationLifetime,
            input.NotificationType,
            input.ContentType);

        UpdateByInput(definitionRecord, input);

        await _definitionRecordRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    [Authorize(NotificationsPermissions.Definition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindRecordByNameAsync(name);

        if (definitionRecord != null)
        {
            await _definitionRecordRepository.DeleteAsync(definitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<NotificationDefinitionDto> GetAsync(string name)
    {
        var definition = await _staticDefinitionStore.GetOrNullAsync(name);
        if (definition != null)
        {
            return DefinitionToDto(await GetGroupDefinition(definition), definition, true);
        }
        definition = await _dynamicDefinitionStore.GetOrNullAsync(name);
        return DefinitionToDto(await GetGroupDefinition(definition), definition);
    }

    public async virtual Task<ListResultDto<NotificationDefinitionDto>> GetListAsync(NotificationDefinitionGetListInput input)
    {
        var dtoList = new List<NotificationDefinitionDto>();

        var staticDefinitions = new List<NotificationDefinition>();

        var staticGroups = await _staticDefinitionStore.GetGroupsAsync();
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();
        foreach (var group in staticGroups.WhereIf(!input.GroupName.IsNullOrWhiteSpace(), x => x.Name == input.GroupName))
        {
            var definitions = group.Notifications;
            staticDefinitions.AddRange(definitions);
            dtoList.AddRange(definitions.Select(f => DefinitionToDto(group, f, true)));
        }
        var staticDefinitionNames = staticDefinitions
            .Select(p => p.Name)
            .ToImmutableHashSet();
        var dynamicGroups = await _dynamicDefinitionStore.GetGroupsAsync();
        foreach (var group in dynamicGroups
            .Where(d => !staticGroupNames.Contains(d.Name))
            .WhereIf(!input.GroupName.IsNullOrWhiteSpace(), x => x.Name == input.GroupName))
        {
            var definitions = group.Notifications;
            dtoList.AddRange(definitions
                .Where(d => !staticDefinitionNames.Contains(d.Name))
                .Select(f => DefinitionToDto(group, f)));
        }

        return new ListResultDto<NotificationDefinitionDto>(dtoList
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter))
            .WhereIf(!input.Template.IsNullOrWhiteSpace(), x => x.Template == input.Template)
            .WhereIf(input.AllowSubscriptionToClients.HasValue, x => x.AllowSubscriptionToClients == input.AllowSubscriptionToClients)
            .WhereIf(input.ContentType.HasValue, x => x.ContentType == input.ContentType)
            .WhereIf(input.NotificationLifetime.HasValue, x => x.NotificationLifetime == input.NotificationLifetime)
            .WhereIf(input.NotificationType.HasValue, x => x.NotificationType == input.NotificationType)
            .ToList());
    }

    [Authorize(NotificationsPermissions.Definition.Update)]
    public async virtual Task<NotificationDefinitionDto> UpdateAsync(string name, NotificationDefinitionUpdateDto input)
    {
        if (await _staticDefinitionStore.GetOrNullAsync(name) != null)
        {
            throw new BusinessException(NotificationsErrorCodes.Definition.StaticFeatureNotAllowedChanged)
              .WithData(nameof(NotificationDefinitionRecord.Name), name);
        }

        var definition = await _definitionManager.GetAsync(name);
        var definitionRecord = await FindRecordByNameAsync(name);

        if (definitionRecord == null)
        {
            var groupDefinition = await GetGroupDefinition(definition);
            definitionRecord = new NotificationDefinitionRecord(
                GuidGenerator.Create(),
                name,
                groupDefinition.Name,
                input.DisplayName,
                input.Description,
                input.Template,
                input.NotificationLifetime,
                input.NotificationType,
                input.ContentType);
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

    protected virtual void UpdateByInput(NotificationDefinitionRecord record, NotificationDefinitionCreateOrUpdateDto input)
    {
        record.AllowSubscriptionToClients = input.AllowSubscriptionToClients;
        record.NotificationLifetime = input.NotificationLifetime;
        record.NotificationType = input.NotificationType;
        record.ContentType = input.ContentType;

        if (!string.Equals(record.Template, input.Template, StringComparison.InvariantCultureIgnoreCase))
        {
            record.Template = input.Template;
        }
        if (!string.Equals(record.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DisplayName = input.DisplayName;
        }
        if (!string.Equals(record.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
        {
            record.Description = input.Description;
        }
        string allowedProviders = null;
        if (!input.Providers.IsNullOrEmpty())
        {
            allowedProviders = input.Providers.JoinAsString(",");
        }
        if (!string.Equals(record.Providers, allowedProviders, StringComparison.InvariantCultureIgnoreCase))
        {
            record.UseProviders(input.Providers.ToArray());
        }
        record.ExtraProperties.Clear();
        foreach (var property in input.ExtraProperties)
        {
            record.SetProperty(property.Key, property.Value);
        }
    }

    protected async virtual Task<NotificationDefinitionRecord> FindRecordByNameAsync(string name)
    {
        return await _definitionRecordRepository.FindByNameAsync(name);
    }

    protected async virtual Task<NotificationGroupDefinition> GetGroupDefinition(NotificationDefinition definition)
    {
        var groups = await _definitionManager.GetGroupsAsync();

        foreach (var group in groups)
        {
            if (group.GetNotificationOrNull(definition.Name) != null)
            {
                return group;
            }
        }

        throw new BusinessException(NotificationsErrorCodes.Definition.FailedGetGroup)
            .WithData(nameof(NotificationDefinitionRecord.Name), definition.Name);
    }

    protected virtual NotificationDefinitionDto DefinitionRecordToDto(NotificationDefinitionRecord definitionRecord)
    {
        var dto = new NotificationDefinitionDto
        {
            IsStatic = false,
            Name = definitionRecord.Name,
            GroupName = definitionRecord.GroupName,
            Description = definitionRecord.Description,
            DisplayName = definitionRecord.DisplayName,
            Template = definitionRecord.Template,
            ContentType = definitionRecord.ContentType,
            NotificationLifetime = definitionRecord.NotificationLifetime,
            NotificationType = definitionRecord.NotificationType,
            AllowSubscriptionToClients = definitionRecord.AllowSubscriptionToClients,
            Providers = definitionRecord.Providers?.Split(",").ToList(),
        };



        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }

    protected virtual NotificationDefinitionDto DefinitionToDto(NotificationGroupDefinition groupDefinition, NotificationDefinition definition, bool isStatic = false)
    {
        var dto = new NotificationDefinitionDto
        {
            IsStatic = isStatic,
            Name = definition.Name,
            GroupName = groupDefinition.Name,
            Template = definition.Template?.Name,
            AllowSubscriptionToClients = definition.AllowSubscriptionToClients,
            ContentType = definition.ContentType,
            NotificationLifetime = definition.NotificationLifetime,
            NotificationType = definition.NotificationType,
            DisplayName = _localizableStringSerializer.Serialize(definition.DisplayName),
            Providers = definition.Providers,
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
