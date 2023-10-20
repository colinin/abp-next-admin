using System.Collections.Generic;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Browsers;
public class CoreUIBrowseReadComponent : List<CoreUIBrowseNode>
{
    public CoreUIBrowseReadComponent(string repository, string node = "/")
    {
        Add(new CoreUIBrowseNode(repository, node));
    }
}
