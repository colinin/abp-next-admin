using System.Collections.Generic;

namespace LINGYUN.Platform.Versions
{
    public class VersionCreateDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 重要级别
        /// </summary>
        public ImportantLevel Level { get; set; } = ImportantLevel.Low;

        public List<VersionFileDto> VersionFiles { get; set; } = new List<VersionFileDto>();
    }
}
