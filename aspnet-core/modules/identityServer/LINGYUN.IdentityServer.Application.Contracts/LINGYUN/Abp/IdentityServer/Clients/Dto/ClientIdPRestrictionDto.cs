using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientIdPRestrictionDto : EntityDto
    {
        public string Provider { get; set; }
    }
}
