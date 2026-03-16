using Volo.Abp.Reflection;

namespace LINGYUN.Abp.AIManagement.Permissions;

public static class AIManagementPermissionNames
{
    public const string GroupName = "AIManagement";

    public static class WorkspaceDefinition
    {
        public const string Default = GroupName + ".WorkspaceDefinitions";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Conversation
    {
        public const string Default = GroupName + ".Conversations";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Chat
    {
        public const string Default = GroupName + ".Chats";

        public const string SendMessage = Default + ".SendMessage";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AIManagementPermissionNames));
    }
}
