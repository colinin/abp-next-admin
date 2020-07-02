using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string ConcurrencyStamp { get; set; }

        [DynamicStringLength(typeof(IdentityResourceConsts), nameof(IdentityResourceConsts.NameMaxLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityResourceConsts), nameof(IdentityResourceConsts.DisplayNameMaxLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(IdentityResourceConsts), nameof(IdentityResourceConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<IdentityClaimDto> UserClaims { get; set; }

        public IdentityResourceUpdateDto()
        {
            Enabled = true;
            Required = false;
            ShowInDiscoveryDocument = false;
            UserClaims = new List<IdentityClaimDto>();
        }
    }
}
