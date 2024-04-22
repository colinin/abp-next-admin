using System;

namespace LINGYUN.Abp.Aliyun
{
    [Serializable]
    public class AliyunBasicSessionCredentialsCacheItem
    {
        private readonly static string _cacheKey;
        public static string CacheKey => _cacheKey;
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string SecurityToken { get; set; }

        static AliyunBasicSessionCredentialsCacheItem()
        {
            _cacheKey = Guid.NewGuid().ToString("N");
        }

        public AliyunBasicSessionCredentialsCacheItem()
        {

        }

        public AliyunBasicSessionCredentialsCacheItem(string accessKeyId, string accessKeySecret, string securityToken)
        {
            AccessKeyId = accessKeyId;
            AccessKeySecret = accessKeySecret;
            SecurityToken = securityToken;
        }
    }
}
