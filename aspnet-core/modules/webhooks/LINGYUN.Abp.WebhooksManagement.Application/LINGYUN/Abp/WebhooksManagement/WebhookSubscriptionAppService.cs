using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Extensions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace LINGYUN.Abp.WebhooksManagement;

[Authorize(WebhooksManagementPermissions.WebhookSubscription.Default)]
public class WebhookSubscriptionAppService : WebhooksManagementAppServiceBase, IWebhookSubscriptionAppService
{
    protected IWebhookDefinitionManager WebhookDefinitionManager { get; }
    protected IWebhookSubscriptionRepository SubscriptionRepository { get; }

    public WebhookSubscriptionAppService(
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
            input.ToSubscribedWebhooksString(),
            input.ToWebhookHeadersString(),
            input.Secret,
            input.TenantId ?? CurrentTenant.Id)
        {
            IsActive = input.IsActive,
            Description = input.Description,
            TimeoutDuration = input.TimeoutDuration,
        };

        subscription = await SubscriptionRepository.InsertAsync(subscription);

        await CurrentUnitOfWork.SaveChangesAsync();

        return subscription.ToWebhookSubscriptionDto();
    }

    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return SubscriptionRepository.DeleteAsync(id);
    }

    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Delete)]
    public async virtual Task DeleteManyAsync(WebhookSubscriptionDeleteManyInput input)
    {
        var subscriptions = await SubscriptionRepository.GetListAsync(
            x => input.RecordIds.Contains(x.Id));

        await SubscriptionRepository.DeleteManyAsync(subscriptions);
    }

    public async virtual Task<WebhookSubscriptionDto> GetAsync(Guid id)
    {
        var subscription = await SubscriptionRepository.GetAsync(id);

        return subscription.ToWebhookSubscriptionDto();
    }

    public async virtual Task<PagedResultDto<WebhookSubscriptionDto>> GetListAsync(WebhookSubscriptionGetListInput input)
    {
        var specification = new WebhookSubscriptionGetListSpecification(input);

        var totalCount = await SubscriptionRepository.GetCountAsync(specification);
        var subscriptions = await SubscriptionRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<WebhookSubscriptionDto>(totalCount,
            subscriptions.Select(subscription => subscription.ToWebhookSubscriptionDto()).ToList());
    }

    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Update)]
    public async virtual Task<WebhookSubscriptionDto> UpdateAsync(Guid id, WebhookSubscriptionUpdateInput input)
    {
        var subscription = await SubscriptionRepository.GetAsync(id);

        UpdateByInput(subscription, input);

        var inputWebhooks = input.ToSubscribedWebhooksString();
        if (!string.Equals(subscription.Webhooks, inputWebhooks, StringComparison.InvariantCultureIgnoreCase))
        {
            subscription.SetWebhooks(inputWebhooks);
        }
        var inputHeaders = input.ToWebhookHeadersString();
        if (!string.Equals(subscription.Headers, inputHeaders, StringComparison.InvariantCultureIgnoreCase))
        {
            subscription.SetHeaders(inputHeaders);
        }

        subscription.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        subscription = await SubscriptionRepository.UpdateAsync(subscription);

        await CurrentUnitOfWork.SaveChangesAsync();

        return subscription.ToWebhookSubscriptionDto();
    }

    public async virtual Task<ListResultDto<WebhookAvailableGroupDto>> GetAllAvailableWebhooksAsync()
    {
        var groups = await WebhookDefinitionManager.GetGroupsAsync();
        var definitions = new List<WebhookAvailableGroupDto>();

        foreach (var groupDefinition in groups)
        {
            var group = new WebhookAvailableGroupDto
            {
                Name = groupDefinition.Name,
                DisplayName = groupDefinition.DisplayName?.Localize(StringLocalizerFactory),
            };

            foreach (var webhookDefinition in groupDefinition.Webhooks.OrderBy(d => d.Name))
            {
                if (await WebhookDefinitionManager.IsAvailableAsync(CurrentTenant.Id, webhookDefinition.Name))
                {
                    group.Webhooks.Add(new WebhookAvailableDto
                    {
                        Name = webhookDefinition.Name,
                        Description = webhookDefinition.Description?.Localize(StringLocalizerFactory),
                        DisplayName = webhookDefinition.DisplayName?.Localize(StringLocalizerFactory)
                    });
                }
            }

            definitions.Add(group);
        }

        return new ListResultDto<WebhookAvailableGroupDto>(definitions.OrderBy(d => d.Name).ToList());
    }

    protected async virtual Task CheckSubscribedAsync(WebhookSubscriptionCreateOrUpdateInput input)
    {
        foreach (var webhookName in input.Webhooks)
        {
            if (await SubscriptionRepository.IsSubscribedAsync(input.TenantId ?? CurrentTenant.Id, input.WebhookUri, webhookName))
            {
                throw new BusinessException(WebhooksManagementErrorCodes.WebhookSubscription.DuplicateSubscribed)
                    .WithData(nameof(WebhookSubscription.WebhookUri), input.WebhookUri)
                    .WithData(nameof(WebhookSubscription.Webhooks), webhookName);
            }
        }
    }

    protected virtual void UpdateByInput(WebhookSubscription subscription, WebhookSubscriptionCreateOrUpdateInput input)
    {
        if (!string.Equals(subscription.Secret, input.Secret, StringComparison.InvariantCultureIgnoreCase))
        {
            subscription.SetSecret(input.Secret);
        }
        if (!string.Equals(subscription.WebhookUri, input.WebhookUri, StringComparison.InvariantCultureIgnoreCase))
        {
            subscription.SetWebhookUri(input.WebhookUri);
        }
        if (!string.Equals(subscription.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
        {
            subscription.Description = input.Description;
        }
        if (!Equals(subscription.TenantId, input.TenantId))
        {
            subscription.SetTenantId(input.TenantId);
        }
        subscription.IsActive = input.IsActive;
        subscription.TimeoutDuration = input.TimeoutDuration;
    }

    private class WebhookSubscriptionGetListSpecification : Volo.Abp.Specifications.Specification<WebhookSubscription>
    {
        protected WebhookSubscriptionGetListInput Filter { get; }

        public WebhookSubscriptionGetListSpecification(WebhookSubscriptionGetListInput filter)
        {
            Filter = filter;
        }

        public override Expression<Func<WebhookSubscription, bool>> ToExpression()
        {
            Expression<Func<WebhookSubscription, bool>> expression = _ => true;

            return expression
                .AndIf(Filter.TenantId.HasValue, x => x.TenantId == Filter.TenantId)
                .AndIf(Filter.IsActive.HasValue, x => x.IsActive == Filter.IsActive)
                .AndIf(Filter.BeginCreationTime.HasValue, x => x.CreationTime >= Filter.BeginCreationTime)
                .AndIf(Filter.EndCreationTime.HasValue, x => x.CreationTime <= Filter.EndCreationTime)
                .AndIf(!Filter.WebhookUri.IsNullOrWhiteSpace(), x => x.WebhookUri == Filter.WebhookUri)
                .AndIf(!Filter.Secret.IsNullOrWhiteSpace(), x => x.Secret == Filter.Secret)
                .AndIf(!Filter.Webhooks.IsNullOrWhiteSpace(), x => x.Webhooks.Contains("\"" + Filter.Webhooks + "\""))
                .AndIf(!Filter.Filter.IsNullOrWhiteSpace(), x => x.WebhookUri.Contains(Filter.Filter) ||
                    x.Secret.Contains(Filter.Filter) || x.Webhooks.Contains(Filter.Filter));
        }
    }
}
