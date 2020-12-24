using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceScopeCreateDto
    {
        [Required]
        public Guid ApiResourceId { get; set; }

        [Required]
        [DynamicStringLength(typeof(ApiScopeConsts), nameof(ApiScopeConsts.NameMaxLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(ApiScopeConsts), nameof(ApiScopeConsts.DisplayNameMaxLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(ApiScopeConsts), nameof(ApiScopeConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> UserClaims { get; set; }

        public ApiResourceScopeCreateDto()
        {
            UserClaims = new List<string>();
        }
    }
}
