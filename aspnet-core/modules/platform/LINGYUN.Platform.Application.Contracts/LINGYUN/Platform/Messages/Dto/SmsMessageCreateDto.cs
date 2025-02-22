using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Messages;
public class SmsMessageCreateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(MessageConsts), nameof(MessageConsts.MaxReceiverLength))]
    public string PhoneNumber { get; set; }

    [Required]
    public string Text { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public SmsMessageCreateDto()
    {

    }

    public SmsMessageCreateDto(
        [NotNull] string phoneNumber,
        [NotNull] string text)
    {
        PhoneNumber = phoneNumber;
        Text = text;
    }
}
