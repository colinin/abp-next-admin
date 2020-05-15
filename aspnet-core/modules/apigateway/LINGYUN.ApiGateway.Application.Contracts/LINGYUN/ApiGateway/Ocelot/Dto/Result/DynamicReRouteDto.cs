using Newtonsoft.Json;
using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class DynamicReRouteDto : DynamicReRouteDtoBase
    {
        [JsonConverter(typeof(HexLongConverter))]
        public virtual long DynamicReRouteId { get; set; }
    }
}
