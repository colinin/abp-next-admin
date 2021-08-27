using LINGYUN.Abp.Rules.RulesEngine.FileProviders.Physical;
using LINGYUN.Abp.Tests;
using System.IO;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    [DependsOn(
        typeof(AbpRulesEngineModule),
        typeof(AbpTestsBaseModule))]
    public class AbpRulesEngineTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpRulesEnginePhysicalFileResolveOptions>(options =>
            {
                options.PhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "Rules");
            });
        }
    }
}
