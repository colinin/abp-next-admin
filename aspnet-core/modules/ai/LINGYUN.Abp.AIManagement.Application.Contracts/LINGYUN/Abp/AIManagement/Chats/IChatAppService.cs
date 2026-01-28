using LINGYUN.Abp.AIManagement.Chats.Dtos;
using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AIManagement.Chats;
public interface IChatAppService : IApplicationService
{
    IAsyncEnumerable<string> SendMessageAsync(SendTextChatMessageDto input);
}
