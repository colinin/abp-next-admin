using Elsa.Studio;
using Elsa.Studio.Contracts;
using Elsa.Studio.Models;
using LINGYUN.Abp.ElsaNext.Studio.Saas.Blazor.Components;
using LINGYUN.Abp.ElsaNext.UIHints;
using Microsoft.AspNetCore.Components;

namespace LINGYUN.Abp.ElsaNext.Studio.Saas.Blazor.Handlers;

public class TenantPickerHandler : IUIHintHandler
{
    public bool GetSupportsUIHint(string uiHint) => uiHint is AbpElsaUIHints.TenantPicker;

    public string UISyntax => WellKnownSyntaxNames.Object;

    public RenderFragment DisplayInputEditor(DisplayInputEditorContext context)
    {
        return builder =>
        {
            builder.OpenComponent(0, typeof(TenantPicker));
            builder.AddAttribute(1, nameof(TenantPicker.EditorContext), context);
            builder.CloseComponent();
        };
    }
}
