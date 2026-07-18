using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.ElsaNext.Studio.Bundling;

public class AbpElsaNextStudioStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap");
        context.Files.AddIfNotContains("https://fonts.googleapis.com/css2?family=Ubuntu:wght@300;400;500;700&display=swap");
        context.Files.AddIfNotContains("https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500;600;700&display=swap");
        context.Files.AddIfNotContains("https://fonts.googleapis.com/css2?family=Grandstander:wght@100&display=swap");
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.css");
        context.Files.AddIfNotContains("/_content/CodeBeam.MudBlazor.Extensions/MudExtensions.min.css");
        context.Files.AddIfNotContains("/_content/Radzen.Blazor/css/material-base.css");
        context.Files.AddIfNotContains("/_content/Elsa.Studio.Shell/css/shell.css");
        context.Files.AddIfNotContains("/ElsaStudioBlazorWasm.styles.css");
    }
}
