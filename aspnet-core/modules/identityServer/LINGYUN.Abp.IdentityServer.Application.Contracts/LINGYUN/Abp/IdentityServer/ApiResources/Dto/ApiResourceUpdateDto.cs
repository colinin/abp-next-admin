using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.IdentityServer.ApiResources;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [StringLength(ApiResourceConsts.DisplayNameMaxLength)]
        public string DisplayName { get; set; }

        [StringLength(ApiResourceConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public List<ApiResourceClaimDto> UserClaims { get; set; }

        public ApiResourceUpdateDto()
        {
            UserClaims = new List<ApiResourceClaimDto>();
        }
    }
}
