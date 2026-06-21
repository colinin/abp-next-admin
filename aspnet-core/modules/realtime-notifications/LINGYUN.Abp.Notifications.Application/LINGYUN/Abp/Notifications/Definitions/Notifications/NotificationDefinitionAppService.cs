using LINGYUN.Abp.Notifications.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace LINGYUN.Abp.Notifications.Definitions.Notifications;

[Authorize(NotificationsPermissions.Definition.Default)]
public class NotificationDefinitionAppService : AbpNotificationsApplicationServiceBase, INotificationDefinitionAppService
{
    private readonly INotificationDefinitionRecordRepository _definitionRecordRepository;
    private readonly INotificationDefinitionGroupRecordRepository _definitionGroupRecordRepository;

    public NotificationDefinitionAppService(
        INotificationDefinitionRecordRepository definitionRecordRepository,
        INotificationDefinitionGroupRecordRepository definitionGroupRecordRepository)
    {
        _definitionRecordRepository = definitionRecordRepository;
        _definitionGroupRecordRepository = definitionGroupRecordRepository;
    }

    [Authorize(NotificationsPermissions.Definition.Create)]
    public async virtual Task<NotificationDefinitionDto> CreateAsync(NotificationDefinitionCreateDto input)
    {
        if (await _definitionRecordRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(NotificationsErrorCodes.Definition.AlreayNameExists)
                .WithData(nameof(NotificationDefinitionRecord.Name), input.Name);
        }

        var groupDefinition = await _definitionGroupRecordRepository.FindByNameAsync(input.GroupName) ??
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), input.GroupName);
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

        definitionRecord.SetProperty(nameof(NotificationDefinitionDto.IsStatic), false);

        await _definitionRecordRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    [Authorize(NotificationsPermissions.Definition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindRecordByNameAsync(name) ??
            throw new BusinessException(NotificationsErrorCodes.Definition.NameNotFount)
                .WithData(nameof(NotificationDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);

        await _definitionRecordRepository.DeleteAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<NotificationDefinitionDto> GetAsync(string name)
    {
        var definitionRecord = await FindRecordByNameAsync(name) ??
            throw new BusinessException(NotificationsErrorCodes.Definition.NameNotFount)
                .WithData(nameof(NotificationDefinitionRecord.Name), name);
        return DefinitionRecordToDto(definitionRecord);
    }

    public async virtual Task<ListResultDto<NotificationDefinitionDto>> GetListAsync(NotificationDefinitionGetListInput input)
    {
        var dtoList = new List<NotificationDefinitionDto>();

        Expression<Func<NotificationDefinitionRecord, bool>> expression = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter));
        }
        if (!input.Template.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Template == input.Template);
        }
        if (input.AllowSubscriptionToClients.HasValue)
        {
            expression = expression.And(x => x.AllowSubscriptionToClients == input.AllowSubscriptionToClients);
        }
        if (input.ContentType.HasValue)
        {
            expression = expression.And(x => x.ContentType == input.ContentType);
        }
        if (input.NotificationLifetime.HasValue)
        {
            expression = expression.And(x => x.NotificationLifetime == input.NotificationLifetime);
        }
        if (input.NotificationType.HasValue)
        {
            expression = expression.And(x => x.NotificationType == input.NotificationType);
        }

        var definitionRecords = await _definitionRecordRepository.GetListAsync(
            new Volo.Abp.Specifications.ExpressionSpecification<NotificationDefinitionRecord>(expression));

        dtoList.AddRange(definitionRecords.Select(DefinitionRecordToDto));

        return new ListResultDto<NotificationDefinitionDto>(dtoList);
    }

    [Authorize(NotificationsPermissions.Definition.Update)]
    public async virtual Task<NotificationDefinitionDto> UpdateAsync(string name, NotificationDefinitionUpdateDto input)
    {
        var definitionRecord = await FindRecordByNameAsync(name) ??
            throw new BusinessException(NotificationsErrorCodes.Definition.NameNotFount)
                .WithData(nameof(NotificationDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);
        UpdateByInput(definitionRecord, input);
        definitionRecord = await _definitionRecordRepository.UpdateAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected virtual void CheckIsStaticDefinitionRecord(NotificationDefinitionRecord record)
    {
        if (record.GetProperty(nameof(NotificationDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(NotificationsErrorCodes.Definition.StaticFeatureNotAllowedChanged)
              .WithData(nameof(NotificationDefinitionGroupRecord.Name), record.Name);
        }
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
            record.Providers = allowedProviders;
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

    protected virtual NotificationDefinitionDto DefinitionRecordToDto(NotificationDefinitionRecord definitionRecord)
    {
        var dto = new NotificationDefinitionDto
        {
            IsStatic = definitionRecord.GetProperty(nameof(NotificationDefinitionDto.IsStatic), true),
            Name = definitionRecord.Name,
            GroupName = definitionRecord.GroupName,
            Description = definitionRecord.Description,
            DisplayName = definitionRecord.DisplayName,
            Template = definitionRecord.Template,
            ContentType = definitionRecord.ContentType,
            NotificationLifetime = definitionRecord.NotificationLifetime,
            NotificationType = definitionRecord.NotificationType,
            AllowSubscriptionToClients = definitionRecord.AllowSubscriptionToClients,
            Providers = definitionRecord.Providers?.Split(",").ToList() ?? [],
        };

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
