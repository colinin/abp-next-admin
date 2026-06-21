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
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.Definitions.Groups;

[Authorize(NotificationsPermissions.GroupDefinition.Default)]
public class NotificationGroupDefinitionAppService : AbpNotificationsApplicationServiceBase, INotificationGroupDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly INotificationDefinitionGroupRecordRepository _definitionGroupRecordRepository;

    public NotificationGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer, 
        INotificationDefinitionGroupRecordRepository definitionGroupRecordRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _definitionGroupRecordRepository = definitionGroupRecordRepository;
    }

    [Authorize(NotificationsPermissions.GroupDefinition.Create)]
    public async virtual Task<NotificationGroupDefinitionDto> CreateAsync(NotificationGroupDefinitionCreateDto input)
    {
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

        definitionRecord.SetProperty(nameof(NotificationGroupDefinitionDto.IsStatic), false);

        await _definitionGroupRecordRepository.InsertAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    [Authorize(NotificationsPermissions.GroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);

        await _definitionGroupRecordRepository.DeleteAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<NotificationGroupDefinitionDto> GetAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), name);
        return DefinitionRecordToDto(definitionRecord);
    }

    public async virtual Task<ListResultDto<NotificationGroupDefinitionDto>> GetListAsync(NotificationGroupDefinitionGetListInput input)
    {
        var definitionDtoList = new List<NotificationGroupDefinitionDto>();

        Expression<Func<NotificationDefinitionGroupRecord, bool>> expression = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Name.Contains(input.Filter));
        }

        var definitionRecords = await _definitionGroupRecordRepository.GetListAsync(
            new Volo.Abp.Specifications.ExpressionSpecification<NotificationDefinitionGroupRecord>(expression));

        definitionDtoList.AddRange(definitionRecords.Select(DefinitionRecordToDto));

        return new ListResultDto<NotificationGroupDefinitionDto>(definitionDtoList);
    }

    [Authorize(NotificationsPermissions.GroupDefinition.Update)]
    public async virtual Task<NotificationGroupDefinitionDto> UpdateAsync(string name, NotificationGroupDefinitionUpdateDto input)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.NameNotFount)
                .WithData(nameof(NotificationDefinitionGroupRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);
        UpdateByInput(definitionRecord, input);
        definitionRecord = await _definitionGroupRecordRepository.UpdateAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected virtual void CheckIsStaticDefinitionRecord(NotificationDefinitionGroupRecord record)
    {
        if (record.GetProperty(nameof(NotificationGroupDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(NotificationsErrorCodes.GroupDefinition.StaticGroupNotAllowedChanged)
              .WithData(nameof(NotificationDefinitionGroupRecord.Name), record.Name);
        }
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
        var definitionRecord = await _definitionGroupRecordRepository.FindByNameAsync(name);

        return definitionRecord;
    }

    protected virtual NotificationGroupDefinitionDto DefinitionRecordToDto(NotificationDefinitionGroupRecord definitionRecord)
    {
        var dto = new NotificationGroupDefinitionDto
        {
            IsStatic = definitionRecord.GetProperty(nameof(NotificationGroupDefinitionDto.IsStatic), true),
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
}
