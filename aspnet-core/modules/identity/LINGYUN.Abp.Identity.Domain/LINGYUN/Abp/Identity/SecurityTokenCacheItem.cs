namespace LINGYUN.Abp.Identity
{
    /// <summary>
    /// 安全令牌验证缓存
    /// </summary>
    public class SecurityTokenCacheItem
    {
        /// <summary>
        /// 用于验证的Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用于验证的安全令牌
        /// </summary>
        public string SecurityToken { get; set; }

        public SecurityTokenCacheItem()
        {

        }

        public SecurityTokenCacheItem(string token, string securityToken)
        {
            Token = token;
            SecurityToken = securityToken;
        }

        /// <summary>
        /// 生成查询Key
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="purpose">安全令牌用途</param>
        /// <returns></returns>
        public static string CalculateSmsCacheKey(string phoneNumber, string purpose)
        {
            return "Totp:" + purpose + ";p:" + phoneNumber;
        }

        /// <summary>
        /// 生成查询Key
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="purpose">安全令牌用途</param>
        /// <returns></returns>
        public static string CalculateEmailCacheKey(string email, string purpose)
        {
            return "Totp:" + purpose + ";e:" + email;
        }
    }
}
