using LINGYUN.Platform.Localization;
using LINGYUN.Platform.Menus;
using System.Collections.Generic;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Platform
{
    [DependsOn(typeof(PlatformDomainSharedModule))]
    public class PlatformApplicationContractModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                // 2020-12-26 对于结构中的 Meta 字段，需要使用 Newtonsoft.Json 来序列化
                options.UnsupportedTypes.AddIfNotContains(typeof(MenuCreateDto));
                options.UnsupportedTypes.AddIfNotContains(typeof(MenuUpdateDto));
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<PlatformApplicationContractModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PlatformResource>()
                    .AddVirtualJson("/LINGYUN/Platform/Localization/ApplicationContracts");
            });
        }
    }
}
