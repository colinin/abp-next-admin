using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Definitions.Dto;
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

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

[Authorize(WebhooksManagementPermissions.WebhookDefinition.Default)]
public class WebhookDefinitionAppService : WebhooksManagementAppServiceBase, IWebhookDefinitionAppService
{
    private readonly IWebhookDefinitionRecordRepository _webhookDefinitionRecordRepository;
    private readonly IWebhookGroupDefinitionRecordRepository _webhookGroupDefinitionRecordRepository;

    public WebhookDefinitionAppService(
        IWebhookDefinitionRecordRepository webhookDefinitionRecordRepository,
        IWebhookGroupDefinitionRecordRepository webhookGroupDefinitionRecordRepository)
    {
        _webhookDefinitionRecordRepository = webhookDefinitionRecordRepository;
        _webhookGroupDefinitionRecordRepository = webhookGroupDefinitionRecordRepository;
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Create)]
    public async virtual Task<WebhookDefinitionDto> CreateAsync(WebhookDefinitionCreateDto input)
    {
        var webhookDefinitionRecord = await _webhookDefinitionRecordRepository.FindByNameAsync(input.Name);
        if (webhookDefinitionRecord != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.AlreayNameExists)
               .WithData("Name", input.Name);
        }
        var webhookDefinitionGroupRecord = await _webhookGroupDefinitionRecordRepository.FindByNameAsync(input.GroupName)
            ?? throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.StaticGroupNotAllowedChanged)
                .WithData(nameof(WebhookDefinitionRecord.Name), input.GroupName);

        webhookDefinitionRecord = new WebhookDefinitionRecord(
            GuidGenerator.Create(),
            webhookDefinitionGroupRecord.Name,
            input.Name,
            input.DisplayName);
        UpdateByInput(webhookDefinitionRecord, input);

        webhookDefinitionRecord.SetProperty(nameof(WebhookDefinitionDto.IsStatic), false);

        await _webhookDefinitionRecordRepository.InsertAsync(webhookDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(webhookDefinitionRecord);
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.NameNotFount)
                .WithData(nameof(WebhookDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);
        await _webhookDefinitionRecordRepository.DeleteAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<WebhookDefinitionDto> GetAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.NameNotFount)
                .WithData(nameof(WebhookDefinitionRecord.Name), name);

        return DefinitionRecordToDto(definitionRecord);
    }

    public async virtual Task<ListResultDto<WebhookDefinitionDto>> GetListAsync(WebhookDefinitionGetListInput input)
    {
        var webhookDtoList = new List<WebhookDefinitionDto>();

        Expression<Func<WebhookDefinitionRecord, bool>> expression = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter));
        }
        if (!input.GroupName.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.GroupName == input.GroupName);
        }

        var definitionRecords = await _webhookDefinitionRecordRepository.GetListAsync(
            new Volo.Abp.Specifications.ExpressionSpecification<WebhookDefinitionRecord>(expression));

        webhookDtoList.AddRange(definitionRecords.Select(DefinitionRecordToDto));

        return new ListResultDto<WebhookDefinitionDto>(webhookDtoList);
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Update)]
    public async virtual Task<WebhookDefinitionDto> UpdateAsync(string name, WebhookDefinitionUpdateDto input)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.NameNotFount)
                .WithData(nameof(WebhookDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);
        UpdateByInput(definitionRecord, input);
        definitionRecord = await _webhookDefinitionRecordRepository.UpdateAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected async virtual Task<WebhookDefinitionRecord> FindByNameAsync(string name)
    {
        return await _webhookDefinitionRecordRepository.FindByNameAsync(name);
    }

    protected virtual void CheckIsStaticDefinitionRecord(WebhookDefinitionRecord record)
    {
        if (record.GetProperty(nameof(WebhookDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.StaticWebhookNotAllowedChanged)
              .WithData("Name", record.Name);
        }
    }

    protected virtual void UpdateByInput(WebhookDefinitionRecord record, WebhookDefinitionCreateOrUpdateDto input)
    {
        record.IsEnabled = input.IsEnabled;
        record.ExtraProperties.Clear();
        foreach (var property in input.ExtraProperties)
        {
            record.SetProperty(property.Key, property.Value);
        }

        if (!string.Equals(record.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
        {
            record.Description = input.Description;
        }
        if (!string.Equals(record.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            record.DisplayName = input.DisplayName;
        }

        string requiredFeatures = null;
        if (!input.RequiredFeatures.IsNullOrEmpty())
        {
            requiredFeatures = input.RequiredFeatures.JoinAsString(",");
        }
        if (!string.Equals(record.RequiredFeatures, requiredFeatures, StringComparison.InvariantCultureIgnoreCase))
        {
            record.RequiredFeatures = requiredFeatures;
        }
    }

    protected virtual WebhookDefinitionDto DefinitionRecordToDto(WebhookDefinitionRecord record)
    {
        var webhookDto = new WebhookDefinitionDto
        {
            IsStatic = record.GetProperty(nameof(WebhookDefinitionDto.IsStatic), true),
            Description = record.Description,
            DisplayName = record.DisplayName,
            GroupName = record.GroupName,
            IsEnabled = record.IsEnabled,
            Name = record.Name,
            RequiredFeatures = [],
        };

        if (!record.RequiredFeatures.IsNullOrWhiteSpace())
        {
            webhookDto.RequiredFeatures = record.RequiredFeatures.Split(',').ToList();
        }

        foreach (var property in record.ExtraProperties)
        {
            webhookDto.SetProperty(property.Key, property.Value);
        }

        return webhookDto;
    }
}
