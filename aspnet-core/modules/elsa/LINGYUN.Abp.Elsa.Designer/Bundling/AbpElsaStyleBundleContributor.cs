using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.Elsa.Designer.Bundling;

public class AbpElsaStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Elsa.Designer.Components.Web/elsa-workflows-studio/assets/fonts/inter/inter.css");
        context.Files.AddIfNotContains("/_content/Elsa.Designer.Components.Web/elsa-workflows-studio/elsa-workflows-studio.css");
    }
}
