namespace LINGYUN.Abp.TuiJuhe.Features;

public static class TuiJuheFeatureNames
{
    public const string GroupName = "TuiJuhe";

    /// <summary>
    /// 启用TuiJuhe
    /// </summary>
    public const string Enable = GroupName + ".Enable";

    public static class Message
    {
        public const string GroupName = TuiJuheFeatureNames.GroupName + ".Message";
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
