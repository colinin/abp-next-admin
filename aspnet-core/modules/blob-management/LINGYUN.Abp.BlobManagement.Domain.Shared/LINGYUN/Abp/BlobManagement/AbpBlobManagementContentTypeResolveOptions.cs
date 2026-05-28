using JetBrains.Annotations;
using System.Collections.Generic;

namespace LINGYUN.Abp.BlobManagement;

public class AbpBlobManagementContentTypeResolveOptions
{
    [NotNull]
    public List<IBlobContentTypeResolveContributor> BlobContentTypeResolvers { get; }

    public AbpBlobManagementContentTypeResolveOptions()
    {
        BlobContentTypeResolvers = new List<IBlobContentTypeResolveContributor>();
    }
}
