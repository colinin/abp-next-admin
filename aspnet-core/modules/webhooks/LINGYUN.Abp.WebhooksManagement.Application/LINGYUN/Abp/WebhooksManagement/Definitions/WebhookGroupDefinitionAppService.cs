using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
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

[Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Default)]
public class WebhookGroupDefinitionAppService : WebhooksManagementAppServiceBase, IWebhookGroupDefinitionAppService
{
    private readonly ILocalizableStringSerializer _localizableStringSerializer;
    private readonly IWebhookDefinitionManager _webhookDefinitionManager;
    private readonly IStaticWebhookDefinitionStore _staticWebhookDefinitionStore;
    private readonly IDynamicWebhookDefinitionStore _dynamicWebhookDefinitionStore;
    private readonly IWebhookGroupDefinitionRecordRepository _webhookGroupDefinitionRecordRepository;

    public WebhookGroupDefinitionAppService(
        ILocalizableStringSerializer localizableStringSerializer,
        IWebhookDefinitionManager webhookDefinitionManager,
        IWebhookGroupDefinitionRecordRepository webhookGroupDefinitionRecordRepository,
        IStaticWebhookDefinitionStore staticWebhookDefinitionStore,
        IDynamicWebhookDefinitionStore dynamicWebhookDefinitionStore)
    {
        _localizableStringSerializer = localizableStringSerializer;
        _webhookDefinitionManager = webhookDefinitionManager;
        _webhookGroupDefinitionRecordRepository = webhookGroupDefinitionRecordRepository;
        _staticWebhookDefinitionStore = staticWebhookDefinitionStore;
        _dynamicWebhookDefinitionStore = dynamicWebhookDefinitionStore;
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Create)]
    public async virtual Task<WebhookGroupDefinitionDto> CreateAsync(WebhookGroupDefinitionCreateDto input)
    {
        if (await _webhookDefinitionManager.GetGroupOrNullAsync(input.Name) != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.AlreayNameExists)
                .WithData(nameof(WebhookGroupDefinitionRecord.Name), input.Name);
        }

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

        await _webhookGroupDefinitionRecordRepository.InsertAsync(webhookGroupDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(webhookGroupDefinitionRecord);
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Delete)]
    public async virtual Task DeleteAysnc(string name)
    {
        var webhookGroupDefinitionRecord = await _webhookGroupDefinitionRecordRepository.FindByNameAsync(name);
        if (webhookGroupDefinitionRecord != null)
        {
            await _webhookGroupDefinitionRecordRepository.DeleteAsync(webhookGroupDefinitionRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task<WebhookGroupDefinitionDto> GetAsync(string name)
    {
        var webhookGroupDefinition = await _staticWebhookDefinitionStore.GetGroupOrNullAsync(name);
        if (webhookGroupDefinition != null)
        {
            return DefinitionToDto(webhookGroupDefinition, true);
        }
        webhookGroupDefinition = await _dynamicWebhookDefinitionStore.GetGroupOrNullAsync(name);
        return DefinitionToDto(webhookGroupDefinition);
    }

    public async virtual Task<ListResultDto<WebhookGroupDefinitionDto>> GetListAsync(WebhookGroupDefinitionGetListInput input)
    {
        var groupDtoList = new List<WebhookGroupDefinitionDto>();
        var staticGroups = await _staticWebhookDefinitionStore.GetGroupsAsync();
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();
        groupDtoList.AddRange(staticGroups.Select(d => DefinitionToDto(d, true)));

        var dynamicGroups = await _dynamicWebhookDefinitionStore.GetGroupsAsync();
        groupDtoList.AddRange(dynamicGroups
            .Where(d => !staticGroupNames.Contains(d.Name))
            .Select(d => DefinitionToDto(d)));

        return new ListResultDto<WebhookGroupDefinitionDto>(groupDtoList
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter))
            .ToList());
    }

    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Update)]
    public async virtual Task<WebhookGroupDefinitionDto> UpdateAsync(string name, WebhookGroupDefinitionUpdateDto input)
    {
        if (await _staticWebhookDefinitionStore.GetOrNullAsync(name) != null)
        {
            throw new BusinessException(WebhooksManagementErrorCodes.WebhookGroupDefinition.StaticGroupNotAllowedChanged)
              .WithData("Name", name);
        }

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

        return DefinitionRecordToDto(webhookGroupDefinitionRecord);
    }

    protected virtual WebhookGroupDefinitionDto DefinitionRecordToDto(WebhookGroupDefinitionRecord webhookGroupDefinitionRecord)
    {
        var webhookGroupDto = new WebhookGroupDefinitionDto
        {
            Name = webhookGroupDefinitionRecord.Name,
            DisplayName = webhookGroupDefinitionRecord.DisplayName,
            IsStatic = false,
        };

        foreach (var property in webhookGroupDefinitionRecord.ExtraProperties)
        {
            webhookGroupDto.SetProperty(property.Key, property.Value);
        }

        return webhookGroupDto;
    }

    protected virtual WebhookGroupDefinitionDto DefinitionToDto(WebhookGroupDefinition webhookGroupDefinition, bool isStatic = false)
    {
        var webhookGroupDto = new WebhookGroupDefinitionDto
        {
            Name = webhookGroupDefinition.Name,
            IsStatic = isStatic,
            DisplayName = _localizableStringSerializer.Serialize(webhookGroupDefinition.DisplayName),
        };

        foreach (var property in webhookGroupDefinition.Properties)
        {
            webhookGroupDto.SetProperty(property.Key, property.Value);
        }

        return webhookGroupDto;
    }
}
