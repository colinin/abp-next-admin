using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor.Bundling;

public class StructuredLogsStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Elsa.Studio.Diagnostics.StructuredLogs/structuredLogs.css");
        context.Files.AddIfNotContains("/_content/LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor/structuredLogs-overrides.css");
    }
}
