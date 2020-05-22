using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.IdentityResources;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        
        public string DisplayName { get; set; }

        
        public string Description { get; set; }

        public string ConcurrencyStamp { get; set; }

        public bool Enabled { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<IdentityClaimDto> UserClaims { get; set; }

        public List<IdentityResourcePropertyDto> Properties { get; set; }

        public IdentityResourceDto()
        {
            UserClaims = new List<IdentityClaimDto>();
            Properties = new List<IdentityResourcePropertyDto>();
        }
    }
}
