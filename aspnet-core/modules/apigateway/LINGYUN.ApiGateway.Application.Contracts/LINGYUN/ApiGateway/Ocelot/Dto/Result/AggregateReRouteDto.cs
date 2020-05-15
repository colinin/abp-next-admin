using Newtonsoft.Json;
using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class AggregateReRouteDto : AggregateReRouteDtoBase
    {
        [JsonConverter(typeof(HexLongConverter))]
        public long ReRouteId { get; set; }

        public AggregateReRouteDto()
        {
        }
    }
}
