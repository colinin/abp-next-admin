using LINGYUN.Abp.WeChat.Features;

namespace LINGYUN.Abp.WeChat.MiniProgram.Features
{
    public static class WeChatMiniProgramFeatures
    {
        public const string GroupName = WeChatFeatures.GroupName + ".MiniProgram";

        public const string Enable = GroupName + ".Enable";

        public const string EnableAuthorization = GroupName + ".EnableAuthorization";

        public static class Messages
        {
            public const string Default = GroupName + ".Messages";

            public const string Enable = Default + ".Enable";
            /// <summary>
            /// 发送次数上限
            /// </summary>
            public const string SendLimit = Default + ".SendLimit";
            /// <summary>
            /// 发送次数上限时长
            /// </summary>
            public const string SendLimitInterval = Default + ".SendLimitInterval";
            /// <summary>
            /// 默认发布次数上限
            /// </summary>
            public const int DefaultSendLimit = 1000;
            /// <summary>
            /// 默认发布次数上限时长
            /// </summary>
            public const int DefaultSendLimitInterval = 1;
        }
    }
}
