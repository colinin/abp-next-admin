using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    public class GetTextsInput
    {
        [Required]
        public string CultureName { get; set; }

        [Required]
        public string TargetCultureName { get; set; }

        public string ResourceName { get; set; }

        public bool? OnlyNull { get; set; }

        public string Filter { get; set; }
    }
}
