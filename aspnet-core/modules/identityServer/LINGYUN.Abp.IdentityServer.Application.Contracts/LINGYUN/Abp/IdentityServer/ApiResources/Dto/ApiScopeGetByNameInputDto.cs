using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiScopeGetByNameInputDto
    {
        [Required]
        public Guid ApiResourceId { get; set; }

        [Required]
        [StringLength(ApiScopeConsts.NameMaxLength)]
        public string Name { get; set; }
    }
}
