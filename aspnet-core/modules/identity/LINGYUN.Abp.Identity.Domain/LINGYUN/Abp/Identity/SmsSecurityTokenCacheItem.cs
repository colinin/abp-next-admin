namespace LINGYUN.Abp.Identity
{
    /// <summary>
    /// 短信安全令牌验证缓存
    /// </summary>
    public class SmsSecurityTokenCacheItem
    {
        /// <summary>
        /// 用于验证的Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用于验证的安全令牌
        /// </summary>
        public string SecurityToken { get; set; }

        public SmsSecurityTokenCacheItem()
        {

        }

        public SmsSecurityTokenCacheItem(string token, string securityToken)
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
        public static string CalculateCacheKey(string phoneNumber, string purpose)
        {
            return "Totp:" + purpose + ";p:" + phoneNumber;
        }
    }
}
