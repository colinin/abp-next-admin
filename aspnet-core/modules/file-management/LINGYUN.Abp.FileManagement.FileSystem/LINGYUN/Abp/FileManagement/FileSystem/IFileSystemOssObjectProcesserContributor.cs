using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Abp.FileManagement.FileSystem
{
    public interface IFileSystemOssObjectProcesserContributor
    {
        Task ProcessAsync(FileSystemOssObjectContext context);
    }
}
