using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundTasks.Jobs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

[Dependency(ReplaceServices = true)]
public class JobExceptionNotifier : IJobExceptionNotifier, ITransientDependency
{
    public const string Prefix = "exception.";

    protected IClock Clock { get; }
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }

    public JobExceptionNotifier(
        IClock clock,
        IJobStore jobStore,
        IJobScheduler jobScheduler)
    {
        Clock = clock;
        JobStore = jobStore;
        JobScheduler = jobScheduler;
    }

    public virtual async Task NotifyAsync([NotNull] JobExceptionNotificationContext context)
    {
        var notifyKey = Prefix + SendEmailJob.PropertyTo;
        if (context.JobInfo.Args.TryGetValue(notifyKey, out var exceptionTo))
        {
            var template = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyTemplate) ?? "";
            var subject = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertySubject) ?? "From job execute exception";
            var globalContext = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyContext) ?? "{}";
            var from = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyFrom) ?? "";
            var culture = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyCulture) ?? CultureInfo.CurrentCulture.Name;

            var jobId = Guid.NewGuid();
            var jobArgs = new Dictionary<string, object>
            {
                { SendEmailJob.PropertyTo, exceptionTo.ToString() },
                { SendEmailJob.PropertySubject, subject },
                { SendEmailJob.PropertyBody, context.Exception.GetBaseException().Message },
                { SendEmailJob.PropertyTemplate, template },
                { SendEmailJob.PropertyContext, globalContext },
                { SendEmailJob.PropertyFrom, from },
                { SendEmailJob.PropertyCulture, culture }
            };
            var jobInfo = new JobInfo
            {
                Id = jobId,
                Name = jobId.ToString(),
                Group = "ExceptionHandling",
                Priority = JobPriority.Normal,
                BeginTime = Clock.Now,
                Args = jobArgs,
                Description = subject.ToString(),
                JobType = JobType.Once,
                Interval = 5,
                MaxCount = 1,
                MaxTryCount = 1,
                CreationTime = Clock.Now,
                Status = JobStatus.None,
                Type = DefaultJobNames.SendEmailJob,
            };

            await JobStore.StoreAsync(jobInfo);

            await JobScheduler.TriggerAsync(jobInfo);
        }
    }
}
