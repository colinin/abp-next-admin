using Elsa.Studio;
using Elsa.Studio.Contracts;
using Elsa.Studio.Models;
using LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor.Components;
using LINGYUN.Abp.ElsaNext.UIHints;
using Microsoft.AspNetCore.Components;

namespace LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor.Handlers;

public class IdentityPickerHandler : IUIHintHandler
{
    public bool GetSupportsUIHint(string uiHint) => uiHint is AbpElsaUIHints.UserPicker;

    public string UISyntax => WellKnownSyntaxNames.Object;

    public RenderFragment DisplayInputEditor(DisplayInputEditorContext context)
    {
        return builder =>
        {
            builder.OpenComponent(0, typeof(IdentityUserPicker));
            builder.AddAttribute(1, nameof(IdentityUserPicker.EditorContext), context);
            builder.CloseComponent();
        };
    }
}
