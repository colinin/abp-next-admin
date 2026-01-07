using Microsoft.Extensions.Diagnostics.HealthChecks;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MicroService.TaskService;

public class ServiceHealthCheck : IHealthCheck
{
    private readonly IScheduler _scheduler;
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<IServiceProvider, Task<IScheduler>> _factory;

    public ServiceHealthCheck(IScheduler scheduler)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
    }

    public ServiceHealthCheck(IServiceProvider serviceProvider, Func<IServiceProvider, Task<IScheduler>> factory)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var scheduler = _scheduler ?? await _factory(_serviceProvider);

            if (scheduler.IsStarted)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
