using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public List<ApiSecretDto> Secrets { get; set; }

        public List<ApiScopeDto> Scopes { get; set; }

        public List<ApiResourceClaimDto> UserClaims { get; set; }

        public ApiResourceDto()
        {
            Scopes = new List<ApiScopeDto>();
            Secrets = new List<ApiSecretDto>();
            UserClaims = new List<ApiResourceClaimDto>();
        }
    }
}
