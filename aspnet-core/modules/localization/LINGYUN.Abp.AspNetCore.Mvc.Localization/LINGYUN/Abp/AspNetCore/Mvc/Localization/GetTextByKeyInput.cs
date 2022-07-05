using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    public class GetTextByKeyInput
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string CultureName { get; set; }

        [Required]
        public string ResourceName { get; set; }
    }
}
