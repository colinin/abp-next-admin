using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Versions
{
    public class VersionCreateDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(AppVersionConsts.MaxTitleLength)]
        public string Title { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        [StringLength(AppVersionConsts.MaxVersionLength)] 
        public string Version { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(AppVersionConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        /// <summary>
        /// 适应平台
        /// </summary>
        public PlatformType PlatformType { get; set; } = PlatformType.None;
        /// <summary>
        /// 重要级别
        /// </summary>
        public ImportantLevel Level { get; set; } = ImportantLevel.Low;
    }
}
