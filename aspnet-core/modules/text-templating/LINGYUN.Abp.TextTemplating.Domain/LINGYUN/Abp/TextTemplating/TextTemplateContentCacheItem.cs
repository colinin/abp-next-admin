using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TextTemplating;

[Serializable]
[IgnoreMultiTenancy]
public class TextTemplateContentCacheItem
{
    private const string CacheKeyFormat = "pn:template-content,n:{0},c:{1}";

    public string Name { get; set; }
    public string Culture { get; set; }
    public string Content { get; set; }
    public TextTemplateContentCacheItem()
    {

    }

    public TextTemplateContentCacheItem(
        string name,
        string content,
        string culture = null)
    {
        Name = name;
        Content = content;
        Culture = culture;
    }

    public static string CalculateCacheKey(
        string name, 
        string culture = null)
    {
        return string.Format(
            CacheKeyFormat,
            name,
            culture);
    }
}
