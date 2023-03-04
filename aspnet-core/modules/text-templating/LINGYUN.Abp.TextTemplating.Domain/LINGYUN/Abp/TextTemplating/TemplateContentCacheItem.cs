using System;

namespace LINGYUN.Abp.TextTemplating;

[Serializable]
public class TemplateContentCacheItem
{
    private const string CacheKeyFormat = "pn:template-content,n:{0},c:{1}";

    public string Name { get; set; }
    public string Culture { get; set; }
    public string Content { get; set; }
    public TemplateContentCacheItem()
    {

    }

    public TemplateContentCacheItem(
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
