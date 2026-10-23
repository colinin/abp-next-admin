using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceStartDto
{
    [Required]
    public Guid TemplateId { get; set; }

    [Required]
    [DynamicStringLength(typeof(ApprovalInstanceConsts), nameof(ApprovalInstanceConsts.MaxTitleLength))]
    public string Title { get; set; } = "";

    [DynamicStringLength(typeof(ApprovalInstanceConsts), nameof(ApprovalInstanceConsts.MaxBusinessKeyLength))]
    public string? BusinessKey { get; set; }

    public string? FormDataJson { get; set; }
}
