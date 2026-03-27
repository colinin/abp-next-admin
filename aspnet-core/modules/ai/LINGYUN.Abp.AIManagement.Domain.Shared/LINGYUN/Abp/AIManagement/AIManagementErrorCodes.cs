namespace LINGYUN.Abp.AIManagement;
public static class AIManagementErrorCodes
{
    public const string Namespace = "AIManagement";

    public static class Workspace
    {
        public const string Prefix = Namespace + ":111";

        /// <summary>
        /// Workspace {Workspace} already exists!
        /// </summary>
        public const string NameAlreadyExists = Prefix + "001";
        /// <summary>
        /// System workspace {Workspace} is not allowed to be deleted!
        /// </summary>
        public const string SystemWorkspaceNotAllowedToBeDeleted = Prefix + "002";
    }

    public static class AITool
    {
        public const string Prefix = Namespace + ":112";

        /// <summary>
        /// AITool {AITool} already exists!
        /// </summary>
        public const string NameAlreadyExists = Prefix + "001";
        /// <summary>
        /// System AITool {AITool} is not allowed to be deleted!
        /// </summary>
        public const string SystemAIToolNotAllowedToBeDeleted = Prefix + "002";
    }
}
