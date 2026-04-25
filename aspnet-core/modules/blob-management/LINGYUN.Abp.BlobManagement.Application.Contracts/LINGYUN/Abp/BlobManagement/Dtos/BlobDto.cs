using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobDto : ExtensibleAuditedEntityDto<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 全名
    /// </summary>
    public string FullName { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public BlobType Type { get; set; }
    /// <summary>
    /// 内容类型
    /// </summary>
    public string? ContentType { get; set; }
    /// <summary>
    /// 文件大小
    /// </summary>
    public long Size { get; set; }
    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime? ExpirationTime { get; set; }
    /// <summary>
    /// 下载次数
    /// </summary>
    public long DownloadCount { get; set; }
}
