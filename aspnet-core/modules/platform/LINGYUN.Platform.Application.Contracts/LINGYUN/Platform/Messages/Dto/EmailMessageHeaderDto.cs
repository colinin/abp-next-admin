using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Messages;
public class EmailMessageHeaderDto
{
    [Required]
    [DynamicStringLength(typeof(EmailMessageHeaderConsts), nameof(EmailMessageHeaderConsts.MaxKeyLength))]
    public string Key { get; set; } = default!;

    [Required]
    [DynamicStringLength(typeof(EmailMessageHeaderConsts), nameof(EmailMessageHeaderConsts.MaxValueLength))]
    public string Value { get; set; } = default!;
    public EmailMessageHeaderDto()
    {

    }

    public EmailMessageHeaderDto(
        [NotNull] string key,
        [NotNull] string value)
    {
        Key = key;
        Value = value;
    }
}
