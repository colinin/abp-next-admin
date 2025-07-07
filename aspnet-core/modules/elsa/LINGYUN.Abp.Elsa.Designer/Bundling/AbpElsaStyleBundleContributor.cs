using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.Elsa.Designer.Bundling;

public class AbpElsaStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        var basePath = configuration["Hosting:BasePath"] ?? "";

        context.Files.AddIfNotContains($"{basePath}/_content/Elsa.Designer.Components.Web/elsa-workflows-studio/assets/fonts/inter/inter.css");
        context.Files.AddIfNotContains($"{basePath}/_content/Elsa.Designer.Components.Web/elsa-workflows-studio/elsa-workflows-studio.css");
    }
}
