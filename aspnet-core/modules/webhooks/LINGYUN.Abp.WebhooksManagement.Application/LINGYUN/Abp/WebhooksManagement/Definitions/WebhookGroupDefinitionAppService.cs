using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
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

[Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Default)]
public class WebhookGroupDefinitionAppService : WebhooksManagementAppServiceBase, IWebhookGroupDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IWebhookDefinitionManager _webhookDefinitionManager;

    private readonly IWebhookGroupDefinitionRecordRepository _webhookGroupDefinitionRecordRepository;

    public WebhookGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        IWebhookDefinitionManager webhookDefinitionManager,
        IWebhookGroupDefinitionRecordRepository webhookGroupDefinitionRecordRepository)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _webhookDefinitionManager = webhookDefinitionManager;
        _webhookGroupDefinitionRecordRepository = webhookGroupDefinitionRecordRepository;
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Create)]
    public async virtual Task<WebhookGroupDefinitionDto> CreateAsync(WebhookGroupDefinitionCreateDto input)
    {
        if (await _webhookDefinitionManager.GetGroupOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.AlreayNameExists)
                .WithData(nameof(WebhookGroupDefinitionRecord.Name), input.Name);
        }

        var webhookGroupDefinitionRecord = new WebhookGroupDefinitionRecord(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName);

        foreach (var property in input.ExtraProperties)
        {
            webhookGroupDefinitionRecord.SetProperty(property.Key, property.Value);
        }

        await _webhookGroupDefinitionRecordRepository.InsertAsync(webhookGroupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await WebhookGroupDefinitionRecordToDto(webhookGroupDefinitionRecord);

        return dto;
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Delete)]
    public async virtual Task DeleteAysnc(string name)
    {
        var webhookGroupDefinitionRecord = await _webhookGroupDefinitionRecordRepository.FindByNameAsync(name);
        if (webhookGroupDefinitionRecord == null)
        {
            return;
        }

        await _webhookGroupDefinitionRecordRepository.DeleteAsync(webhookGroupDefinitionRecord);
    }

    public async virtual Task<WebhookGroupDefinitionDto> GetAsync(string name)
    {
        var webhookGroupDefinition = await _webhookDefinitionManager.GetGroupAsync(name);

        var dto = await WebhookGroupDefinitionToDto(webhookGroupDefinition);

        return dto;
    }

    public async virtual Task<PagedResultDto<WebhookGroupDefinitionDto>> GetListAsync(WebhookGroupDefinitionGetListInput input)
    {
        var webhookGroups = new List<WebhookGroupDefinitionDto>();

        var webhookGroupDefinitions = await _webhookDefinitionManager.GetGroupsAsync();

        var webhookGroupDefinitionFilter = webhookGroupDefinitions.AsQueryable()
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter));

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(WebhookGroupDefinitionRecord.Name);
        }

        var filterGroupDefinitionCount = webhookGroupDefinitionFilter.Count();
        var filterGroupDefinitions = webhookGroupDefinitionFilter
            .OrderBy(sorting)
            .PageBy(input.SkipCount, input.MaxResultCount);

        foreach (var webhookDefinition in filterGroupDefinitions)
        {
            var webhookGroupDto = await WebhookGroupDefinitionToDto(webhookDefinition);

            webhookGroups.Add(webhookGroupDto);
        }

        return new PagedResultDto<WebhookGroupDefinitionDto>(filterGroupDefinitionCount, webhookGroups);
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Update)]
    public async virtual Task<WebhookGroupDefinitionDto> UpdateAsync(string name, WebhookGroupDefinitionUpdateDto input)
    {
        var webhookGroupDefinition = await _webhookDefinitionManager.GetGroupAsync(name);
        var webhookGroupDefinitionRecord = await _webhookGroupDefinitionRecordRepository.FindByNameAsync(name);

        if (webhookGroupDefinitionRecord == null)
        {
            webhookGroupDefinitionRecord = new WebhookGroupDefinitionRecord(
                GuidGenerator.Create(),
                name,
                input.DisplayName);

            foreach (var property in input.ExtraProperties)
            {
                webhookGroupDefinitionRecord.SetProperty(property.Key, property.Value);
            }

            webhookGroupDefinitionRecord = await _webhookGroupDefinitionRecordRepository.InsertAsync(webhookGroupDefinitionRecord);
        }
        else
        {
            webhookGroupDefinitionRecord.ExtraProperties.Clear();
            foreach (var property in input.ExtraProperties)
            {
                webhookGroupDefinitionRecord.SetProperty(property.Key, property.Value);
            }

            if (!string.Equals(webhookGroupDefinitionRecord.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                webhookGroupDefinitionRecord.DisplayName = input.DisplayName;
            }

            webhookGroupDefinitionRecord = await _webhookGroupDefinitionRecordRepository.UpdateAsync(webhookGroupDefinitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = await WebhookGroupDefinitionRecordToDto(webhookGroupDefinitionRecord);

        if (webhookGroupDefinition.Properties.TryGetValue("IsStatic", out var isStaticObj) &&
            isStaticObj is bool isStatic)
        {
            dto.IsStatic = isStatic;
        }

        return dto;
    }

    protected async virtual Task<WebhookGroupDefinitionDto> WebhookGroupDefinitionRecordToDto(WebhookGroupDefinitionRecord webhookGroupDefinitionRecord)
    {
        var webhookGroupDto = new WebhookGroupDefinitionDto
        {
            Name = webhookGroupDefinitionRecord.Name,
            FormatedDisplayName = webhookGroupDefinitionRecord.DisplayName,
            IsStatic = false,
        };

        var displayName = _localizableStringSerializer.Deserialize(webhookGroupDefinitionRecord.DisplayName);
        webhookGroupDto.DisplayName = await displayName.LocalizeAsync(StringLocalizerFactory);

        foreach (var property in webhookGroupDefinitionRecord.ExtraProperties)
        {
            webhookGroupDto.SetProperty(property.Key, property.Value);
        }

        return webhookGroupDto;
    }

    protected async virtual Task<WebhookGroupDefinitionDto> WebhookGroupDefinitionToDto(WebhookGroupDefinition webhookGroupDefinition)
    {
        var webhookGroupDto = new WebhookGroupDefinitionDto
        {
            Name = webhookGroupDefinition.Name
        };

        if (webhookGroupDefinition.DisplayName != null)
        {
            webhookGroupDto.DisplayName = await webhookGroupDefinition.DisplayName.LocalizeAsync(StringLocalizerFactory);
            webhookGroupDto.FormatedDisplayName = _localizableStringSerializer.Serialize(webhookGroupDefinition.DisplayName);
        }

        if (webhookGroupDefinition.Properties.TryGetValue("IsStatic", out var isStaticObj) &&
            isStaticObj is bool isStatic)
        {
            webhookGroupDto.IsStatic = isStatic;
        }

        var ignoreFiledNames = new string[]
        {
            "IsStatic"
        };

        foreach (var property in webhookGroupDefinition.Properties.Where(x => !ignoreFiledNames.Contains(x.Key)))
        {
            webhookGroupDto.SetProperty(property.Key, property.Value);
        }

        return webhookGroupDto;
    }
}
