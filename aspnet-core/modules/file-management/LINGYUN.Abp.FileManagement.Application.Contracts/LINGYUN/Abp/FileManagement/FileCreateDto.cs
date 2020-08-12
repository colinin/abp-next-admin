using System.ComponentModel.DataAnnotations;
using System.IO;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FileManagement
{
    public class FileCreateDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [StringLength(255)]
        public string Path { get; set; }
        /// <summary>
        /// 文件数据，前端无需传递此参数,由控制器传递
        /// </summary>
        [DisableAuditing]
        [DisableValidation]// TODO: 需要禁用参数检查,否则会有一个框架方面的性能问题存在
        public byte[] Data { get; set; }
        /// <summary>
        /// 是否覆盖文件
        /// </summary>
        public bool Rewrite { get; set; } = false;
    }
}
