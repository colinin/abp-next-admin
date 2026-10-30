using Elsa.Api.Client.Shared.UIHints.DropDown;
using Elsa.Studio.Models;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Components;
using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor.Components;

public partial class NotificationPicker
{
    private SelectListItem? _selectedItem;

    [Parameter] public DisplayInputEditorContext EditorContext { get; set; } = null!;

    [Inject] protected IDialogService DialogService { get; set; } = null!;
    [Inject] private IStringLocalizerFactory StringLocalizerFactory { get; set; } = null!;
    [Inject] private INotificationDefinitionManager NotificationDefinitionManager { get; set; } = null!;

    protected async override Task OnInitializedAsync()
    {
        var selectedItem = await ResolveSelectedItemAsync();

        if (selectedItem != null)
        {
            await InvokeAsync(() => _selectedItem = selectedItem);
        }
    }

    private async Task<SelectListItem?> ResolveSelectedItemAsync()
    {
        var value = EditorContext.GetLiteralValueOrDefault();

        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(value);
        if (notificationDefine != null)
        {
            return new SelectListItem(notificationDefine.DisplayName.Localize(StringLocalizerFactory), notificationDefine.Name);
        }

        return new SelectListItem(value, value);
    }

    private async Task OpenSelectDialogAsync()
    {
        if (EditorContext.IsReadOnly)
        {
            return;
        }

        var inputDescriptor = EditorContext.InputDescriptor;
        var parameters = new DialogParameters<NotificationSelectDialog>
        {
            { x => x.Title, Localizer[inputDescriptor.DisplayName ?? inputDescriptor.Name] },
            { x => x.SelectedValue, _selectedItem?.Value }
        };
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        var dialog = await DialogService.ShowAsync<NotificationSelectDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: SelectListItem item })
        {
            await InvokeAsync(async () =>
            {
                _selectedItem = item;
                await EditorContext.UpdateValueOrLiteralExpressionAsync(item.Value);
            });
        }
    }

    private MudTextFieldExIcon[] GetAdornmentIcons() =>
    [
        new()
        {
            Icon = Icons.Material.Filled.Search,
            Tooltip = Localizer["Search"],
            Disabled = EditorContext.IsReadOnly,
            OnClick = OpenSelectDialogAsync
        },
        new()
        {
            Icon = Icons.Material.Filled.Clear,
            Tooltip = Localizer["Clear"],
            Visible = _selectedItem != null,
            Disabled = EditorContext.IsReadOnly,
            OnClick = ClearAsync
        }
    ];

    private async Task ClearAsync()
    {
        await InvokeAsync(async () =>
        {
            _selectedItem = null;
            await EditorContext.UpdateValueOrLiteralExpressionAsync(string.Empty);
        });
    }
}
