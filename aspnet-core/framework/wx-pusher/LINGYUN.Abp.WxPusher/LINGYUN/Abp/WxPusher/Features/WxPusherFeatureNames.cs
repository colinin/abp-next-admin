namespace LINGYUN.Abp.WxPusher.Features;

public static class WxPusherFeatureNames
{
    public const string GroupName = "WxPusher";

    /// <summary>
    /// 启用WxPusher
    /// </summary>
    public const string Enable = GroupName + ".Enable";

    public static class Message
    {
        public const string GroupName = WxPusherFeatureNames.GroupName + ".Message";
        /// <summary>
        /// 启用消息推送
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
