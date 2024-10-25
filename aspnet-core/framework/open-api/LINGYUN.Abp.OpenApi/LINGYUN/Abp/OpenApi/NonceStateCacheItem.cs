using System;

namespace LINGYUN.Abp.OpenApi;

[Serializable]
public class NonceStateCacheItem
{
    private const string CacheKeyFormat = "open-api,nonce:{0}";

    public string Nonce { get; set; }

    public NonceStateCacheItem()
    {

    }

    public NonceStateCacheItem(string nonce)
    {
        Nonce = nonce;
    }

    public static string CalculateCacheKey(string nonce)
    {
        return string.Format(CacheKeyFormat, nonce);
    }
}
