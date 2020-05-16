using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientScopeDto : EntityDto
    {
        public string Scope { get; set; }
    }
}
