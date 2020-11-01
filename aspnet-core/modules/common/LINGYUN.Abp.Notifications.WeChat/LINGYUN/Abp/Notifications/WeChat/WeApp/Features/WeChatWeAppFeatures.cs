using LINGYUN.Abp.WeChat.Features;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp.Features
{
    public static class WeChatWeAppFeatures
    {
        public const string GroupName = WeChatFeatures.GroupName + ".WeApp";

        public static class Notifications
        {
            public const string Default = GroupName + ".Notifications";
            /// <summary>
            /// 发布次数上限
            /// </summary>
            public const string PublishLimit = Default + ".PublishLimit";
            /// <summary>
            /// 发布次数上限时长
            /// </summary>
            public const string PublishLimitInterval = Default + ".PublishLimitInterval";
            /// <summary>
            /// 默认发布次数上限
            /// </summary>
            public const int DefaultPublishLimit = 1000;
            /// <summary>
            /// 默认发布次数上限时长
            /// </summary>
            public const int DefaultPublishLimitInterval = 1;
        }
    }
}
