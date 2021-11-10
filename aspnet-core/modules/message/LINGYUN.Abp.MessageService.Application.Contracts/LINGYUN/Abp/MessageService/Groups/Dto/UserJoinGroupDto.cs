using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Groups
{
    public class UserJoinGroupDto
    {
        [Required]
        public long GroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string JoinInfo { get; set; }
    }
}
