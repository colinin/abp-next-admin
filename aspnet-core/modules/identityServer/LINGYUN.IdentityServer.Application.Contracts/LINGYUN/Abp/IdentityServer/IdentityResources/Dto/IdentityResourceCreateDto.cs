using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.IdentityResources;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceCreateDto
    {
        [Required]
        [StringLength(IdentityResourceConsts.NameMaxLength)]
        public string Name { get; set; }

        [StringLength(IdentityResourceConsts.DisplayNameMaxLength)]
        public string DisplayName { get; set; }

        [StringLength(IdentityResourceConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<IdentityClaimDto> UserClaims { get; set; }

        public IdentityResourceCreateDto()
        {
            Enabled = true;
            Required = false;
            ShowInDiscoveryDocument = false;
            UserClaims = new List<IdentityClaimDto>();
        }
    }
}
