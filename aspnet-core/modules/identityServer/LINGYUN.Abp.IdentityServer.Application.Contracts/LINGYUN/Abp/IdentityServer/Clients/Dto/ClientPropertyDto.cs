using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientPropertyDto : EntityDto
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
