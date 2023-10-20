using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Browsers;

[Serializable]
public class CoreUIBrowseNode
{
    [JsonPropertyName("node")]
    public string Node { get; set; }

    [JsonPropertyName("repositoryName")]
    public string RepositoryName { get; set; }
    public CoreUIBrowseNode(string repositoryName, string node = "/")
    {
        Node = node;
        RepositoryName = repositoryName;
    }
}
