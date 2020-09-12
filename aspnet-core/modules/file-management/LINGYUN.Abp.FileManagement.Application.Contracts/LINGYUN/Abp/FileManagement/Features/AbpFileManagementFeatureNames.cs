namespace LINGYUN.Abp.FileManagement.Features
{
    public class AbpFileManagementFeatureNames
    {
        public const string GroupName = "Abp.FileManagement";
        public class FileSystem
        {
            public const string Default = GroupName + ".FileSystem";
            /// <summary>
            /// 下载文件功能
            /// </summary>
            public const string DownloadFile = Default + ".DownloadFile";
            /// <summary>
            /// 上传文件功能
            /// </summary>
            public const string UploadFile = Default + ".UploadFile";
            /// <summary>
            /// 最大上传文件
            /// </summary>
            public const string MaxUploadFileCount = Default + ".MaxUploadFileCount";
        }
    }
}
