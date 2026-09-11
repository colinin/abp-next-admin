using Blazored.FluentValidation;
using Elsa.Api.Client.Resources.WorkflowDefinitions.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Components.WorkflowProperties.Tabs.Properties.Sections.Properties;

public partial class EditPropertyDialog
{
    private bool _isEdit = false;
    private readonly PropertyModel _model = new();
    private EditContext _editContext = null!;
    private PropertyModelValidator _validator = null!;
    private FluentValidationValidator _fluentValidationValidator = null!;

    [Parameter] public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
    [Parameter] public PropertyModel? Property { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        _editContext = new(_model);
        _validator = new(WorkflowDefinition, Localizer);

        if (Property == null)
        {
            _model.Key = GetNewPropertyKey(WorkflowDefinition.CustomProperties);
            _isEdit = false;
        }
        else
        {
            _model.Key = Property.Key;
            _model.Value = Property.Value;
            _isEdit = true;
        }
    }

    private string GetNewPropertyKey(IDictionary<string, object> existingPropertites)
    {
        var count = 0;

        while (true)
        {
            var key = $"Property{++count}";

            if (existingPropertites.All(x => x.Key != key))
            {
                return key;
            }
        }
    }

    private Task OnCancelClicked()
    {
        MudDialog.Cancel();
        return Task.CompletedTask;
    }

    private async Task OnSubmitClicked()
    {
        if (!await _fluentValidationValidator.ValidateAsync())
        {
            return;
        }

        await OnValidSubmit();
    }

    private Task OnValidSubmit()
    {
        var property = Property ?? new PropertyModel();

        property.Key = _model.Key;
        property.Value = _model.Value;

        MudDialog.Close(property);
        return Task.CompletedTask;
    }
}
