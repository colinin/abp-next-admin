using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobContentTypeResolver
{
    Task<string> ResolveContentTypeAsync(string blobName, Stream blobStream);
}
