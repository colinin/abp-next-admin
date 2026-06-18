using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobContentTypeResolveContributor
{
    string Name { get; }
    int Priority { get; }
    Task ResolveAsync(BlobContentTypeResolveContext context);
}
