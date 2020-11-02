using System.Collections.Generic;

namespace LINGYUN.Abp.AspNetCore.SignalR.JwtToken
{
    public class AbpAspNetCoreSignalRJwtTokenMapPathOptions
    {
        public List<string> MapJwtTokenPaths { get; set; }
        public AbpAspNetCoreSignalRJwtTokenMapPathOptions()
        {
            MapJwtTokenPaths = new List<string>();
        }

        public void MapPath(string path)
        {
            if (path.StartsWith("/signalr-hubs/"))
            {
                MapJwtTokenPaths.AddIfNotContains(path);
            }
            else
            {
                MapJwtTokenPaths.AddIfNotContains($"/signalr-hubs/{path}");
            }
        }
    }
}
