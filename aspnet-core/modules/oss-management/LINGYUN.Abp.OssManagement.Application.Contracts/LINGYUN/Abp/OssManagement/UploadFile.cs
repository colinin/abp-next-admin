using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.OssManagement
{
    public class UploadFile
    {
        /// <summary>
        /// 总文件大小
        /// </summary>
        [Required]
        public long TotalSize { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [Required]
        public string FileName { get; set; }
    }
}
