using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    public class GetPendingActivityInput
    {
        [Required]
        public string ActivityName { get; set; }

        [Required]
        public string WorkflowId { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}
