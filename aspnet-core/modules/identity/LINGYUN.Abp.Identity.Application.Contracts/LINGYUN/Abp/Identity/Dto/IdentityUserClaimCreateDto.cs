using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity
{
    public class IdentityUserClaimCreateDto
    {
        [Required]
        [DynamicMaxLength(typeof(IdentityUserClaimConsts), nameof(IdentityUserClaimConsts.MaxClaimTypeLength))]
        public string ClaimType { get; set; }

        [Required]
        [DynamicMaxLength(typeof(IdentityUserClaimConsts), nameof(IdentityUserClaimConsts.MaxClaimValueLength))]
        public string ClaimValue { get; set; }
    }
}
