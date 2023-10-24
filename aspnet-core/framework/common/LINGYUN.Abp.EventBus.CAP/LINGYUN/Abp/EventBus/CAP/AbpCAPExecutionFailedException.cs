using DotNetCore.CAP.Messages;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// AbpECAPExecutionFailedException
    /// </summary>
    public class AbpCAPExecutionFailedException : AbpException
    {
        /// <summary>
        /// MessageType
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public Message Origin { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="origin"></param>
        public AbpCAPExecutionFailedException(MessageType messageType, Message origin)
        {
            MessageType = messageType;
            Origin = origin;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="origin"></param>
        /// <param name="message"></param>
        public AbpCAPExecutionFailedException(MessageType messageType, Message origin, string message) : base(message)
        {
            MessageType = messageType;
            Origin = origin;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="origin"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AbpCAPExecutionFailedException(MessageType messageType, Message origin, string message, Exception innerException) : base(message, innerException)
        {
            MessageType = messageType;
            Origin = origin;
        }
    }
}
