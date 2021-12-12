using System;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    public class PendingActivityDto
    {
        public string Token { get; set; }

        public string ActivityName { get; set; }

        public object Parameters { get; set; }

        public DateTime TokenExpiry { get; set; }
    }
}
