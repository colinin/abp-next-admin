using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Webhooks.BackgroundWorker
{
    public class WebhookSenderJob : AsyncBackgroundJob<WebhookSenderArgs>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IWebhookDefinitionManager _webhookDefinitionManager;
        private readonly IWebhookSubscriptionManager _webhookSubscriptionManager;
        private readonly IWebhookSendAttemptStore _webhookSendAttemptStore;
        private readonly IWebhookSender _webhookSender;

        private readonly AbpWebhooksOptions _options;

        public WebhookSenderJob(
            IUnitOfWorkManager unitOfWorkManager,
            IWebhookDefinitionManager webhookDefinitionManager,
            IWebhookSubscriptionManager webhookSubscriptionManager,
            IWebhookSendAttemptStore webhookSendAttemptStore,
            IWebhookSender webhookSender,
            IOptions<AbpWebhooksOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _webhookDefinitionManager = webhookDefinitionManager;
            _webhookSubscriptionManager = webhookSubscriptionManager;
            _webhookSendAttemptStore = webhookSendAttemptStore;
            _webhookSender = webhookSender;
            _options = options.Value;
        }

        public override async Task ExecuteAsync(WebhookSenderArgs args)
        {
            var webhookDefinition = _webhookDefinitionManager.Get(args.WebhookName);

            if (webhookDefinition.TryOnce)
            {
                try
                {
                    await SendWebhook(args, webhookDefinition);
                }
                catch (Exception e)
                {
                    Logger.LogWarning("An error occured while sending webhook with try once.", e);
                    // ignored
                }
            }
            else
            {
                await SendWebhook(args, webhookDefinition);
            }
        }

        private async Task SendWebhook(WebhookSenderArgs args, WebhookDefinition webhookDefinition)
        {
            if (args.WebhookEventId == default)
            {
                return;
            }

            if (args.WebhookSubscriptionId == default)
            {
                return;
            }

            if (!webhookDefinition.TryOnce)
            {
                var sendAttemptCount = await _webhookSendAttemptStore.GetSendAttemptCountAsync(
                    args.TenantId,
                    args.WebhookEventId,
                    args.WebhookSubscriptionId
                );

                if ((webhookDefinition.MaxSendAttemptCount > 0 && sendAttemptCount > webhookDefinition.MaxSendAttemptCount) ||
                    sendAttemptCount > _options.MaxSendAttemptCount)
                {
                    return;
                }
            }

            try
            {
                await _webhookSender.SendWebhookAsync(args);
            }
            catch (Exception)
            {
                // no need to retry to send webhook since subscription disabled
                if (!await TryDeactivateSubscriptionIfReachedMaxConsecutiveFailCount(
                        args.TenantId,
                        args.WebhookSubscriptionId))
                {
                    throw; //Throw exception to re-try sending webhook
                }
            }
        }

        private async Task<bool> TryDeactivateSubscriptionIfReachedMaxConsecutiveFailCount(
            Guid? tenantId,
            Guid subscriptionId)
        {
            if (!_options.IsAutomaticSubscriptionDeactivationEnabled)
            {
                return false;
            }

            var hasXConsecutiveFail = await _webhookSendAttemptStore
                .HasXConsecutiveFailAsync(
                    tenantId,
                    subscriptionId,
                    _options.MaxConsecutiveFailCountBeforeDeactivateSubscription
                );

            if (!hasXConsecutiveFail)
            {
                return false;
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                await _webhookSubscriptionManager.ActivateWebhookSubscriptionAsync(subscriptionId, false);
                await uow.CompleteAsync();
                return true;
            }
        }
    }
}
