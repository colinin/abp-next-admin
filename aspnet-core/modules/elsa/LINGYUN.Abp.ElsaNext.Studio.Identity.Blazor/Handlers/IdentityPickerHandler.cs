using Elsa.Studio;
using Elsa.Studio.Contracts;
using Elsa.Studio.Models;
using LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor.Components;
using LINGYUN.Abp.ElsaNext.UIHints;
using Microsoft.AspNetCore.Components;

namespace LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor.Handlers;

public class IdentityPickerHandler : IUIHintHandler
{
    public bool GetSupportsUIHint(string uiHint) => uiHint is AbpElsaUIHints.UserPicker or AbpElsaUIHints.UserPickerMultiple;

    public string UISyntax => WellKnownSyntaxNames.Object;

    public RenderFragment DisplayInputEditor(DisplayInputEditorContext context)
    {
        var allowMultiple = context.InputDescriptor.UIHint == AbpElsaUIHints.UserPickerMultiple;

        return builder =>
        {
            builder.OpenComponent(0, typeof(IdentityUserPicker));
            builder.AddAttribute(1, nameof(IdentityUserPicker.EditorContext), context);
            builder.AddAttribute(2, nameof(IdentityUserPicker.AllowMultiple), true);
            builder.CloseComponent();
        };
    }
}
