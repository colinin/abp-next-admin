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

        public List<string> UserClaims { get; set; }

        public ApiResourceDto()
        {
            UserClaims = new List<string>();
            Scopes = new List<ApiScopeDto>();
            Secrets = new List<ApiSecretDto>();
        }
    }
}
