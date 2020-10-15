using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity
{
    public class IdentityRoleClaimCreateDto
    {
        [Required]
        [DynamicMaxLength(typeof(IdentityRoleClaimConsts), nameof(IdentityRoleClaimConsts.MaxClaimTypeLength))]
        public string ClaimType { get; set; }

        [Required]
        [DynamicMaxLength(typeof(IdentityRoleClaimConsts), nameof(IdentityRoleClaimConsts.MaxClaimValueLength))]
        public string ClaimValue { get; set; }
    }
}
