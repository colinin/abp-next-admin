using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.CachingManagement;

public class CacheValueResponse
{
    public string Type { get; }
    public long Size { get; }
    public TimeSpan? Ttl { get; }
    public IDictionary<string, object> Values { get; }
    public CacheValueResponse(
        string type,
        long size,
        IDictionary<string, object> values,
        TimeSpan? ttl = null)
    {
        Type = type;
        Size = size;
        Ttl = ttl;
        Values = values;
    }
}
