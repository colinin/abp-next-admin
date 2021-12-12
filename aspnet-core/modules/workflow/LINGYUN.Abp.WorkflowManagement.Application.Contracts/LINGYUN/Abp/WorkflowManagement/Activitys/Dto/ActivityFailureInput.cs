using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    public class ActivityFailureInput
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public object Result { get; set; }
    }
}
