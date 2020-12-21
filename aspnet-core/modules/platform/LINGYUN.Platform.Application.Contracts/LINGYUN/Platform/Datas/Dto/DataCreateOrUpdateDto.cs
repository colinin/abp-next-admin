using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Datas
{
    public class DataCreateOrUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(DataConsts), nameof(DataConsts.MaxNameLength))]
        public string Name { get; set; }

        [Required]
        [DynamicStringLength(typeof(DataConsts), nameof(DataConsts.MaxDisplayNameLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(DataConsts), nameof(DataConsts.MaxDescriptionLength))]
        public string Description { get; set; }
    }
}
