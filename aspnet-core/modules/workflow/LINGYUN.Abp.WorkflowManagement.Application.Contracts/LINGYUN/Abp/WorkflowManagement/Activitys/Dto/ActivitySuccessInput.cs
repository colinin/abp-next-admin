using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    public class ActivitySuccessInput
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public object Result { get; set; }
    }
}
