using LINGYUN.Abp.BackgroundTasks;

namespace LINGYUN.Abp.TaskManagement;
public static class BackgroundJobInfoEbackgroundJobInfotensions
{
    public static JobInfo ToJobInfo(this BackgroundJobInfo backgroundJobInfo)
    {
        return new JobInfo
        {
            Id = backgroundJobInfo.Id,
            TenantId = backgroundJobInfo.TenantId,
            Name = backgroundJobInfo.Name,
            NextRunTime = backgroundJobInfo.NextRunTime,
            Args = backgroundJobInfo.Args,
            IsAbandoned = backgroundJobInfo.IsAbandoned,
            BeginTime = backgroundJobInfo.BeginTime,
            EndTime = backgroundJobInfo.EndTime,
            CreationTime = backgroundJobInfo.CreationTime,
            Cron = backgroundJobInfo.Cron,
            MaxCount = backgroundJobInfo.MaxCount,
            MaxTryCount = backgroundJobInfo.MaxTryCount,
            Description = backgroundJobInfo.Description,
            Group = backgroundJobInfo.Group,
            Interval = backgroundJobInfo.Interval,
            JobType = backgroundJobInfo.JobType,
            Status = backgroundJobInfo.Status,
            Priority = backgroundJobInfo.Priority,
            Source = backgroundJobInfo.Source,
            LastRunTime = backgroundJobInfo.LastRunTime,
            LockTimeOut = backgroundJobInfo.LockTimeOut,
            Result = backgroundJobInfo.Result,
            TriggerCount = backgroundJobInfo.TriggerCount,
            TryCount = backgroundJobInfo.TryCount,
            Type = backgroundJobInfo.Type,
            NodeName = backgroundJobInfo.NodeName,
        };
    }
}
