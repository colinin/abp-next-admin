namespace LINGYUN.Abp.Account.OAuth.Settings;

public static class AccountOAuthSettingNames
{
    public const string GroupName = "Abp.Account.OAuth";
    public static class GitHub
    {
        public const string Prefix = GroupName + ".GitHub";
        /// <summary>
        /// ClientId
        /// </summary>
        public const string ClientId = Prefix + ".ClientId";
        /// <summary>
        /// ClientSecret
        /// </summary>
        public const string ClientSecret = Prefix + ".ClientSecret";
    }
    public static class Bilibili
    {
        public const string Prefix = GroupName + ".Bilibili";
        /// <summary>
        /// ClientId
        /// </summary>
        public const string ClientId = Prefix + ".ClientId";
        /// <summary>
        /// ClientSecret
        /// </summary>
        public const string ClientSecret = Prefix + ".ClientSecret";
    }
}
