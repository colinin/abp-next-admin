using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.IM.Messages
{
    [Serializable]
    [EventName("im.message")]
    public class ChatMessage : ExtensibleObject
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

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = ExtensibleObjectValidator.GetValidationErrors(this, validationContext);

            foreach (var result in ValidateReceiver(validationContext))
            {
                results.Add(result);
            }

            return results;
        }

        protected virtual IEnumerable<ValidationResult> ValidateReceiver(ValidationContext validationContext)
        {
            if (GroupId.IsNullOrWhiteSpace() && !ToUserId.HasValue)
            {
                yield return new ValidationResult("");
            }
        }
    }
}
