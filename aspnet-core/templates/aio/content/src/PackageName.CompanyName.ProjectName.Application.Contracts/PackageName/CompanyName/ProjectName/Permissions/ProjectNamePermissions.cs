namespace PackageName.CompanyName.ProjectName.Permissions;

public static class ProjectNamePermissions
{
    public const string GroupName = "ProjectName";

    public const string ManageSettings = GroupName + ".ManageSettings";
    
    public class User
    {
        public const string Default = GroupName + ".User";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
