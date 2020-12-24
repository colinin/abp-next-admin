using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.ApiScopes
{
    public class ApiScopeDto : ExtensibleAuditedEntityDto<Guid>
    {
        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<ApiScopeClaimDto> UserClaims { get; set; }

        public List<ApiScopePropertyDto> Properties { get; set; }

        public ApiScopeDto()
        {
            UserClaims = new List<ApiScopeClaimDto>();
            Properties = new List<ApiScopePropertyDto>();
        }
    }
}
