using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ExceptionHandling.Emailing
{
    public class AbpEmailExceptionHandlingOptions
    {
        /// <summary>
        /// 发送堆栈信息
        /// </summary>
        public bool SendStackTrace { get; set; } = false;
        /// <summary>
        /// 默认邮件标题
        /// </summary>
        public string DefaultTitle { get; set; }
        /// <summary>
        /// 默认邮件内容头
        /// </summary>
        public string DefaultContentHeader { get; set; }
        /// <summary>
        /// 默认邮件内容底
        /// </summary>
        public string DefaultContentFooter { get; set; }
        /// <summary>
        /// 默认异常收件人
        /// </summary>
        public string DefaultReceiveEmail { get; set; }
        /// <summary>
        /// 异常类型指定收件人处理映射列表
        /// </summary>
        public IDictionary<Type, string> Handlers { get; set; }
        public AbpEmailExceptionHandlingOptions()
        {
            Handlers = new Dictionary<Type, string>();
        }

        /// <summary>
        /// 把需要接受异常通知的用户加进处理列表
        /// </summary>
        /// <typeparam name="TException">处理的异常类型</typeparam>
        /// <param name="receivedEmails">接收邮件的用户类别,群发用,符号分隔</param>
        public void HandReceivedException<TException>(string receivedEmails) where TException : Exception
        {
            HandReceivedException(typeof(TException), receivedEmails);
        }
        /// <summary>
        /// 把需要接受异常通知的用户加进处理列表
        /// </summary>
        /// <param name="ex">处理的异常类型</param>
        /// <param name="receivedEmails">接收邮件的用户类别,群发用,符号分隔</param>
        public void HandReceivedException(Type exceptionType, string receivedEmails)
        {
            if (Handlers.ContainsKey(exceptionType))
            {
                Handlers[exceptionType] += receivedEmails;
            }
            else
            {
                Handlers.Add(exceptionType, receivedEmails);
            }
        }

        public string GetReceivedEmailOrDefault(Type exceptionType)
        {
            if (Handlers.TryGetValue(exceptionType, out string receivedUsers))
            {
                return receivedUsers;
            }
            return DefaultReceiveEmail;
        }
    }
}
