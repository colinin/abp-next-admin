using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement.FileSystem;

public class NoneFileSystemOssObjectProcesser : IFileSystemOssObjectProcesserContributor
{
    public Task ProcessAsync(FileSystemOssObjectContext context)
    {
        context.SetContent(context.OssObject.Content);

        return Task.CompletedTask;
    }
}
