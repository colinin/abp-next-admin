using LINGYUN.Abp.Tests;
using System.IO;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    [DependsOn(
        typeof(AbpRulesEngineModule),
        typeof(AbpTestsBaseModule))]
    public class AbpRulesEngineTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();

            Configure<AbpRulesEngineOptions>(options =>
            {
                options.PhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "Rules");
            });
        }
    }
}
