using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobGetPagedListInput : BlobGetPagedListInputBase
{
    /// <summary>
    /// 容器Id
    /// </summary>
    [Required]
    public Guid ContainerId { get; set; }
}
