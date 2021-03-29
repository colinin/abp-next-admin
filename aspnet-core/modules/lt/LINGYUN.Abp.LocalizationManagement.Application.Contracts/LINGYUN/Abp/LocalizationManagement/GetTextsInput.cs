using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class GetTextsInput : PagedAndSortedResultRequestDto
    {
        [Required]
        [DynamicStringLength(typeof(LanguageConsts), nameof(LanguageConsts.MaxCultureNameLength))]
        public string CultureName { get; set; }

        [Required]
        [DynamicStringLength(typeof(LanguageConsts), nameof(LanguageConsts.MaxCultureNameLength))]
        public string TargetCultureName { get; set; }

        [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxNameLength))]
        public string ResourceName { get; set; }

        public bool? OnlyNull { get; set; }

        public string Filter { get; set; }
    }
}
