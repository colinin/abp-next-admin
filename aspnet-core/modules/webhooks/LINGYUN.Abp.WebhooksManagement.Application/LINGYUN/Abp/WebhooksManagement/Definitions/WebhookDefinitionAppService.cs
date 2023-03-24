using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Definitions.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
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

    private readonly IWebhookDefinitionRecordRepository _webhookDefinitionRecordRepository;

    public WebhookDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        IWebhookDefinitionManager webhookDefinitionManager,
        IWebhookDefinitionRecordRepository webhookDefinitionRecordRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _webhookDefinitionManager = webhookDefinitionManager;
        _webhookDefinitionRecordRepository = webhookDefinitionRecordRepository;
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Create)]
    public async virtual Task<WebhookDefinitionDto> CreateAsync(WebhookDefinitionCreateDto input)
    {
        if (await _webhookDefinitionManager.GetOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookDefinition.AlreayNameExists)
                .WithData(nameof(WebhookDefinitionRecord.Name), input.Name);
        }

        var webhookDefinitionRecord = new WebhookDefinitionRecord(
            GuidGenerator.Create(),
            input.GroupName,
            input.Name,
            input.DisplayName,
            input.Description,
            input.IsEnabled,
            input.RequiredFeatures);

        foreach (var property in input.ExtraProperties)
        {
            webhookDefinitionRecord.SetProperty(property.Key, property.Value);
        }

        await _webhookDefinitionRecordRepository.InsertAsync(webhookDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await WebhookDefinitionRecordToDto(webhookDefinitionRecord);

        return dto;
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var webhookDefinitionRecord = await _webhookDefinitionRecordRepository.FindByNameAsync(name);
        if (webhookDefinitionRecord == null)
        {
            return;
        }

        await _webhookDefinitionRecordRepository.DeleteAsync(webhookDefinitionRecord);
    }

    public async virtual Task<WebhookDefinitionDto> GetAsync(string name)
    {
        var webhookDefinition = await _webhookDefinitionManager.GetAsync(name);

        var dto = await WebhookDefinitionToDto(webhookDefinition);

        return dto;
    }

    public async virtual Task<PagedResultDto<WebhookDefinitionDto>> GetListAsync(WebhookDefinitionGetListInput input)
    {
        var webhooks = new List<WebhookDefinitionDto>();

        var webhookDefinitions = await _webhookDefinitionManager.GetWebhooksAsync();

        var webhookDefinitionFilter = webhookDefinitions.AsQueryable()
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>
                x.Name.Contains(input.Filter) || x.GroupName.Contains(input.Filter));

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(WebhookDefinitionRecord.Name);
        }

        var filterDefinitionCount = webhookDefinitionFilter.Count();
        var filterDefinitions = webhookDefinitionFilter
            .OrderBy(sorting)
            .PageBy(input.SkipCount, input.MaxResultCount);

        foreach (var webhookDefinition in filterDefinitions)
        {
            var webhookDto = await WebhookDefinitionToDto(webhookDefinition);

            webhooks.Add(webhookDto);
        }

        return new PagedResultDto<WebhookDefinitionDto>(filterDefinitionCount, webhooks);
    }

    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Update)]
    public async virtual Task<WebhookDefinitionDto> UpdateAsync(string name, WebhookDefinitionUpdateDto input)
    {
        var webhookDefinition = await _webhookDefinitionManager.GetAsync(name);
        var webhookDefinitionRecord = await _webhookDefinitionRecordRepository.FindByNameAsync(name);

        if (webhookDefinitionRecord == null)
        {
            webhookDefinitionRecord = new WebhookDefinitionRecord(
                GuidGenerator.Create(),
                webhookDefinition.GroupName,
                name,
                input.DisplayName,
                input.Description,
                input.IsEnabled,
                input.RequiredFeatures);

            foreach (var property in input.ExtraProperties)
            {
                webhookDefinitionRecord.SetProperty(property.Key, property.Value);
            }

            webhookDefinitionRecord = await _webhookDefinitionRecordRepository.InsertAsync(webhookDefinitionRecord);
        }
        else
        {
            webhookDefinitionRecord.ExtraProperties.Clear();
            foreach (var property in input.ExtraProperties)
            {
                webhookDefinitionRecord.SetProperty(property.Key, property.Value);
            }

            if (!string.Equals(webhookDefinitionRecord.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                webhookDefinitionRecord.Description = input.Description;
            }
            if (!string.Equals(webhookDefinitionRecord.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                webhookDefinitionRecord.DisplayName = input.DisplayName;
            }
            if (!string.Equals(webhookDefinitionRecord.RequiredFeatures, input.RequiredFeatures, StringComparison.InvariantCultureIgnoreCase))
            {
                webhookDefinitionRecord.RequiredFeatures = input.RequiredFeatures;
            }
            webhookDefinitionRecord.IsEnabled = input.IsEnabled;

            webhookDefinitionRecord = await _webhookDefinitionRecordRepository.UpdateAsync(webhookDefinitionRecord);
        }
        
        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await WebhookDefinitionRecordToDto(webhookDefinitionRecord);

        if (webhookDefinition.Properties.TryGetValue("IsStatic", out var isStaticObj) &&
            isStaticObj is bool isStatic)
        {
            dto.IsStatic = isStatic;
        }

        return dto;
    }

    protected async virtual Task<WebhookDefinitionDto> WebhookDefinitionRecordToDto(WebhookDefinitionRecord webhookDefinitionRecord)
    {
        var webhookDto = new WebhookDefinitionDto
        {
            FormatedDescription = webhookDefinitionRecord.Description,
            FormatedDisplayName = webhookDefinitionRecord.DisplayName,
            GroupName = webhookDefinitionRecord.GroupName,
            IsEnabled = webhookDefinitionRecord.IsEnabled,
            Name = webhookDefinitionRecord.Name,
            RequiredFeatures = webhookDefinitionRecord.RequiredFeatures,
        };

        var displayName = _localizableStringSerializer.Deserialize(webhookDefinitionRecord.DisplayName);
        webhookDto.DisplayName = await displayName.LocalizeAsync(StringLocalizerFactory);

        if (!webhookDefinitionRecord.Description.IsNullOrWhiteSpace())
        {
            var description = _localizableStringSerializer.Deserialize(webhookDefinitionRecord.Description);
            webhookDto.Description = await description.LocalizeAsync(StringLocalizerFactory);
        }

        foreach (var property in webhookDefinitionRecord.ExtraProperties)
        {
            webhookDto.SetProperty(property.Key, property.Value);
        }

        return webhookDto;
    }

    protected async virtual Task<WebhookDefinitionDto> WebhookDefinitionToDto(WebhookDefinition webhookDefinition)
    {
        var webhookDto = new WebhookDefinitionDto
        {
            GroupName = webhookDefinition.GroupName,
            Name = webhookDefinition.Name,
            RequiredFeatures = webhookDefinition.RequiredFeatures.JoinAsString(";"),
        };

        if (webhookDefinition.DisplayName != null)
        {
            webhookDto.DisplayName = await webhookDefinition.DisplayName.LocalizeAsync(StringLocalizerFactory);
            webhookDto.FormatedDisplayName = _localizableStringSerializer.Serialize(webhookDefinition.DisplayName);
        }

        if (webhookDefinition.Description != null)
        {
            webhookDto.Description = await webhookDefinition.Description.LocalizeAsync(StringLocalizerFactory);
            webhookDto.FormatedDescription = _localizableStringSerializer.Serialize(webhookDefinition.Description);
        }

        if (webhookDefinition.Properties.TryGetValue(nameof(WebhookDefinitionRecord.IsEnabled), out var isEnabledObj) &&
            isEnabledObj is bool isEnabled)
        {
            webhookDto.IsEnabled = isEnabled;
        }

        if (webhookDefinition.Properties.TryGetValue("IsStatic", out var isStaticObj) &&
            isStaticObj is bool isStatic)
        {
            webhookDto.IsStatic = isStatic;
        }

        var ignoreFiledNames = new string[]
        {
            "IsStatic", nameof(WebhookDefinitionRecord.IsEnabled)
        };

        foreach (var property in webhookDefinition.Properties.Where(x => !ignoreFiledNames.Contains(x.Key)))
        {
            webhookDto.SetProperty(property.Key, property.Value);
        }

        return webhookDto;
    }
}
