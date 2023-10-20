using System.Collections.Generic;

namespace LINGYUN.Abp.CachingManagement;

public class CacheKeysDto
{
    public string NextMarker { get; set; }

    public List<string> Keys { get; set; } = new List<string>();
}
