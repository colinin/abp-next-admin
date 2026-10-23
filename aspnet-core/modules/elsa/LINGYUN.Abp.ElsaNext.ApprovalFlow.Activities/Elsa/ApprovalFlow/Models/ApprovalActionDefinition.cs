namespace Elsa.ApprovalFlow.Models;

public sealed class ApprovalActionDefinition
{
    public ApprovalActionKey Key { get; set; } = ApprovalActionKey.Approve;

    /// <summary>
    /// The action is rendered by its key so no UI language is baked into the workflow model.
    /// </summary>
    public string Label => Key.ToString();

    public ApprovalActionEffect Effect { get; set; } = ApprovalActionEffect.Approve;

    public bool RequireComment { get; set; }

    public bool Enabled { get; set; } = true;
}
