using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.Elasticsearch.Jobs.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Elasticsearch.Jobs;

[DependsOn(typeof(AbpTimingModule))]
[DependsOn(typeof(AbpElasticsearchModule))]
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
public class AbpElasticsearchJobsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElasticsearchJobsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ElasticsearchJobsResource>()
                .AddVirtualJson("/LINGYUN/Abp/Elasticsearch/Jobs/Localization/Resources");
        });
    }
}