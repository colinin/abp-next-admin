using Hangfire;
using Hangfire.Dashboard;
using LINGYUN.Abp.Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundJobs.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpHangfireModule),
        typeof(AbpHangfireDashboardModule)
    )]
    public class AbpBackgroundJobsHangfireModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<DashboardOptions>(options =>
            {
                options.DisplayNameFunc = (dashboardContext, job) =>
                {
                    if (job.Args.Count == 0)
                    {
                        return job.Type.FullName;

                        //if (job.Type.GenericTypeArguments.Length == 0)
                        //{
                        //    return job.Type.FullName;
                        //}
                        //// TODO: 把特性作为任务名称?
                        //return BackgroundJobNameAttribute.GetName(job.Type.GenericTypeArguments[0]);
                    }
                    var context = dashboardContext.GetHttpContext();
                    var options = context.RequestServices.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
                    return options.GetJob(job.Args.First().GetType()).JobName;
                };
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var jobOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
            if (!jobOptions.IsJobExecutionEnabled)
            {
                var hangfireOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
                hangfireOptions.BackgroundJobServerFactory = CreateOnlyEnqueueJobServer;
            }
        }

        private BackgroundJobServer CreateOnlyEnqueueJobServer(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<JobStorage>();
            return null;
        }
    }
}