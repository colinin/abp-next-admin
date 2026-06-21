using LINGYUN.Abp.WebhooksManagement.Authorization;
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

[Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Default)]
public class WebhookGroupDefinitionAppService : WebhooksManagementAppServiceBase, IWebhookGroupDefinitionAppService
{
    private readonly IWebhookGroupDefinitionRecordRepository _webhookGroupDefinitionRecordRepository;

    public WebhookGroupDefinitionAppService(
        IWebhookGroupDefinitionRecordRepository webhookGroupDefinitionRecordRepository)
    {
        _webhookGroupDefinitionRecordRepository = webhookGroupDefinitionRecordRepository;
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Create)]
    public async virtual Task<WebhookGroupDefinitionDto> CreateAsync(WebhookGroupDefinitionCreateDto input)
    {
        var webhookGroupDefinitionRecord = await _webhookGroupDefinitionRecordRepository.FindByNameAsync(input.Name);
        if (webhookGroupDefinitionRecord != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.AlreayNameExists)
                .WithData(nameof(WebhookGroupDefinitionRecord.Name), input.Name);
        }

        webhookGroupDefinitionRecord = new WebhookGroupDefinitionRecord(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName);

        foreach (var property in input.ExtraProperties)
        {
            webhookGroupDefinitionRecord.SetProperty(property.Key, property.Value);
        }
        webhookGroupDefinitionRecord.SetProperty(nameof(WebhookGroupDefinitionDto.IsStatic), false);

        await _webhookGroupDefinitionRecordRepository.InsertAsync(webhookGroupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(webhookGroupDefinitionRecord);
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.NameNotFount)
                .WithData(nameof(WebhookGroupDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);
        await _webhookGroupDefinitionRecordRepository.DeleteAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<WebhookGroupDefinitionDto> GetAsync(string name)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.NameNotFount)
                .WithData(nameof(WebhookGroupDefinitionRecord.Name), name);

        return DefinitionRecordToDto(definitionRecord);
    }

    public async virtual Task<ListResultDto<WebhookGroupDefinitionDto>> GetListAsync(WebhookGroupDefinitionGetListInput input)
    {
        var groupDtoList = new List<WebhookGroupDefinitionDto>();

        Expression<Func<WebhookGroupDefinitionRecord, bool>> expression = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Name.Contains(input.Filter));
        }

        var definitionRecords = await _webhookGroupDefinitionRecordRepository.GetListAsync(
            new Volo.Abp.Specifications.ExpressionSpecification<WebhookGroupDefinitionRecord>(expression));

        groupDtoList.AddRange(definitionRecords.Select(DefinitionRecordToDto));

        return new ListResultDto<WebhookGroupDefinitionDto>(groupDtoList);
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Update)]
    public async virtual Task<WebhookGroupDefinitionDto> UpdateAsync(string name, WebhookGroupDefinitionUpdateDto input)
    {
        var definitionRecord = await FindByNameAsync(name) ??
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.NameNotFount)
                .WithData(nameof(WebhookGroupDefinitionRecord.Name), name);

        CheckIsStaticDefinitionRecord(definitionRecord);

        definitionRecord.ExtraProperties.Clear();
        foreach (var property in input.ExtraProperties)
        {
            definitionRecord.SetProperty(property.Key, property.Value);
        }

        if (!string.Equals(definitionRecord.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            definitionRecord.DisplayName = input.DisplayName;
        }

        definitionRecord = await _webhookGroupDefinitionRecordRepository.UpdateAsync(definitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(definitionRecord);
    }

    protected async virtual Task<WebhookGroupDefinitionRecord> FindByNameAsync(string name)
    {
        return await _webhookGroupDefinitionRecordRepository.FindByNameAsync(name);
    }

    protected virtual void CheckIsStaticDefinitionRecord(WebhookGroupDefinitionRecord record)
    {
        if (record.GetProperty(nameof(WebhookGroupDefinitionDto.IsStatic), true))
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.StaticGroupNotAllowedChanged)
              .WithData("Name", record.Name);
        }
    }

    protected virtual WebhookGroupDefinitionDto DefinitionRecordToDto(WebhookGroupDefinitionRecord webhookGroupDefinitionRecord)
    {
        var webhookGroupDto = new WebhookGroupDefinitionDto
        {
            Name = webhookGroupDefinitionRecord.Name,
            DisplayName = webhookGroupDefinitionRecord.DisplayName,
            IsStatic = webhookGroupDefinitionRecord.GetProperty(nameof(WebhookGroupDefinitionDto.IsStatic), true),
        };

        foreach (var property in webhookGroupDefinitionRecord.ExtraProperties)
        {
            webhookGroupDto.SetProperty(property.Key, property.Value);
        }

        return webhookGroupDto;
    }
}
