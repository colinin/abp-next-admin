using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public abstract class BlobGetPagedListInputBase : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 上级BlobId
    /// </summary>
    public Guid? ParentId { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public BlobType? Type { get; set; }
    /// <summary>
    /// 内容类型
    /// </summary>
    public string? ContentType { get; set; }
}
