using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public abstract class BlobFileCreateBaseDto
{
    /// <summary>
    /// 用于服务端验证上传文件
    /// </summary>
    public string? CompareMd5 { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(BlobConsts), nameof(BlobConsts.MaxNameLength))]
    public string Name { get; set; }
    /// <summary>
    /// 所属BlobId
    /// </summary>
    public Guid? ParentId { get; set; }
    /// <summary>
    /// 文件内容
    /// </summary>
    [DisableAuditing]
    [DisableValidation]
    public IRemoteStreamContent File { get; set; }
}
