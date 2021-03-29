using Volo.Abp.Validation;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class CreateOrUpdateTextInput
    {
        [DynamicStringLength(typeof(TextConsts), nameof(TextConsts.MaxValueLength))]
        public string Value { get; set; }
    }
}
