namespace LINGYUN.Abp.AI;
public static class AbpAIErrorCodes
{
    public const string Namespace = "Abp.AI";
    /// <summary>
    /// 工作区不可用: {Workspace}!
    /// </summary>
    public const string WorkspaceIsNotEnabled = Namespace + ":110001";
    /// <summary>
    /// 对话已过期, 请重新创建会话!
    /// </summary>
    public const string ConversationHasExpired = Namespace + ":110101";
}
