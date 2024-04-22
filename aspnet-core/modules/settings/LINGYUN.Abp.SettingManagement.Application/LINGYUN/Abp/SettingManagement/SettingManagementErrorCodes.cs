namespace LINGYUN.Abp.SettingManagement;
public static class SettingManagementErrorCodes
{
    public const string Namespace = "SettingManagement";

    public static class Definition
    {
        public const string Prefix = Namespace + ":001";

        public const string DuplicateName = Prefix + "001";

        public const string StaticSettingNotAllowedChanged = Prefix + "010";
    }
}
