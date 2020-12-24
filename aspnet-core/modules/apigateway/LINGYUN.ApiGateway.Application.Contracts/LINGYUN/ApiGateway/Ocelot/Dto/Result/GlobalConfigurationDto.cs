using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class GlobalConfigurationDto : GlobalConfigurationDtoBase
    {
        public string ItemId { get;  set; }

        public string AppId { get; set; }
        public GlobalConfigurationDto()
        {
        }
    }
}
