using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class CreateOrUpdateResourceInput
    {
        public bool Enable { get; set; }

        [Required]
        [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxDisplayNameLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxDescriptionLength))]
        public string Description { get; set; }
    }
}
