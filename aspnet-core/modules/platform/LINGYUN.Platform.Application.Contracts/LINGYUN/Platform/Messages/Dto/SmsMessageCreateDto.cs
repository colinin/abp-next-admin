using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Messages;
public class SmsMessageCreateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(MessageConsts), nameof(MessageConsts.MaxReceiverLength))]
    public string PhoneNumber { get; set; } = default!;

    [Required]
    public string Text { get; set; } = default!;

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public SmsMessageCreateDto()
    {
        ExtraProperties = new ExtraPropertyDictionary();
    }

    public SmsMessageCreateDto(
        [NotNull] string phoneNumber,
        [NotNull] string text)
    {
        PhoneNumber = phoneNumber;
        Text = text;

        ExtraProperties = new ExtraPropertyDictionary();
    }
}
