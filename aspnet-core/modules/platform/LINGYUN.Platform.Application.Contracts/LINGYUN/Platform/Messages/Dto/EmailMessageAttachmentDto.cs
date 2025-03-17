using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Messages;
public class EmailMessageAttachmentDto
{
    [Required]
    [DynamicStringLength(typeof(EmailMessageAttachmentConsts), nameof(EmailMessageAttachmentConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicStringLength(typeof(EmailMessageAttachmentConsts), nameof(EmailMessageAttachmentConsts.MaxNameLength))]
    public string BlobName { get; set; }

    public long Size { get; set; }
}
