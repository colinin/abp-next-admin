using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public class ConversationUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(ConversationRecordConsts), nameof(ConversationRecordConsts.MaxNameLength))]
    public string Name { get; set; }
}
