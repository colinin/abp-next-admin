using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.Elsa.Designer.Bundling;

public class AbpElsaScriptBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        var basePath = configuration["Hosting:BasePath"] ?? "";

        context.Files.AddIfNotContains($"{basePath}/_content/Elsa.Designer.Components.Web/monaco-editor/min/vs/loader.js");
    }
}
