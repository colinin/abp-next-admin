using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Repositories.Raw;

[Serializable]
public class RawGroup
{
    [JsonPropertyName("memberNames")]
    public List<string> MemberNames { get; set; }

    public RawGroup()
    {
        MemberNames = new List<string>();
    }
}
