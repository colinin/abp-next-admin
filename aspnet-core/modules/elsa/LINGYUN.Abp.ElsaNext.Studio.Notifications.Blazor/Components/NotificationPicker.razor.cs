using Elsa.Api.Client.Shared.UIHints.DropDown;
using Elsa.Studio.Models;
using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor.Components;

public partial class NotificationPicker
{
    private ICollection<SelectListItem> _items = [];
    private IReadOnlyList<SelectListItem> _notifications = [];

    [Parameter] public DisplayInputEditorContext EditorContext { get; set; } = null!;

    [Inject] private IStringLocalizerFactory StringLocalizerFactory { get; set; } = null!;
    [Inject] private INotificationDefinitionManager NotificationDefinitionManager { get; set; } = null!;

    private SelectListItem? GetSelectedValue()
    {
        var value = EditorContext.GetLiteralValueOrDefault();
        return _items.FirstOrDefault(x => x.Value == value);
    }

    private void OnSearch(string? filter)
    {
        _items = _notifications
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Text.Contains(filter!))
            .Take(10)
            .ToList();
    }

    private async Task OnValueChanged(SelectListItem? value)
    {
        await EditorContext.UpdateValueOrLiteralExpressionAsync(value?.Value ?? "");
    }

    protected async override Task OnInitializedAsync()
    {
        var notifications = await NotificationDefinitionManager.GetNotificationsAsync();

        _notifications = notifications
            .Select(notification => 
                new SelectListItem(
                    notification.DisplayName.Localize(StringLocalizerFactory), notification.Name))
            .OrderBy(x => x.Text)
            .ToList();

        _items = _notifications.Take(10).ToList();
    }
}
