using Newtonsoft.Json;
using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class GlobalConfigurationDto : GlobalConfigurationDtoBase
    {
        [JsonConverter(typeof(HexLongConverter))]
        public long ItemId { get;  set; }

        public string AppId { get; set; }
        public GlobalConfigurationDto()
        {
        }
    }
}
