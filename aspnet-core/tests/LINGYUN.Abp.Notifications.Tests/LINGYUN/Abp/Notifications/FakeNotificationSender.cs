using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;

[Dependency(ReplaceServices = true)]
public class FakeNotificationSender : INotificationSender, ITransientDependency
{
    public ILogger<FakeNotificationSender> Logger { get; set; }

    protected AbpNotificationsPublishOptions Options { get; }

    protected IJsonSerializer JsonSerializer { get; }

    protected ITemplateRenderer TemplateRenderer { get; }

    protected INotificationStore NotificationStore { get; }

    protected INotificationDataSerializer NotificationDataSerializer { get; }

    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }

    protected INotificationPublishProviderManager NotificationPublishProviderManager { get; }

    public FakeNotificationSender(
        IJsonSerializer jsonSerializer,
        ITemplateRenderer templateRenderer,
        IStringLocalizerFactory stringLocalizerFactory,
        IOptions<AbpNotificationsPublishOptions> options,
        INotificationStore notificationStore,
        INotificationDataSerializer notificationDataSerializer,
        INotificationDefinitionManager notificationDefinitionManager,
        INotificationSubscriptionManager notificationSubscriptionManager,
        INotificationPublishProviderManager notificationPublishProviderManager)
    {
        Options = options.Value;
        JsonSerializer = jsonSerializer;
        TemplateRenderer = templateRenderer;
        StringLocalizerFactory = stringLocalizerFactory;
        NotificationStore = notificationStore;
        NotificationDataSerializer = notificationDataSerializer;
        NotificationDefinitionManager = notificationDefinitionManager;
        NotificationSubscriptionManager = notificationSubscriptionManager;
        NotificationPublishProviderManager = notificationPublishProviderManager;

        Logger = NullLogger<FakeNotificationSender>.Instance;
    }

    public async virtual Task<string> SendNofiterAsync(
        string name,
        NotificationData data, 
        IEnumerable<UserIdentifier> users = null,
        Guid? tenantId = null, 
        NotificationSeverity severity = NotificationSeverity.Info,
        IEnumerable<string> useProviders = null)
    {
        var notification = await NotificationDefinitionManager.GetOrNullAsync(name);
        if (notification == null)
        {
            return "";
        }

        var notificationInfo = new NotificationInfo
        {
            Name = notification.Name,
            CreationTime = DateTime.Now,
            Data = data,
            Severity = severity,
            Lifetime = notification.NotificationLifetime,
            TenantId = tenantId,
            Type = notification.NotificationType
        };
        notificationInfo.SetId(DateTimeOffset.Now.ToUnixTimeMilliseconds());

        notificationInfo.Data = NotificationDataSerializer.Serialize(notificationInfo.Data);

        Logger.LogDebug($"Persistent notification {notificationInfo.Name}");

        await NotificationStore.InsertNotificationAsync(notificationInfo);

        var providers = Enumerable.Reverse(NotificationPublishProviderManager.Providers);

        if (useProviders?.Any() == true)
        {
            providers = providers.Where(p => useProviders.Contains(p.Name));
        }
        else if (notification.Providers.Any())
        {
            providers = providers.Where(p => notification.Providers.Contains(p.Name));
        }

        await PublishFromProvidersAsync(
            providers,
            users ?? new List<UserIdentifier>(), 
            notificationInfo);

        return notificationInfo.Id;
    }

    public async virtual Task<string> SendNofiterAsync(
        string name,
        NotificationTemplate template, 
        IEnumerable<UserIdentifier> users = null,
        Guid? tenantId = null, 
        NotificationSeverity severity = NotificationSeverity.Info,
        IEnumerable<string> useProviders = null)
    {
        var notification = await NotificationDefinitionManager.GetOrNullAsync(name);
        if (notification == null)
        {
            return "";
        }

        var notificationInfo = new NotificationInfo
        {
            Name = notification.Name,
            TenantId = tenantId,
            Severity = severity,
            Type = notification.NotificationType,
            CreationTime = DateTime.Now,
            Lifetime = notification.NotificationLifetime,
        };
        notificationInfo.SetId(DateTimeOffset.Now.ToUnixTimeMilliseconds());

        var title = notification.DisplayName.Localize(StringLocalizerFactory);

        var message = await TemplateRenderer.RenderAsync(
            templateName: name,
            model: template.ExtraProperties);

        var notificationData = new NotificationData();
        notificationData.WriteStandardData(
            title: title,
            message: message,
            createTime: notificationInfo.CreationTime,
            formUser: "Fake User");
        notificationData.ExtraProperties.AddIfNotContains(template.ExtraProperties);

        notificationInfo.Data = notificationData;

        Logger.LogDebug($"Persistent notification {notificationInfo.Name}");

        // 持久化通知
        await NotificationStore.InsertNotificationAsync(notificationInfo);

        var providers = Enumerable.Reverse(NotificationPublishProviderManager.Providers);

        // 过滤用户指定提供者
        if (useProviders?.Any() == true)
        {
            providers = providers.Where(p => useProviders.Contains(p.Name));
        }
        else if (notification.Providers.Any())
        {
            providers = providers.Where(p => notification.Providers.Contains(p.Name));
        }

        await PublishFromProvidersAsync(
            providers, 
            users ?? new List<UserIdentifier>(), 
            notificationInfo);

        return notificationInfo.Id;
    }

    /// <summary>
    /// 指定提供者发布通知
    /// </summary>
    /// <param name="providers">提供者列表</param>
    /// <param name="notificationInfo">通知信息</param>
    /// <returns></returns>
    protected async Task PublishFromProvidersAsync(
        IEnumerable<INotificationPublishProvider> providers,
        IEnumerable<UserIdentifier> users,
        NotificationInfo notificationInfo)
    {
        foreach (var provider in providers)
        {
            await PublishAsync(provider, notificationInfo, users);
        }
    }
    /// <summary>
    /// 发布通知
    /// </summary>
    /// <param name="provider">通知发布者</param>
    /// <param name="notificationInfo">通知信息</param>
    /// <param name="subscriptionUserIdentifiers">订阅用户列表</param>
    /// <returns></returns>
    protected async Task PublishAsync(
        INotificationPublishProvider provider,
        NotificationInfo notificationInfo,
        IEnumerable<UserIdentifier> subscriptionUserIdentifiers)
    {
        Logger.LogDebug($"Sending notification with provider {provider.Name}");
        // var notifacationDataMapping = Options.NotificationDataMappings
        //         .GetMapItemOrDefault(provider.Name, notificationInfo.Name);
        // if (notifacationDataMapping != null)
        // {
        //     notificationInfo.Data = notifacationDataMapping.MappingFunc(notificationInfo.Data);
        // }
        // 发布
        await provider.PublishAsync(notificationInfo, subscriptionUserIdentifiers);

        Logger.LogDebug($"Send notification {notificationInfo.Name} with provider {provider.Name} was successful");
    }
}
