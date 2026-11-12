namespace LINGYUN.Abp.PrintDesigner.Permissions;

public static class PrintDesignerPermissions
{
    public const string GroupName = "PrintDesigner";

    public static class CustomElement
    {
        public const string Default = GroupName + ".CustomElements";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Copy = Default + ".Copy";
    }

    public static class Template
    {
        public const string Default = GroupName + ".Templates";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Copy = Default + ".Copy";
    }
}
