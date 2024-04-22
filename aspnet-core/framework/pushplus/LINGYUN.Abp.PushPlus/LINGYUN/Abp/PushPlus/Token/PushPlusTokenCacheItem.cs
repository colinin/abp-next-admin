namespace LINGYUN.Abp.PushPlus.Token;

public class PushPlusTokenCacheItem
{
    public const string KeyFormat = "t:{0};s:{1}";

    public string AccessKey { get; set; }

    public int ExpiresIn { get; set; }

    public PushPlusTokenCacheItem()
    {

    }

    public PushPlusTokenCacheItem(string accessKey, int expiresIn)
    {
        AccessKey = accessKey;
        ExpiresIn = expiresIn;
    }

    public static string CalculateCacheKey(string token, string secretKey)
    {
        return string.Format(KeyFormat, token, secretKey);
    }
}
