using LINGYUN.Abp.Aliyun.Settings;

namespace LINGYUN.Abp.Sms.Aliyun.Settings
{
    public static class AliyunSmsSettingNames
    {
        /// <summary>
        /// 短信服务
        /// </summary>
        public class Sms
        {
            public const string Prefix = AliyunSettingNames.Prefix + ".Sms";
            /// <summary>
            /// 阿里云sms服务域名
            /// </summary>
            public const string Domain = Prefix + ".Domain";
            /// <summary>
            /// 调用方法名称
            /// </summary>
            public const string ActionName = Prefix + ".ActionName";
            /// <summary>
            /// 默认版本号
            /// </summary>
            public const string Version = Prefix + ".Version";
            /// <summary>
            /// 默认签名
            /// </summary>
            public const string DefaultSignName = Prefix + ".DefaultSignName";
            /// <summary>
            /// 默认短信模板号
            /// </summary>
            public const string DefaultTemplateCode = Prefix + ".DefaultTemplateCode";
            /// <summary>
            /// 默认号码
            /// </summary>
            public const string DefaultPhoneNumber = Prefix + ".DefaultPhoneNumber";
            /// <summary>
            /// 展示错误给客户端
            /// </summary>
            public const string VisableErrorToClient = Prefix + ".VisableErrorToClient";
        }
    }
}
