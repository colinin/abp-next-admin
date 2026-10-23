namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

/// <summary>
/// Business actions recorded in an approval timeline. The key is persisted as a string so
/// clients can render a localized label without storing presentation text in the database.
/// </summary>
public enum ApprovalRecordAction
{
    Start,
    End,
    Approve,
    Reject,
    Return,
    Transfer,
    AddSign,
    RemoveSign,
    AddApproval,
    Withdraw,
    TimeoutReminder,
}
