namespace LINGYUN.Abp.Dingtalk;

public class DingtalkAccessTokenCacheItem
{
    public string AccessToken { get; set; }

    public long? ExpireIn { get; set; }
    public DingtalkAccessTokenCacheItem()
    {

    }

    public DingtalkAccessTokenCacheItem(string accessToken, long? expireIn)
    {
        AccessToken = accessToken;
        ExpireIn = expireIn;
    }
}
