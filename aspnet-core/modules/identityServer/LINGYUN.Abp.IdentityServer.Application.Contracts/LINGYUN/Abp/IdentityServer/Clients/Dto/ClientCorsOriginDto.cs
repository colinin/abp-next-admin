using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientCorsOriginDto : EntityDto
    {
        public string Origin { get; set; }
    }
}
