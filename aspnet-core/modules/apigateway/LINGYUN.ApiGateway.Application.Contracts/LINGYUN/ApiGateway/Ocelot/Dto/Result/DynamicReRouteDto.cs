using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class DynamicReRouteDto : DynamicReRouteDtoBase
    {
        public virtual string DynamicReRouteId { get; set; }
    }
}
