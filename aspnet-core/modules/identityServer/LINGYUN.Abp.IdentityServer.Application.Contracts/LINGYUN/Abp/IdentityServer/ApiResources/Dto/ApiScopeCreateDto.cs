using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiScopeCreateDto
    {
        [Required]
        public Guid ApiResourceId { get; set; }

        [Required]
        [StringLength(ApiScopeConsts.NameMaxLength)]
        public string Name { get; set; }

        [StringLength(ApiScopeConsts.DisplayNameMaxLength)]
        public string DisplayName { get; set; }

        [StringLength(ApiScopeConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> UserClaims { get; set; }

        public ApiScopeCreateDto()
        {
            UserClaims = new List<string>();
        }
    }
}
