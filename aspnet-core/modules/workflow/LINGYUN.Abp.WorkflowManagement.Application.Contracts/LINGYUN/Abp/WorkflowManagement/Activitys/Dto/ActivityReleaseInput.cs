using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    public class ActivityReleaseInput
    {
        [Required]
        public string Token { get; set; }
    }
}
