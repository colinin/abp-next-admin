namespace LINGYUN.Abp.AIManagement;
public static class AIManagementErrorCodes
{
    public const string Namespace = "AIManagement";

    public static class Workspace
    {
        public const string Prefix = Namespace + ":100";

        public const string NameAlreadyExists = Prefix + "001";
    }
}
