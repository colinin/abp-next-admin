namespace LINGYUN.Abp.BlobManagement;

public static class BlobManagementErrorCodes
{
    public const string Namespace = "BlobManagement";

    public static class Container
    {
        public const string Prefix = Namespace + ":001";
        public const string NameAlreadyExists = Prefix + "001";
        public const string DeleteWithNotEmpty = Prefix + "002";
        public const string DeleteWithStatic = Prefix + "003";
        public const string NameNotFound = Prefix + "004";
    }

    public static class Blob
    {
        public const string Prefix = Namespace + ":002";
        public const string NonFolderChildBlob = Prefix + "001";
        public const string NameAlreadyExists = Prefix + "002";
        public const string DeleteWithNotEmpty = Prefix + "003";
        public const string NameNotFound = Prefix + "004";
        public const string BlobHashValidFailed = Prefix + "005";
        public const string UploadFileSizeTooLong = Prefix + "006";
        public const string UploadFileExtendCanNotBeMatch = Prefix + "007";
    }
}
