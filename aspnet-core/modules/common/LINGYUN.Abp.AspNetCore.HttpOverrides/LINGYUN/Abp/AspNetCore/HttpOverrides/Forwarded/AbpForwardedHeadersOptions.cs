using Microsoft.AspNetCore.HttpOverrides;
using System.Collections.Generic;

namespace LINGYUN.Abp.AspNetCore.HttpOverrides.Forwarded
{
    public class AbpForwardedHeadersOptions
    {
        public string ForwardedForHeaderName { get; set; }
        public string ForwardedHostHeaderName { get; set; }
        public string ForwardedProtoHeaderName { get; set; }
        public string OriginalForHeaderName { get; set; }
        public string OriginalHostHeaderName { get; set; }
        public string OriginalProtoHeaderName { get; set; }
        public ForwardedHeaders ForwardedHeaders { get; set; }
        public int? ForwardLimit { get; set; }
        public bool RequireHeaderSymmetry { get; set; }
        public List<string> AllowedHosts { get; set; }
        public List<string> KnownNetworks { get; set; }
        public List<string> KnownProxies { get; set; }
        public AbpForwardedHeadersOptions()
        {
            ForwardedForHeaderName = ForwardedHeadersDefaults.XForwardedForHeaderName;
            ForwardedHostHeaderName = ForwardedHeadersDefaults.XForwardedHostHeaderName;
            ForwardedProtoHeaderName = ForwardedHeadersDefaults.XForwardedProtoHeaderName;
            OriginalForHeaderName = ForwardedHeadersDefaults.XOriginalForHeaderName;
            OriginalHostHeaderName = ForwardedHeadersDefaults.XOriginalHostHeaderName;
            OriginalProtoHeaderName = ForwardedHeadersDefaults.XOriginalProtoHeaderName;
            ForwardedHeaders = ForwardedHeaders.None;
            ForwardLimit = 1;
            RequireHeaderSymmetry = false;
            AllowedHosts = new List<string>();
            KnownProxies = new List<string>();
            KnownNetworks = new List<string>();
        }
    }
}
