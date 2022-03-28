using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Extensions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement;

[Authorize(WebhooksManagementPermissions.WebhookSubscription.Default)]
public class WebhooksSubscriptionAppService : WebhooksManagementAppServiceBase, IWebhooksSubscriptionAppService
{
    protected IWebhookDefinitionManager WebhookDefinitionManager { get; }
    protected IWebhookSubscriptionRepository SubscriptionRepository { get; }

    public WebhooksSubscriptionAppService(
        IWebhookDefinitionManager webhookDefinitionManager,
        IWebhookSubscriptionRepository subscriptionRepository)
    {
        WebhookDefinitionManager = webhookDefinitionManager;
        SubscriptionRepository = subscriptionRepository;
    }

    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Create)]
    public async virtual Task<WebhookSubscriptionDto> CreateAsync(WebhookSubscriptionCreateInput input)
    {
        await CheckSubscribedAsync(input);

        var subscription = new WebhookSubscription(
            GuidGenerator.Create(),
            input.WebhookUri,
            input.Secret,
            JsonConvert.SerializeObject(input.Webhooks),
            JsonConvert.SerializeObject(input.Headers),
            CurrentTenant.Id);

        await SubscriptionRepository.InsertAsync(subscription);

        await CurrentUnitOfWork.SaveChangesAsync();

        return subscription.ToWebhookSubscriptionDto();
    }

    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return SubscriptionRepository.DeleteAsync(id);
    }

    public async virtual Task<WebhookSubscriptionDto> GetAsync(Guid id)
    {
        var subscription = await SubscriptionRepository.GetAsync(id);

        return subscription.ToWebhookSubscriptionDto();
    }

    public async virtual Task<PagedResultDto<WebhookSubscriptionDto>> GetListAsync(WebhookSubscriptionGetListInput input)
    {
        var filter = new WebhookSubscriptionFilter
        {
            Filter = input.Filter,
            BeginCreationTime = input.BeginCreationTime,
            EndCreationTime = input.EndCreationTime,
            IsActive = input.IsActive,
            Secret = input.Secret,
            TenantId = input.TenantId,
            Webhooks = input.Webhooks,
            WebhookUri = input.WebhookUri
        };

        var totalCount = await SubscriptionRepository.GetCountAsync(filter);
        var subscriptions = await SubscriptionRepository.GetListAsync(filter,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<WebhookSubscriptionDto>(totalCount,
            subscriptions.Select(subscription => subscription.ToWebhookSubscriptionDto()).ToList());
    }

    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Update)]
    public async virtual Task<WebhookSubscriptionDto> UpdateAsync(Guid id, WebhookSubscriptionUpdateInput input)
    {
        var subscription = await SubscriptionRepository.GetAsync(id);
        if (!string.Equals(subscription.WebhookUri, input.WebhookUri))
        {
            await CheckSubscribedAsync(input);
        }

        subscription.SetSecret(input.Secret);
        subscription.SetWebhookUri(input.WebhookUri);
        subscription.SetWebhooks(input.ToSubscribedWebhooksString());
        subscription.SetHeaders(input.ToWebhookHeadersString());
        subscription.IsActive = input.IsActive;

        await SubscriptionRepository.UpdateAsync(subscription);

        await CurrentUnitOfWork.SaveChangesAsync();

        return subscription.ToWebhookSubscriptionDto();
    }

    public async virtual Task<ListResultDto<WebhooksAvailableDto>> GetAllAvailableWebhooksAsync()
    {
        var webhooks = WebhookDefinitionManager.GetAll();
        var definitions = new List<WebhooksAvailableDto>();

        foreach (var webhookDefinition in webhooks)
        {
            if (await WebhookDefinitionManager.IsAvailableAsync(CurrentTenant.Id, webhookDefinition.Name))
            {
                definitions.Add(new WebhooksAvailableDto
                {
                    Name = webhookDefinition.Name,
                    Description = webhookDefinition.Description?.Localize(StringLocalizerFactory),
                    DisplayName = webhookDefinition.DisplayName?.Localize(StringLocalizerFactory)
                });
            }
        }

        return new ListResultDto<WebhooksAvailableDto>(definitions.OrderBy(d => d.Name).ToList());
    }

    protected async virtual Task CheckSubscribedAsync(WebhookSubscriptionCreateOrUpdateInput input)
    {
        foreach (var webhookName in input.Webhooks)
        {
            if (await SubscriptionRepository.IsSubscribedAsync(CurrentTenant.Id, input.WebhookUri, webhookName))
            {
                throw new BusinessException(WebhooksManagementErrorCodes.WebhookSubscription.DuplicateSubscribed)
                    .WithData(nameof(WebhookSubscription.WebhookUri), input.WebhookUri)
                    .WithData(nameof(WebhookSubscription.Webhooks), webhookName);
            }
        }
    }
}
