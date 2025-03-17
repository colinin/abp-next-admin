using System.Collections.Generic;

namespace LINGYUN.Abp.IP.Location;
public class IPLocationResolveResult
{
    public IPLocation? Location { get; set; }

    public List<string> AppliedResolvers { get; }

    public IPLocationResolveResult()
    {
        AppliedResolvers = new List<string>();
    }
}
