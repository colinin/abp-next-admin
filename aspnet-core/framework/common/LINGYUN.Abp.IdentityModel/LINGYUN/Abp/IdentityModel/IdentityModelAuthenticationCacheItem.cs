using System;

namespace LINGYUN.Abp.IdentityModel
{
    public class IdentityModelAuthenticationCacheItem
    {
        public string AccessToken { get; set; }
        public IdentityModelAuthenticationCacheItem()
        {

        }

        public IdentityModelAuthenticationCacheItem(string accessToken)
        {
            AccessToken = accessToken;
        }

        public static string CalculateCacheKey(string grantType, string clientId, string userName = null)
        {
            if (userName.IsNullOrWhiteSpace())
            {
                return "gt:" + grantType + ",ci:" + clientId;
            }
            return "gt:" + grantType + ",ci:" + clientId + ",un:" + userName;
        }
    }
}
