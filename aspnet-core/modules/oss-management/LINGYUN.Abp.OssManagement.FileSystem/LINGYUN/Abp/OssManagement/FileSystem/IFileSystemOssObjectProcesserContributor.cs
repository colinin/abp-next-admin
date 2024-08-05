using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement.FileSystem;

public interface IFileSystemOssObjectProcesserContributor
{
    Task ProcessAsync(FileSystemOssObjectContext context);
}
