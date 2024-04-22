using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantCreateDto : TenantCreateOrUpdateBase
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

    /// <summary>
    /// 其他数据库连接
    /// </summary>
    public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();
}