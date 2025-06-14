namespace LINGYUN.Abp.Account.OAuth.Features;

public static class AccountOAuthFeatureNames
{
    public const string GroupName = "Abp.Account.OAuth";
    public static class GitHub
    {
        public const string Prefix = GroupName + ".GitHub";
        /// <summary>
        /// 启用Github认证登录
        /// </summary>
        public const string Enable = Prefix + ".Enable";
    }
    public static class QQ
    {
        public const string Prefix = GroupName + ".QQ";
        /// <summary>
        /// 启用QQ认证登录
        /// </summary>
        public const string Enable = Prefix + ".Enable";
    }
    public static class WeChat
    {
        public const string Prefix = GroupName + ".WeChat";
        /// <summary>
        /// 启用微信认证登录
        /// </summary>
        public const string Enable = Prefix + ".Enable";
    }
    public static class WeCom
    {
        public const string Prefix = GroupName + ".WeCom";
        /// <summary>
        /// 启用企业微信认证登录
        /// </summary>
        public const string Enable = Prefix + ".Enable";
    }
    public static class Bilibili
    {
        public const string Prefix = GroupName + ".Bilibili";
        /// <summary>
        /// 启用Bilibili认证登录
        /// </summary>
        public const string Enable = Prefix + ".Enable";
    }
}
