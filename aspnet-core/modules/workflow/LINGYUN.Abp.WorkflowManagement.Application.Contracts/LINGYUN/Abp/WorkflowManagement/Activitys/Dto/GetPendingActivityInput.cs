using System;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    public class GetPendingActivityInput
    {
        public string ActivityName { get; set; }

        public string WorkflowId { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}
