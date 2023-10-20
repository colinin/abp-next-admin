using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.MiniProgram.Messages
{
    /// <summary>
    /// 小程序模板消息
    /// 详情: https://developers.weixin.qq.com/miniprogram/dev/api-backend/open-api/subscribe-message/subscribeMessage.send.html
    /// </summary>
    /// <remarks>
    /// 暂时仅实现发送订阅消息
    /// </remarks>
    public interface ISubscribeMessager
    {
        /// <summary>
        /// 发送订阅消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendAsync(SubscribeMessage message, CancellationToken cancellation = default);
        /// <summary>
        /// 发送订阅消息
        /// </summary>
        /// <param name="toUser">用户</param>
        /// <param name="templateId">模板</param>
        /// <param name="page">跳转页面</param>
        /// <param name="lang">语言</param>
        /// <param name="state">类型</param>
        /// <param name="data">数据</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendAsync(
            Guid toUser, 
            string templateId,
            string page = "",
            string lang = "zh_CN",
            string state = "formal",
            Dictionary<string, object> data = null,
            CancellationToken cancellation = default);
    }
}
