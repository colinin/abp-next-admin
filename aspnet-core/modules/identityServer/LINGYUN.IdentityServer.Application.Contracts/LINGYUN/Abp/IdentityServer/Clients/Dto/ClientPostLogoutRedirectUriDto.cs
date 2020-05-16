using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientPostLogoutRedirectUriDto : EntityDto
    {
        public string PostLogoutRedirectUri { get; set; }
    }
}
