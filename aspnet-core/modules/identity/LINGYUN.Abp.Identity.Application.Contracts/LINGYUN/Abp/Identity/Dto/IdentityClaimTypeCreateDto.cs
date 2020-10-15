using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity
{
    public class IdentityClaimTypeCreateDto : IdentityClaimTypeCreateOrUpdateBaseDto
    {
        [Required]
        [DynamicStringLength(typeof(IdentityClaimTypeConsts), nameof(IdentityClaimTypeConsts.MaxNameLength))]
        public string Name { get; set; }

        public bool IsStatic { get; set; }

        public IdentityClaimValueType ValueType { get; set; }
    }
}
