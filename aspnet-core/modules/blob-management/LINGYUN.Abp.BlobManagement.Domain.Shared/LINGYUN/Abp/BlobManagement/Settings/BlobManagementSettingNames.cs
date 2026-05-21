namespace LINGYUN.Abp.BlobManagement.Settings;

public static class BlobManagementSettingNames
{
    public const string GroupName = "Abp.BlobManagement";
    /// <summary>
    /// 文件限制长度
    /// </summary>
    public const string FileLimitLength = GroupName + ".FileLimitLength";
    /// <summary>
    /// 允许的文件扩展名类型
    /// </summary>
    public const string AllowFileExtensions = GroupName + ".AllowFileExtensions";
    /// <summary>
    /// 生成临时下载链接有效期(s)
    /// </summary>
    public const string GenerateDownloadUrlExpirySeconds = GroupName + ".GenerateDownloadUrlExpirySeconds";

    public const long DefaultFileLimitLength = 5L;
    public const double DefaultGenerateDownloadUrlExpirySeconds = 600d;
    public const string DefaultAllowFileExtensions = "dll,zip,rar,txt,log,xml,config,json,jpeg,jpg,png,bmp,ico,xlsx,xltx,xls,xlt,docs,dots,doc,dot,pdf,pptx,potx,ppt,pot,chm";
}
