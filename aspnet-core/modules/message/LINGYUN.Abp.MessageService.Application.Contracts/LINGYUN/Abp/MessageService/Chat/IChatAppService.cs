using LINGYUN.Abp.IM.Group;
using LINGYUN.Abp.IM.Messages;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IChatAppService : IApplicationService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage chatMessage);
        /// <summary>
        /// 申请加入群组
        /// </summary>
        /// <param name="userJoinGroup"></param>
        /// <returns></returns>
        Task ApplyJoinGroupAsync(UserJoinGroupDto userJoinGroup);
        /// <summary>
        /// 获取我的群组
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<Group>> GetMyGroupsAsync();
        /// <summary>
        /// 获取群组用户
        /// </summary>
        /// <param name="groupUserGetByPaged"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserGroup>> GetGroupUsersAsync(GroupUserGetByPagedDto groupUserGetByPaged);
        /// <summary>
        /// 处理用户群组申请
        /// </summary>
        /// <param name="groupAcceptUser"></param>
        /// <returns></returns>
        Task GroupAcceptUserAsync(GroupAcceptUserDto groupAcceptUser);
        /// <summary>
        /// 群组移除用户
        /// </summary>
        /// <param name="groupRemoveUser"></param>
        /// <returns></returns>
        Task GroupRemoveUserAsync(GroupRemoveUserDto groupRemoveUser);
        /// <summary>
        /// 获取群组消息
        /// </summary>
        /// <param name="groupMessageGetByPaged"></param>
        /// <returns></returns>
        Task<PagedResultDto<ChatMessage>> GetGroupMessageAsync(GroupMessageGetByPagedDto groupMessageGetByPaged);
        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <param name="userMessageGetByPaged"></param>
        /// <returns></returns>
        Task<PagedResultDto<ChatMessage>> GetMyChatMessageAsync(UserMessageGetByPagedDto userMessageGetByPaged);

        //TOTO: 还应该有获取我的未读消息 获取我的未读群组消息
    }
}
