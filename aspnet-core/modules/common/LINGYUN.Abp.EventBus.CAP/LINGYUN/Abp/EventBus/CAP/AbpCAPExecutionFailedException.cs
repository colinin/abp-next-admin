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
        /// <param name="prigin"></param>
        public AbpCAPExecutionFailedException(MessageType messageType, Message prigin)
        {
            MessageType = messageType;
            Origin = prigin;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="prigin"></param>
        /// <param name="message"></param>
        public AbpCAPExecutionFailedException(MessageType messageType, Message prigin, string message) : base(message)
        {
            MessageType = messageType;
            Origin = prigin;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="prigin"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AbpCAPExecutionFailedException(MessageType messageType, Message prigin, string message, Exception innerException) : base(message, innerException)
        {
            MessageType = messageType;
            Origin = prigin;
        }
    }
}
