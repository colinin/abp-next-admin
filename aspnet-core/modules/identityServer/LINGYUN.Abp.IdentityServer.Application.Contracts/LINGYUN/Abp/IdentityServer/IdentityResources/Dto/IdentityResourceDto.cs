using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

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

        public List<string> UserClaims { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        public IdentityResourceDto()
        {
            UserClaims = new List<string>();
            Properties = new Dictionary<string, string>();
        }
    }
}
