using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    [DependsOn(
        typeof(AbpLoggingModule),
        typeof(AbpElasticsearchModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpJsonModule))]
    public class AbpLoggingSerilogElasticsearchModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpLoggingSerilogElasticsearchOptions>(configuration.GetSection("Logging:Serilog:Elasticsearch"));

            context.Services.AddAutoMapperObjectMapper<AbpLoggingSerilogElasticsearchModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpLoggingSerilogElasticsearchMapperProfile>(validate: true);
            });
        }
    }
}
