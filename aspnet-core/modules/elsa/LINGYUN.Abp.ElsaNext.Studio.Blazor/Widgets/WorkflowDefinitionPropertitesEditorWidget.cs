using Elsa.Studio.Workflows.Widgets;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Components.WorkflowProperties.Tabs.Properties.Sections.Properties;
using Microsoft.AspNetCore.Components;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Widgets;

public class WorkflowDefinitionPropertitesEditorWidget : WorkflowDefinitionPropertiesWidgetBase
{
    public override double Order => 15;

    public override Func<IDictionary<string, object?>, RenderFragment> Render => attributes => builder =>
    {
        builder.OpenComponent<WorkflowDefinitionPropertitesEditor>(0);
        builder.AddAttribute(1, nameof(WorkflowDefinitionPropertitesEditor.WorkflowDefinition), attributes["WorkflowDefinition"]);
        builder.AddAttribute(2, nameof(WorkflowDefinitionPropertitesEditor.WorkflowDefinitionUpdated), attributes["WorkflowDefinitionUpdated"]);
        builder.CloseComponent();

    };
}
