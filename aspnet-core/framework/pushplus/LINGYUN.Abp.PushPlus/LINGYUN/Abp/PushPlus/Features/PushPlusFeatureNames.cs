namespace LINGYUN.Abp.PushPlus.Features;

public static class PushPlusFeatureNames
{
    public const string GroupName = "PushPlus";

    public static class Message
    {
        public const string GroupName = PushPlusFeatureNames.GroupName + ".Message";

        public const string Enable = GroupName + ".Enable";
    }

    public static class Channel
    {
        public const string GroupName = PushPlusFeatureNames.GroupName + ".Channel";

        public static class WeChat
        {
            public const string GroupName = Channel.GroupName + ".WeChat";
            /// <summary>
            /// 启用微信通道
            /// </summary>
            public const string Enable = GroupName + ".Enable";
            /// <summary>
            /// 发送次数上限
            /// </summary>
            public const string SendLimit = GroupName + ".SendLimit";
            /// <summary>
            /// 发送次数上限时长
            /// </summary>
            public const string SendLimitInterval = GroupName + ".SendLimitInterval";
        }

        public static class WeWork
        {
            public const string GroupName = Channel.GroupName + ".WeWork";
            /// <summary>
            /// 启用企业微信通道
            /// </summary>
            public const string Enable = GroupName + ".Enable";
            /// <summary>
            /// 发送次数上限
            /// </summary>
            public const string SendLimit = GroupName + ".SendLimit";
            /// <summary>
            /// 发送次数上限时长
            /// </summary>
            public const string SendLimitInterval = GroupName + ".SendLimitInterval";
        }

        public static class Webhook
        {
            public const string GroupName = Channel.GroupName + ".Webhook";
            /// <summary>
            /// 启用Webhook通道
            /// </summary>
            public const string Enable = GroupName + ".Enable";
            /// <summary>
            /// 发送次数上限
            /// </summary>
            public const string SendLimit = GroupName + ".SendLimit";
            /// <summary>
            /// 发送次数上限时长
            /// </summary>
            public const string SendLimitInterval = GroupName + ".SendLimitInterval";
        }

        public static class Email
        {
            public const string GroupName = Channel.GroupName + ".Email";
            /// <summary>
            /// 启用Email通道
            /// </summary>
            public const string Enable = GroupName + ".Enable";
            /// <summary>
            /// 发送次数上限
            /// </summary>
            public const string SendLimit = GroupName + ".SendLimit";
            /// <summary>
            /// 发送次数上限时长
            /// </summary>
            public const string SendLimitInterval = GroupName + ".SendLimitInterval";
        }

        public static class Sms
        {
            public const string GroupName = Channel.GroupName + ".Sms";
            /// <summary>
            /// 启用Sms通道
            /// </summary>
            public const string Enable = GroupName + ".Enable";
            /// <summary>
            /// 发送次数上限
            /// </summary>
            public const string SendLimit = GroupName + ".SendLimit";
            /// <summary>
            /// 发送次数上限时长
            /// </summary>
            public const string SendLimitInterval = GroupName + ".SendLimitInterval";
        }
    }
}
