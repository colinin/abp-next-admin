namespace LINGYUN.Abp.WxPusher.Settings;

public static class WxPusherSettingNames
{
    public const string Prefix = "WxPusher";

    public static class Security
    {
        public const string Prefix = WxPusherSettingNames.Prefix + ".Security";

        public const string AppToken = Prefix + ".AppToken";
    }
}
