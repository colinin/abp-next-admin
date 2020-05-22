using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientGrantTypeDto : EntityDto
    {
        public string GrantType { get; set; }
    }
}
