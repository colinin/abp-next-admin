using LINGYUN.Abp.AIManagement.Chats.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AIManagement.Chats;
public interface IChatAppService : IApplicationService
{
    IAsyncEnumerable<string> SendMessageAsync(SendTextChatMessageDto input);

    Task<PagedResultDto<TextChatMessageDto>> GetListAsync(TextChatMessageGetListInput input);
}
