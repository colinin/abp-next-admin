namespace LINGYUN.Abp.TuiJuhe.Settings;

public static class TuiJuheSettingNames
{
    public const string Prefix = "TuiJuhe";

    public static class Security
    {
        public const string Prefix = TuiJuheSettingNames.Prefix + ".Security";

        public const string Token = Prefix + ".Token";
    }
}
