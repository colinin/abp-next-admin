using System;

namespace LINGYUN.Abp.Aliyun;

[Serializable]
public class AliyunBasicSessionCredentialsCacheItem
{
    public string AccessKeyId { get; set; }
    public string AccessKeySecret { get; set; }
    public string SecurityToken { get; set; }
    public DateTime? Expiration { get; set; }

    public AliyunBasicSessionCredentialsCacheItem()
    {

    }

    public AliyunBasicSessionCredentialsCacheItem(string accessKeyId, string accessKeySecret, string securityToken, DateTime? expiration = null)
    {
        AccessKeyId = accessKeyId;
        AccessKeySecret = accessKeySecret;
        SecurityToken = securityToken;
        Expiration = expiration;
    }
}
