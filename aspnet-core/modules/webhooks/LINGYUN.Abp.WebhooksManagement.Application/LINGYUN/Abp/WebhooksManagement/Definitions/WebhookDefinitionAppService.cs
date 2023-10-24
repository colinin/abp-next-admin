using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Definitions.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

[Authorize(WebhooksManagementPermissions.WebhookDefinition.Default)]
public class WebhookDefinitionAppService : WebhooksManagementAppServiceBase, IWebhookDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IWebhookDefinitionManager _webhookDefinitionManager;
    private readonly IStaticWebhookDefinitionStore _staticWebhookDefinitionStore;
    private readonly IWebhookDefinitionRecordRepository _webhookDefinitionRecordRepository;

    public WebhookDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        IWebhookDefinitionManager webhookDefinitionManager,
        IWebhookDefinitionRecordRepository webhookDefinitionRecordRepository,
        IStaticWebhookDefinitionStore staticWebhookDefinitionStore)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _webhookDefinitionManager = webhookDefinitionManager;
        _webhookDefinitionRecordRepository = webhookDefinitionRecordRepository;
        _staticWebhookDefinitionStore = staticWebhookDefinitionStore;
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Create)]
    public async virtual Task<WebhookDefinitionDto> CreateAsync(WebhookDefinitionCreateDto input)
    {
        if (await _webhookDefinitionManager.GetOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.AlreayNameExists)
                .WithData(nameof(WebhookDefinitionRecord.Name), input.Name);
        }

        var webhookDefinitionRecord = await _webhookDefinitionRecordRepository.FindByNameAsync(input.Name);
        if (webhookDefinitionRecord != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.AlreayNameExists)
               .WithData("Name", input.Name);
        }

        webhookDefinitionRecord = new WebhookDefinitionRecord(
            GuidGenerator.Create(),
            input.GroupName,
            input.Name,
            input.DisplayName);
        UpdateByInput(webhookDefinitionRecord, input);

        await _webhookDefinitionRecordRepository.InsertAsync(webhookDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(webhookDefinitionRecord);
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var webhookDefinitionRecord = await _webhookDefinitionRecordRepository.FindByNameAsync(name);
        if (webhookDefinitionRecord != null)
        {
            await _webhookDefinitionRecordRepository.DeleteAsync(webhookDefinitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<WebhookDefinitionDto> GetAsync(string name)
    {
        var webhookDefinition = await _staticWebhookDefinitionStore.GetOrNullAsync(name);
        if (webhookDefinition != null)
        {
            return DefinitionToDto(webhookDefinition, true);
        }
        var webhookDefinitionRecord = await _webhookDefinitionRecordRepository.GetByNameAsync(name);
        return DefinitionRecordToDto(webhookDefinitionRecord);
    }

    public async virtual Task<ListResultDto<WebhookDefinitionDto>> GetListAsync(WebhookDefinitionGetListInput input)
    {
        var webhookDtoList = new List<WebhookDefinitionDto>();
        var staticWebhooks = await _staticWebhookDefinitionStore.GetWebhooksAsync();
        var staticWebhookNames = staticWebhooks
            .Select(p => p.Name)
            .ToImmutableHashSet();
        webhookDtoList.AddRange(staticWebhooks.Select(d => DefinitionToDto(d, true, true)));

        var dynamicWebhooks = await _webhookDefinitionRecordRepository.GetListAsync();
        webhookDtoList.AddRange(dynamicWebhooks
            .Where(d => !staticWebhookNames.Contains(d.Name))
            .Select(d => DefinitionRecordToDto(d)));

        return new ListResultDto<WebhookDefinitionDto>(webhookDtoList
            .WhereIf(!input.GroupName.IsNullOrWhiteSpace(), x => x.GroupName.Equals(input.GroupName))
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter))
            .ToList());
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Update)]
    public async virtual Task<WebhookDefinitionDto> UpdateAsync(string name, WebhookDefinitionUpdateDto input)
    {
        if (await _staticWebhookDefinitionStore.GetOrNullAsync(name) != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.StaticWebhookNotAllowedChanged)
              .WithData("Name", name);
        }

        var webhookDefinition = await _webhookDefinitionManager.GetAsync(name);
        var webhookDefinitionRecord = await _webhookDefinitionRecordRepository.FindByNameAsync(name);

        if (webhookDefinitionRecord == null)
        {
            webhookDefinitionRecord = new WebhookDefinitionRecord(
                GuidGenerator.Create(),
                webhookDefinition.GroupName,
                name,
                input.DisplayName);
            UpdateByInput(webhookDefinitionRecord, input);

            webhookDefinitionRecord = await _webhookDefinitionRecordRepository.InsertAsync(webhookDefinitionRecord);
        }
        else
        {
            UpdateByInput(webhookDefinitionRecord, input);
            webhookDefinitionRecord = await _webhookDefinitionRecordRepository.UpdateAsync(webhookDefinitionRecord);
        }
        
        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(webhookDefinitionRecord);
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
            IsStatic = false,
            Description = record.Description,
            DisplayName = record.DisplayName,
            GroupName = record.GroupName,
            IsEnabled = record.IsEnabled,
            Name = record.Name,
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

    protected virtual WebhookDefinitionDto DefinitionToDto(WebhookDefinition definition, bool isStatic = false, bool isEnabled = true)
    {
        var webhookDto = new WebhookDefinitionDto
        {
            GroupName = definition.GroupName,
            Name = definition.Name,
            IsStatic = isStatic,
            IsEnabled = isEnabled,
            RequiredFeatures = definition.RequiredFeatures,
            DisplayName = _localizableStringSerializer.Serialize(definition.DisplayName),
        };

        if (definition.Description != null)
        {
            webhookDto.Description = _localizableStringSerializer.Serialize(definition.Description);
        }

        foreach (var property in definition.Properties)
        {
            webhookDto.SetProperty(property.Key, property.Value);
        }

        return webhookDto;
    }
}
