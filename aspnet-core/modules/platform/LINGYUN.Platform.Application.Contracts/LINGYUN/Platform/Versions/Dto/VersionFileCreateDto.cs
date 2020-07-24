using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Versions
{
    public class VersionFileCreateDto
    {
        [Required]
        public Guid VersionId { get; set; }

        [Required]
        [StringLength(AppVersionConsts.MaxVersionLength)]
        public string Version { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [StringLength(VersionFileConsts.MaxPathLength)] 
        public string FilePath { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        [Required] 
        [StringLength(VersionFileConsts.MaxNameLength)]
        public string FileName { get; set; }
        /// <summary>
        /// 文件版本
        /// </summary>
        [Required]
        [StringLength(VersionFileConsts.MaxVersionLength)] 
        public string FileVersion { get; set; }
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
        /// 文件类型
        /// </summary>
        public FileType FileType { get; set; }
        /// <summary>
        /// 文件指纹
        /// </summary>
        public string SHA256 { get; set; }
    }
}
