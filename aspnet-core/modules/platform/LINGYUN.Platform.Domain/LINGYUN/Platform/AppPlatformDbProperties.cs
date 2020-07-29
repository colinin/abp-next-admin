namespace LINGYUN.Platform
{
    public static class AppPlatformDbProperties
    {
        public static string DbTablePrefix { get; set; } = "AppPlatform";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "AppPlatform";
    }
}
