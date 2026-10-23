using Elsa.Api.Client.Resources.WorkflowDefinitions.Models;
using Elsa.Studio.Workflows.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Components.WorkflowProperties.Tabs.Properties.Sections.Properties;

public partial class WorkflowDefinitionPropertitesEditor
{
    [Parameter]
    public WorkflowDefinition WorkflowDefinition { get; set; } = default!;

    [Parameter]
    public EventCallback WorkflowDefinitionUpdated { get; set; }

    [CascadingParameter] public IWorkspace? Workspace { get; set; }

    [Inject] IDialogService DialogService { get; set; } = default!;

    private bool IsReadOnly => Workspace?.IsReadOnly ?? true;
    private ICollection<PropertyModel> Properties => WorkflowDefinition.CustomProperties
        .Select(x => new PropertyModel(x.Key, x.Value.ToString()!))
        .ToList();

    private async Task RaiseWorkflowDefinitionUpdatedAsync()
    {
        if (WorkflowDefinitionUpdated.HasDelegate)
        {
            await InvokeAsync(WorkflowDefinitionUpdated.InvokeAsync);
        }
    }

    private async Task OnEditClicked(PropertyModel property)
    {
        await OpenPropertyEditorDialog(property);
    }

    private async Task OnDeleteClicked(PropertyModel property)
    {
        var result = await DialogService.ShowMessageBoxAsync(Localizer["Delete selected property?"], Localizer["Are you sure you want to delete the selected property?"], yesText: Localizer["Delete"], cancelText: Localizer["Cancel"]);

        if (result != true)
        {
            return;
        }

        WorkflowDefinition.CustomProperties.RemoveAll(x => x.Key == property.Key);

        await RaiseWorkflowDefinitionUpdatedAsync();
    }

    private async Task OnAddPropertyClicked()
    {
        await OpenPropertyEditorDialog(null);
    }

    private async Task OpenPropertyEditorDialog(PropertyModel? property)
    {
        var isNew = property == null;

        var parameters = new DialogParameters<EditPropertyDialog>
        {
            [nameof(EditPropertyDialog.WorkflowDefinition)] = WorkflowDefinition
        };

        if (!isNew)
        {
            parameters[nameof(EditPropertyDialog.Property)] = property;
        }

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var title = property == null ? Localizer["Create property"] : Localizer["Edit property"];
        var dialog = await DialogService.ShowAsync<EditPropertyDialog>(title, parameters, options);
        var result = await dialog.Result;

        if (result?.Canceled != false)
        {
            return;
        }

        if (result.Data is PropertyModel newProperty)
        {
            WorkflowDefinition.CustomProperties[newProperty.Key] = newProperty.Value;
        }

        await RaiseWorkflowDefinitionUpdatedAsync();
    }
}
