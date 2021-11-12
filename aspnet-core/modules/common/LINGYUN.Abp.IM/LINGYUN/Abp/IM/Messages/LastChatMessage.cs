using System;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace LINGYUN.Abp.IM.Messages
{
    /// <summary>
    /// 上一次通讯消息
    /// </summary>
    public class LastChatMessage : IHasExtraProperties
    {
        public string AvatarUrl { get; set; }
        public string Object { get; set; }
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
        public string ToUserId { get; set; }
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
        public bool IsAnonymous => this.GetProperty(nameof(IsAnonymous), false);
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        public MessageSourceTye Source { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public LastChatMessage()
        {
            ExtraProperties = new ExtraPropertyDictionary();
        }
    }
}
