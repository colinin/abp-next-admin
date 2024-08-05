using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Chat;

public class UserGroupGetByGroupIdDto
{
    [Required]
    public long GroupId { get; set; }
}
