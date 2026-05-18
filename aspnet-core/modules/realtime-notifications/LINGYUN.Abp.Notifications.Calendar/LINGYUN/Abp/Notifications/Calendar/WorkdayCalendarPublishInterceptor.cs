using LINGYUN.Abp.Calendar;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Notifications.Calendar;

/// <summary>
/// 工作日通知发布拦截器
/// </summary>
public class WorkdayCalendarPublishInterceptor : INotificationPublishInterceptor, ITransientDependency
{
    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly IWorkdayCalendarService _workdayCalendarService;
    public ILogger<WorkdayCalendarPublishInterceptor> Logger { protected get; set; }

    public WorkdayCalendarPublishInterceptor(
        IClock clock,
        ICurrentTenant currentTenant,
        IWorkdayCalendarService workdayCalendarService)
    {
        _clock = clock;
        _currentTenant = currentTenant;
        _workdayCalendarService = workdayCalendarService;

        Logger = NullLogger<WorkdayCalendarPublishInterceptor>.Instance;
    }

    public int Priority => 100;

    public async Task<bool> CanPublishAsync(NotificationInfo notification)
    {
        try
        {
            using (_currentTenant.Change(notification.TenantId))
            {
                var isWorktime = await _workdayCalendarService.IsInWorkTimeAsync(_clock.Now);

                if (!isWorktime)
                {
                    Logger.LogInformation(
                        "Notification {NotificationName} was blocked during the working time check. Type: {NotificationType}, Time: {Time}.",
                        notification.Name,
                        notification.Type,
                        _clock.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    return false;
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex,
                "If an abnormality occurs during the working day check, the default setting allows {NotificationName} to be notified to publish.",
                notification.Name);

            return true;
        }
    }
}
