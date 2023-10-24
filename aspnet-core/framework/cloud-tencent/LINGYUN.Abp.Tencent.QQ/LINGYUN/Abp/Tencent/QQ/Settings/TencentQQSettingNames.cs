using LINGYUN.Abp.Tencent.Settings;

namespace LINGYUN.Abp.Tencent.QQ.Settings
{
    public class TencentQQSettingNames
    {
        public static class QQConnect
        {
            private const string Prefix = TencentCloudSettingNames.Prefix + ".QQConnect";

            public const string AppId = Prefix + ".AppId";
            public const string AppKey = Prefix + ".AppKey";
            public const string IsMobile = Prefix + ".IsMobile";
        }
    }
}
