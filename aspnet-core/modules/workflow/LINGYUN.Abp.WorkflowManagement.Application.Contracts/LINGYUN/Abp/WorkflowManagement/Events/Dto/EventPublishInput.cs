using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.WorkflowManagement.Events
{
    public class EventPublishInput
    {
        [Required]
        public string EventName { get; set; }

        [Required]
        public string EventKey { get; set; }

        [Required]
        public object EventData { get; set; }

        public DateTime? EffectiveDate { get; set; }
    }
}
