using System;
using System.Collections.Generic;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class AggregateReRouteDto : AggregateReRouteDtoBase
    {
        public string AppId { get; set; }

        public string ReRouteId { get; set; }

        public string Name { get; set; }

        public string ConcurrencyStamp { get; set; }

        public List<AggregateReRouteConfigDto> ReRouteKeysConfig { get; set; }

        public AggregateReRouteDto()
        {
            ReRouteKeysConfig = new List<AggregateReRouteConfigDto>();
        }
    }
}
