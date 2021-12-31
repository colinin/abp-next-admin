using LINGYUN.Abp.Tencent.Settings;

namespace LINGYUN.Abp.Sms.Tencent.Settings
{
    public static class TencentCloudSmsSettingNames
    {
        public const string Prefix = TencentCloudSettingNames.Prefix + ".Sms";

        /// <summary>
        /// 短信 SdkAppId
        /// 在 短信控制台 添加应用后生成的实际 SdkAppId，示例如1400006666。
        /// </summary>
        public const string AppId = Prefix + ".Domain";
        /// <summary>
        /// 短信签名内容
        /// </summary>
        public const string DefaultSignName = Prefix + ".DefaultSignName";
        /// <summary>
        /// 默认短信模板 ID
        /// </summary>
        public const string DefaultTemplateId = Prefix + ".DefaultTemplateId";
    }
}
