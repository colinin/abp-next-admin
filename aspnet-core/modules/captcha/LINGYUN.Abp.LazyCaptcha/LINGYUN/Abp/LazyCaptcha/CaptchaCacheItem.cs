using Volo.Abp.Caching;

namespace LINGYUN.Abp.LazyCaptcha;

[CacheName("LazyCaptcha")]
public class CaptchaCacheItem
{
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
    public CaptchaCacheItem()
    {

    }

    public CaptchaCacheItem(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
