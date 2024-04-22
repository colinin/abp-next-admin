using LINGYUN.Abp.Elasticsearch.Jobs.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Elasticsearch.Jobs;

internal static class LocalizableStatic
{
    public static ILocalizableString Create(string name)
    {
        return LocalizableString.Create<ElasticsearchJobsResource>(name);
    }
}
