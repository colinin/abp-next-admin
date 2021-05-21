using Ocelot.Configuration.File;
using System.Collections.Generic;

namespace LINGYUN.Abp.ApiGateway
{
    public class AbpApiGatewayOptions
    {
        public FileGlobalConfiguration GlobalConfiguration { get; set; } = new FileGlobalConfiguration();
        public List<string> AggrageRouteUrls { get; set; } = new List<string>();
    }
}
