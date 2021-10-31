using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantCreateDto : TenantCreateOrUpdateDtoBase
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string AdminEmailAddress { get; set; }

        [Required]
        [MaxLength(128)]
        public string AdminPassword { get; set; }

        /// <summary>
        /// 使用共享数据库
        /// </summary>
        public bool UseSharedDatabase { get; set; } = true;

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public string DefaultConnectionString { get; set; }
    }
}