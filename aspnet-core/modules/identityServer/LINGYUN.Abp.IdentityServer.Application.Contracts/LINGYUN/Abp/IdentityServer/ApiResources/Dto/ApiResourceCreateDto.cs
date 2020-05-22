using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceCreateDto
    {
        [Required]
        [StringLength(ApiResourceConsts.NameMaxLength)]
        public string Name { get; set; }

        [StringLength(ApiResourceConsts.DisplayNameMaxLength)]
        public string DisplayName { get; set; }

        [StringLength(ApiResourceConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public List<ApiResourceClaimDto> UserClaims { get; set; }
        public ApiResourceCreateDto()
        {
            Enabled = true;
            UserClaims = new List<ApiResourceClaimDto>();
        }
    }
}
