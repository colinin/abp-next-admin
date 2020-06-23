using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Messages
{
    public interface IMessageStore
    {
        /// <summary>
        /// 存储聊天记录
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <param name="formUserId"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        Task StoreMessageAsync(ChatMessage chatMessage);
        /// <summary>
        /// 获取群组聊天记录总数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<long> GetGroupMessageCountAsync(Guid? tenantId, long groupId, string filter = "", MessageType? type = null);
        /// <summary>
        /// 获取群组聊天记录
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupName"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<List<ChatMessage>> GetGroupMessageAsync(Guid? tenantId, long groupId, string filter = "", string sorting = nameof(ChatMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10);
        /// <summary>
        /// 获取与某个用户的聊天记录总数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sendUserId"></param>
        /// <param name="receiveUserId"></param>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<long> GetChatMessageCountAsync(Guid? tenantId, Guid sendUserId, Guid receiveUserId, string filter = "", MessageType? type = null);
        /// <summary>
        /// 获取与某个用户的聊天记录
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<List<ChatMessage>> GetChatMessageAsync(Guid? tenantId, Guid sendUserId, Guid receiveUserId, string filter = "", string sorting = nameof(ChatMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10);
    }
}
