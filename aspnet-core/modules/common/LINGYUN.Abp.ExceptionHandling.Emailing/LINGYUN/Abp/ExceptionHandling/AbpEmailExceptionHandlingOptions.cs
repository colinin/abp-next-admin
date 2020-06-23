using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ExceptionHandling
{
    public class AbpEmailExceptionHandlingOptions
    {
        /// <summary>
        /// 默认异常收件人
        /// </summary>
        public string DefaultReceiveEmail { get; set; }
        /// <summary>
        /// 异常类型指定收件人处理映射列表
        /// </summary>
        public IDictionary<Exception, string> Handlers { get; set; }
        public AbpEmailExceptionHandlingOptions()
        {
            Handlers = new Dictionary<Exception, string>();
        }
        /// <summary>
        /// 把需要接受异常通知的用户加进处理列表
        /// </summary>
        /// <param name="ex">处理的异常类型</param>
        /// <param name="receivedEmails">接收邮件的用户类别,群发用,符号分隔</param>
        public void HandReceivedException(Exception ex, string receivedEmails)
        {
            if (Handlers.ContainsKey(ex))
            {
                Handlers[ex] += receivedEmails;
            }
            else
            {
                Handlers.Add(ex, receivedEmails);
            }
        }

        public string GetReceivedEmailOrDefault(Exception ex)
        {
            if (Handlers.TryGetValue(ex, out string receivedUsers))
            {
                return receivedUsers;
            }
            return DefaultReceiveEmail;
        }
    }
}
