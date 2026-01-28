using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public class ConversationCreateDto
{
    [DynamicStringLength(typeof(ConversationRecordConsts), nameof(ConversationRecordConsts.MaxNameLength))]
    public string? Name { get; set; }
}
