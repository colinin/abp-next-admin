using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.Elsa.Designer.Bundling;

public class AbpElsaScriptBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Elsa.Designer.Components.Web/monaco-editor/min/vs/loader.js");
    }
}
