namespace LINGYUN.Abp.Tencent.Features
{
    public static class TencentCloudFeatures
    {
        public const string GroupName = "Abp.TencentCloud";

        public static class Sms
        {
            public const string GroupName = TencentCloudFeatures.GroupName + ".Sms";
            /// <summary>
            /// 启用短信
            /// </summary>
            public const string Enable = GroupName + ".Enable";
        }

        public static class BlobStoring
        {
            public const string GroupName = TencentCloudFeatures.GroupName + ".BlobStoring";
            /// <summary>
            /// 启用对象存储
            /// </summary>
            public const string Enable = GroupName + ".Enable";
            /// <summary>
            /// 最大流大小限制, 小于等于0无效, 单位(MB)
            /// 默认： 0
            /// </summary>
            public const string MaximumStreamSize = GroupName + ".MaximumStreamSize";
        }
    }
}
