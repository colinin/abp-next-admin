namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    internal static class PersistenceIndexConsts
    {
        public const string WorkflowInstanceIndex = "instances";
        public const string EventIndex = "events";
        public const string EventSubscriptionIndex = "subscriptions";
        public const string ExecutionErrorIndex = "executionerrors";
        public const string ScheduledCommandIndex = "scheduledcommands";
    }
}
