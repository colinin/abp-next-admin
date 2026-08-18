namespace LINGYUN.Abp.Gdpr;

public class GdprInfoCacheItem
{
    public string Data { get; set; } = default!;
    public string Provider { get; set; } = default!;
    public GdprInfoCacheItem()
    {

    }

    public GdprInfoCacheItem(string provider, string data)
    {
        Data = data;
        Provider = provider;
    }
}
