using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.ApiScopes;

public class ApiScopeCreateDto : ApiScopeCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(ApiScopeConsts), nameof(ApiScopeConsts.NameMaxLength))]
    public string Name { get; set; } = default!;
}
