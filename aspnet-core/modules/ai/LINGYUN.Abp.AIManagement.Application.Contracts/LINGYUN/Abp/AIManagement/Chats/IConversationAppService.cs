using LINGYUN.Abp.AIManagement.Chats.Dtos;
using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AIManagement.Chats;
public interface IConversationAppService : 
    ICrudAppService<
        ConversationDto,
        Guid,
        ConversationGetListInput,
        ConversationCreateDto,
        ConversationUpdateDto>
{
}
