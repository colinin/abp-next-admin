namespace LINGYUN.Abp.BlobManagement.Features;

public static class BlobManagementFeatureNames
{
    public const string GroupName = "AbpBlobManagement";
    /// <summary>
    /// 启用对象存储管理
    /// </summary>
    public const string Enable = GroupName + ".Enable";
    /// <summary>
    /// 是否允许未经授权的用户访问公共目录
    /// </summary>
    public const string PublicAccess = GroupName + ".PublicAccess";

    public class Blob
    {
        public const string Default = GroupName + ".Blob";
        /// <summary>
        /// 启用Blob
        /// </summary>
        public const string Enable = Default + ".Enable";
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
