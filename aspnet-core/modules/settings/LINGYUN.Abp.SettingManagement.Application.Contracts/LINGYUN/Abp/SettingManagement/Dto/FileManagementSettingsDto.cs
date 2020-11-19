namespace LINGYUN.Abp.SettingManagement
{
    public class FileManagementSettingsDto
    {
        /// <summary>
        /// 限制上传文件大小
        /// </summary>
        public SettingDetailsDto FileLimitLength { get; set; }
        /// <summary>
        /// 允许的文件扩展名
        /// </summary>
        public SettingDetailsDto AllowFileExtensions { get; set; }
    }
}
