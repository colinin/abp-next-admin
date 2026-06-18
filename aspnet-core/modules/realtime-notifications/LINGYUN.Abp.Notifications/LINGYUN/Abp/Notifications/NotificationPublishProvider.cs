using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications;

public abstract class NotificationPublishProvider : INotificationPublishProvider, ITransientDependency
{
    public abstract string Name { get; }

    public IAbpLazyServiceProvider ServiceProvider { protected get; set; }

    public ILoggerFactory LoggerFactory => ServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ILogger Logger => _lazyLogger.Value;
    private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

    public ICancellationTokenProvider CancellationTokenProvider => ServiceProvider.LazyGetService<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);

    private IEnumerable<INotificationPublishInterceptor> _interceptors;
    protected IEnumerable<INotificationPublishInterceptor> Interceptors =>
        _interceptors ??= ServiceProvider.LazyGetService<IEnumerable<INotificationPublishInterceptor>>()
                          ?? Enumerable.Empty<INotificationPublishInterceptor>();

    public async virtual Task<bool> CanPublishAsync(NotificationInfo notification)
    {
        return await CanPublishAsync(notification, GetCancellationToken());
    }

    public async Task PublishAsync(NotificationPublishContext context)
    {
        var cancellationToken = GetCancellationToken();

        // 通知拦截器检查
        if (!await ExecuteInterceptorsAsync(context, cancellationToken))
        {
            return;
        }

        await PublishAsync(context, cancellationToken);
    }

    /// <summary>
    /// 是否允许发布通知
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task<bool> CanPublishAsync(
        NotificationInfo notification,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// 执行通知发布拦截器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<bool> ExecuteInterceptorsAsync(
        NotificationPublishContext context,
        CancellationToken cancellationToken = default)
    {
        if (context.Notification.Type == NotificationType.System ||
            context.Notification.Type == NotificationType.ServiceCallback)
        {
            Logger.LogDebug(
                "System or service callback notification {NotificationName}, skip all interceptors.",
                context.Notification.Name);
            return true;
        }

        if (!Interceptors.Any())
        {
            Logger.LogDebug("No notification publishing interceptors have been registered.");
            return true;
        }

        var orderedInterceptors = Interceptors.OrderBy(i => i.Priority);

        foreach (var interceptor in orderedInterceptors)
        {
            try
            {
                if (!await interceptor.CanPublishAsync(context.Notification))
                {
                    var reason = string.Format("The interceptor {0} prevented the publication of the notification {1}.",
                        interceptor.GetType().Name,
                        context.Notification.Name);

                    context.Cancel(reason);

                    Logger.LogWarning(reason);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex,
                    "An exception occurred while executing the interceptor {InterceptorType}. By default, publication is allowed.",
                    interceptor.GetType().Name);
            }
        }

        return true;
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken cancellationToken = default)
    {
        return CancellationTokenProvider.FallbackToProvider(cancellationToken);
    }
    /// <summary>
    /// 重写实现通知发布
    /// </summary>
    /// <param name="context">通知发送上下文</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task PublishAsync(NotificationPublishContext context, CancellationToken cancellationToken = default);
}
