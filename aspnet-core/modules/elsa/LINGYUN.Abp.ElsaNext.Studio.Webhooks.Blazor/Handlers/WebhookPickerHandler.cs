using Elsa.Studio;
using Elsa.Studio.Contracts;
using Elsa.Studio.Models;
using LINGYUN.Abp.ElsaNext.Studio.Webhooks.Blazor.Components;
using LINGYUN.Abp.ElsaNext.Webhooks.UIHints;
using Microsoft.AspNetCore.Components;

namespace LINGYUN.Abp.ElsaNext.Studio.Webhooks.Blazor.Handlers;

public class WebhookPickerHandler : IUIHintHandler
{
    public bool GetSupportsUIHint(string uiHint) => uiHint is WebhooksUIHints.WebhookPicker;

    public string UISyntax => WellKnownSyntaxNames.Object;

    public RenderFragment DisplayInputEditor(DisplayInputEditorContext context)
    {
        return builder =>
        {
            builder.OpenComponent(0, typeof(WebhookPicker));
            builder.AddAttribute(1, nameof(WebhookPicker.EditorContext), context);
            builder.CloseComponent();
        };
    }
}
