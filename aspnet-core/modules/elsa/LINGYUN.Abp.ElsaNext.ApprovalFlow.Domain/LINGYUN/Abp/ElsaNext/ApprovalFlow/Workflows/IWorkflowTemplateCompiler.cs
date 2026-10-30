using Elsa.Workflows.Activities;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using System.Text.Json;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

/// <summary>
/// Compiles a workflow template definition (designer document) into an Elsa execution sequence.
/// </summary>
public interface IWorkflowTemplateCompiler
{
    /// <summary>
    /// Returns <c>true</c> when this compiler supports the supplied template document.
    /// </summary>
    bool CanCompile(JsonElement rootElement);

    /// <summary>
    /// Compiles the template document into an Elsa sequence.
    /// </summary>
    Sequence Compile(JsonElement rootElement, ApprovalTemplate template);
}
