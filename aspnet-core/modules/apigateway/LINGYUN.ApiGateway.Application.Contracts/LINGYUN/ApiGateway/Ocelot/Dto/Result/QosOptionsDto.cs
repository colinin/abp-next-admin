using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class QosOptionsDto
    {
        public int? ExceptionsAllowedBeforeBreaking { get; set; }

        public int? DurationOfBreak { get; set; }

        public int? TimeoutValue { get; set; }
    }
}
