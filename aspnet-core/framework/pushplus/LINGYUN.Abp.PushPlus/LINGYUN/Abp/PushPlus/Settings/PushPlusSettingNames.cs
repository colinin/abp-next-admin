namespace LINGYUN.Abp.PushPlus.Settings;

public static class PushPlusSettingNames
{
    public const string Prefix = "PushPlus";

    public static class Security
    {
        public const string Prefix = PushPlusSettingNames.Prefix + ".Security";

        public const string Token = Prefix + ".Token";

        public const string SecretKey = Prefix + ".SecretKey";
    }
}
