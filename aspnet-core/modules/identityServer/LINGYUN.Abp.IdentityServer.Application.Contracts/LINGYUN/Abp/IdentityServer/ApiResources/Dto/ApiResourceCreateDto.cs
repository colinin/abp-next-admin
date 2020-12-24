using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceCreateDto : ApiResourceCreateOrUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.NameMaxLength))]
        public string Name { get; set; }
    }
}
