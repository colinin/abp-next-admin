namespace LINGYUN.Abp.OpenApi
{
    public static class AbpOpenApiConsts
    {
        public const string SecurityChecking = "_AbpOpenApiSecurityChecking";

        public const string AppKeyFieldName = "appKey";
        public const string SignatureFieldName = "sign";
        public const string TimeStampFieldName = "t";

        public const string KeyPrefix = "AbpOpenApi";

        public const string InvalidAccessWithAppKey = KeyPrefix + ":9100";
        public const string InvalidAccessWithAppKeyNotFound = KeyPrefix + ":9101";

        public const string InvalidAccessWithSign = KeyPrefix + ":9110";
        public const string InvalidAccessWithSignNotFound = KeyPrefix + ":9111";

        public const string InvalidAccessWithTimestamp = KeyPrefix + ":9210";
        public const string InvalidAccessWithTimestampNotFound = KeyPrefix + ":9211";
    }
}
