using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientRedirectUriDto : EntityDto
    {
        public string RedirectUri { get; set; }
    }
}
