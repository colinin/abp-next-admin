using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement
{
    public interface IFileUploader
    {
        Task UploadAsync(UploadFileChunkInput input, CancellationToken cancellationToken = default);
    }
}
