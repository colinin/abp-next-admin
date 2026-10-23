using Elsa.Studio;
using Elsa.Studio.Contracts;
using Elsa.Studio.Models;
using LINGYUN.Abp.ElsaNext.Notifications.UIHints;
using LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor.Components;
using Microsoft.AspNetCore.Components;

namespace LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor.Handlers;

public class NotificationPickerHandler : IUIHintHandler
{
    public bool GetSupportsUIHint(string uiHint) => uiHint is NotificationsUIHints.NotificationPicker;

    public string UISyntax => WellKnownSyntaxNames.Object;

    public RenderFragment DisplayInputEditor(DisplayInputEditorContext context)
    {
        return builder =>
        {
            builder.OpenComponent(0, typeof(NotificationPicker));
            builder.AddAttribute(1, nameof(NotificationPicker.EditorContext), context);
            builder.CloseComponent();
        };
    }
}
