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
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ChatMessageSendResultDto> SendMessageAsync(ChatMessage input);
        ///// <summary>
        ///// 申请加入群组
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task ApplyJoinGroupAsync(UserJoinGroupDto input);
        ///// <summary>
        ///// 获取我的群组
        ///// </summary>
        ///// <returns></returns>
        //Task<ListResultDto<Group>> GetMyGroupsAsync();
        ///// <summary>
        ///// 获取群组用户
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto input);
        ///// <summary>
        ///// 处理用户群组申请
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task GroupAcceptUserAsync(GroupAcceptUserDto input);
        ///// <summary>
        ///// 群组移除用户
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task GroupRemoveUserAsync(GroupRemoveUserDto input);
        /// <summary>
        /// 获取群组消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ChatMessage>> GetMyGroupMessageAsync(GroupMessageGetByPagedDto input);
        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ChatMessage>> GetMyChatMessageAsync(UserMessageGetByPagedDto input);
        /// <summary>
        /// 获取我最近的消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ListResultDto<LastChatMessage>> GetMyLastChatMessageAsync(GetUserLastMessageDto input);
        //TOTO: 还应该有获取我的未读消息 获取我的未读群组消息
    }
}
