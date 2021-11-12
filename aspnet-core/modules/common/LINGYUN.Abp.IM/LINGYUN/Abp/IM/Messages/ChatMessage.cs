using LINGYUN.Abp.RealTime.Localization;
using System;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.IM.Messages
{
    [Serializable]
    [EventName("im.message")]
    public class ChatMessage : IHasExtraProperties
    {
        /// <summary>
        /// 租户
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 群组标识
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// 消息标识
        /// </summary>
        /// <remarks>
        /// 调用者无需关注此字段,将由服务自动生成
        /// </remarks>
        public string MessageId { get; set; }
        /// <summary>
        /// 发送者标识
        /// </summary>
        public Guid FormUserId { get; set; }
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string FormUserName { get; set; }
        /// <summary>
        /// 接收用户标识
        /// </summary>
        /// <remarks>
        /// 设计为可空是为了兼容群聊消息
        /// /remarks>
        public Guid? ToUserId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [DisableAuditing]
        public string Content { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 是否匿名发送(存储在扩展字段)
        /// </summary>
        public bool IsAnonymous { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; } = MessageType.Text;

        public MessageSourceTye Source { get; set; } = MessageSourceTye.User;

        public ExtraPropertyDictionary ExtraProperties { get; set; }

        public ChatMessage()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public static ChatMessage User(
            Guid formUserId,
            string formUserName,
            Guid toUserId,
            string content,
            IClock clock,
            bool isAnonymous = false,
            MessageType type = MessageType.Text,
            MessageSourceTye souce = MessageSourceTye.User,
            Guid? tenantId = null)
        {
            return new ChatMessage
            {
                FormUserId = formUserId,
                FormUserName = formUserName,
                ToUserId = toUserId,
                Content = content,
                SendTime = clock.Now,
                IsAnonymous = isAnonymous,
                MessageType = type,
                TenantId = tenantId,
                Source = souce,
            };
        }
        public static ChatMessage System(
            Guid formUserId,
            Guid toUserId,
            string content,
            IClock clock,
            MessageType type = MessageType.Text,
            Guid? tenantId = null)
        {
            return new ChatMessage
            {
                FormUserId = formUserId,
                FormUserName = "system",
                ToUserId = toUserId,
                Content = content,
                SendTime = clock.Now,
                IsAnonymous = false,
                MessageType = type,
                TenantId = tenantId,
                Source = MessageSourceTye.System,
            }
            .SetProperty("L", false);
        }

        /// <summary>
        /// 本地化系统消息
        /// 用户消息与群组消息不能使用多语言
        /// </summary>
        /// <param name="formUserId"></param>
        /// <param name="toUserId"></param>
        /// <param name="content"></param>
        /// <param name="clock"></param>
        /// <param name="type"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static ChatMessage SystemLocalized(
            Guid formUserId,
            Guid toUserId,
            LocalizableStringInfo content,
            IClock clock,
            MessageType type = MessageType.Text,
            Guid? tenantId = null)
        {
            return new ChatMessage
            {
                FormUserId = formUserId,
                FormUserName = "system",
                ToUserId = toUserId,
                Content = "",
                SendTime = clock.Now,
                IsAnonymous = false,
                MessageType = type,
                TenantId = tenantId,
                Source = MessageSourceTye.System,
            }
            .SetProperty("L", true)
            .SetProperty(nameof(ChatMessage.Content).ToPascalCase(), content);
        }

        public static ChatMessage System(
            Guid formUserId,
            string groupId,
            string content,
            IClock clock,
            MessageType type = MessageType.Text,
            Guid? tenantId = null)
        {
            return new ChatMessage
            {
                FormUserId = formUserId,
                FormUserName = "system",
                GroupId = groupId,
                Content = content,
                SendTime = clock.Now,
                IsAnonymous = false,
                MessageType = type,
                TenantId = tenantId,
                Source = MessageSourceTye.System,
            }
            .SetProperty("L", false);
        }
        /// <summary>
        /// 本地化系统消息
        /// 用户消息与群组消息不能使用多语言
        /// </summary>
        /// <param name="formUserId"></param>
        /// <param name="groupId"></param>
        /// <param name="content"></param>
        /// <param name="clock"></param>
        /// <param name="type"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static ChatMessage SystemLocalized(
            Guid formUserId,
            string groupId,
            LocalizableStringInfo content,
            IClock clock,
            MessageType type = MessageType.Text,
            Guid? tenantId = null)
        {
            return new ChatMessage
            {
                FormUserId = formUserId,
                FormUserName = "system",
                GroupId = groupId,
                Content = "",
                SendTime = clock.Now,
                IsAnonymous = false,
                MessageType = type,
                TenantId = tenantId,
                Source = MessageSourceTye.System,
            }
            .SetProperty("L", true)
            .SetProperty(nameof(ChatMessage.Content).ToPascalCase(), content);
        }

        public static ChatMessage Group(
            Guid formUserId,
            string formUserName,
            string groupId,
            string content,
            IClock clock,
            bool isAnonymous = false,
            MessageType type = MessageType.Text,
            MessageSourceTye souce = MessageSourceTye.User,
            Guid? tenantId = null)
        {
            return new ChatMessage
            {
                FormUserId = formUserId,
                FormUserName = formUserName,
                GroupId = groupId,
                Content = content,
                SendTime = clock.Now,
                IsAnonymous = isAnonymous,
                MessageType = type,
                TenantId = tenantId,
                Source = souce,
            };
        }
    }
}
