using System.Collections.Generic;

namespace LINGYUN.Abp.AspNetCore.SignalR.JwtToken
{
    public class AbpAspNetCoreSignalRJwtTokenMapPathOptions
    {
        public string PrefixHubPath { get; set; }
        public List<string> MapJwtTokenPaths { get; set; }
        public AbpAspNetCoreSignalRJwtTokenMapPathOptions()
        {
            PrefixHubPath = "/signalr-hubs/";
            MapJwtTokenPaths = new List<string>();
        }

        public void MapPath(string path)
        {
            if (path.StartsWith(PrefixHubPath))
            {
                MapJwtTokenPaths.AddIfNotContains(path);
            }
            else
            {
                MapJwtTokenPaths.AddIfNotContains($"{PrefixHubPath}{path}");
            }
        }
    }
}
