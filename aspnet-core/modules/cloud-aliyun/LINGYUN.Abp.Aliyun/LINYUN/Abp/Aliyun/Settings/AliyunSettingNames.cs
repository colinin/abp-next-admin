namespace LINYUN.Abp.Aliyun.Settings
{
    public static class AliyunSettingNames
    {
        public const string Prefix = "Abp.Aliyun";

        /// <summary>
        /// 认证方式
        /// </summary>
        public class Authorization
        {
            public const string Prefix = AliyunSettingNames.Prefix + ".Authorization";

            public const string AccessKeyId = Prefix + ".AccessKeyId";

            public const string AccessKeySecret = Prefix + ".AccessKeySecret";
        }
    }
}
