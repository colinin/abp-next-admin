using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.ElsaNext.Studio.Bundling;

public class AbpElsaNextStudioScriptBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/BlazorMonaco/jsInterop.js");
        context.Files.AddIfNotContains("/_content/BlazorMonaco/lib/monaco-editor/min/vs/loader.js");
        context.Files.AddIfNotContains("/_content/BlazorMonaco/lib/monaco-editor/min/vs/editor/editor.main.js");
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.js");
        context.Files.AddIfNotContains("/_content/CodeBeam.MudBlazor.Extensions/MudExtensions.min.js");
        context.Files.AddIfNotContains("/_content/Radzen.Blazor/Radzen.Blazor.js");
        context.Files.AddIfNotContains("/_framework/blazor.webassembly.js");
    }
}
