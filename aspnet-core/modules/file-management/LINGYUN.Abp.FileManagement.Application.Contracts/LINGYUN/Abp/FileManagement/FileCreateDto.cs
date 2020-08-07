using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.FileManagement
{
    public class FileCreateDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [StringLength(255)]
        public string Path { get; set; }
        /// <summary>
        /// 文件数据，前端无需传递此参数,由控制器传递
        /// </summary>
        [DisableAuditing]
        public byte[] Data { get; set; }
        /// <summary>
        /// 当前字节数
        /// </summary>
        [Required]
        public int CurrentByte { get; set; }
        /// <summary>
        /// 最大字节数
        /// </summary>
        [Required]
        public int TotalByte { get; set; }
        /// <summary>
        /// 是否覆盖文件
        /// </summary>
        public bool Rewrite { get; set; } = false;
    }
}
