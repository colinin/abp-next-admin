namespace LINGYUN.Abp.LocalizationManagement
{
    public static class LocalizationDbProperties
    {
        public static string DbTablePrefix { get; set; } = "AbpLocalization";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "AbpLocalizationManagement";
    }
}
