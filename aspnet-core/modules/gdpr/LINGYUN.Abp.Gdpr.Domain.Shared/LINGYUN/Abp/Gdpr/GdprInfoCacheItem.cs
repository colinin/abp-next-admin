namespace LINGYUN.Abp.Gdpr;

public class GdprInfoCacheItem
{
    public string Data { get; set; }
    public string Provider { get; set; }
    public GdprInfoCacheItem()
    {

    }

    public GdprInfoCacheItem(string provider, string data)
    {
        Data = data;
        Provider = provider;
    }
}
