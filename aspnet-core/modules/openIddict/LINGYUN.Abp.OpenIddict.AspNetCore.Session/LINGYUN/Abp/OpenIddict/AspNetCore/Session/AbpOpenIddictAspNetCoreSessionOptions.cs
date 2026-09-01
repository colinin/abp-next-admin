using OpenIddict.Server;
using System.Collections.Generic;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
public class AbpOpenIddictAspNetCoreSessionOptions
{
    public List<string> PersistentSessionGrantTypes { get; set; }
    public List<OpenIddictServerEndpointType> ValidationSessionEndpointTypes { get; set; }
    public AbpOpenIddictAspNetCoreSessionOptions()
    {
        PersistentSessionGrantTypes = new List<string>();
        ValidationSessionEndpointTypes = new List<OpenIddictServerEndpointType>();
    }
}
