namespace LINGYUN.Abp.OssManagement.Features
{
    public class AbpOssManagementFeatureNames
    {
        public const string GroupName = "AbpOssManagement";
        /// <summary>
        /// 是否运行未经授权的用户访问公共目录
        /// </summary>
        public const string PublicAccess = GroupName + ".PublicAccess";

        public class OssObject
        {
            public const string Default = GroupName + ".OssObject";
            /// <summary>
            /// 下载文件功能
            /// </summary>
            public const string DownloadFile = Default + ".DownloadFile";
            /// <summary>
            /// 下载文件功能限制次数
            /// </summary>
            public const string DownloadLimit = Default + ".DownloadLimit";
            /// <summary>
            /// 下载文件功能限制次数周期
            /// </summary>
            public const string DownloadInterval = Default + ".DownloadInterval";
            /// <summary>
            /// 上传文件功能
            /// </summary>
            public const string UploadFile = Default + ".UploadFile";
            /// <summary>
            /// 上传文件功能限制次数
            /// </summary>
            public const string UploadLimit = Default + ".UploadLimit";
            /// <summary>
            /// 上传文件功能限制次数周期
            /// </summary>
            public const string UploadInterval = Default + ".UploadInterval";
            /// <summary>
            /// 最大上传文件
            /// </summary>
            public const string MaxUploadFileCount = Default + ".MaxUploadFileCount";
        }
    }
}
