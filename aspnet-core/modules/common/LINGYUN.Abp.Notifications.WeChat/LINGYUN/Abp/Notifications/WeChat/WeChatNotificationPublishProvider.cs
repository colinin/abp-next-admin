using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.WeChat
{
    public class WeChatNotificationPublishProvider : NotificationPublishProvider
    {
        public override string Name => "WeChat";

        public WeChatNotificationPublishProvider(
            IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {

        }

        public override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers)
        {
            // step1 默认微信openid绑定的就是username,如果不是,那就根据userid去获取

            // step2 调用微信消息推送接口
        }
    }
}
