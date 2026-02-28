using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.ApiScopes;

public class ApiScopePropertyDto : EntityDto
{
    public string Key { get; set; }

    public string Value { get; set; }
}
