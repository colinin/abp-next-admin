namespace LINGYUN.Abp.OssManagement
{
    public static class OssManagementErrorCodes
    {
        public const string Namespace = "Abp.OssManagement";

        public const string ContainerDeleteWithStatic = Namespace + ":010000";
        public const string ContainerDeleteWithNotEmpty = Namespace + ":010001";
        public const string ContainerAlreadyExists = Namespace + ":010402";
        public const string ContainerNotFound = Namespace + ":010404";

        public const string ObjectDeleteWithNotEmpty = Namespace + ":020001";
        public const string ObjectAlreadyExists = Namespace + ":020402";
        public const string ObjectNotFound = Namespace + ":020404";

        public const string OssNameHasTooLong = Namespace + ":000405";
    }
}
