using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceCreateDto : ApiResourceCreateOrUpdateDto
    {
        [Required]
        [StringLength(ApiResourceConsts.NameMaxLength)]
        public string Name { get; set; }
    }
}
