using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Abp.FileManagement.FileSystem
{
    public class NoneFileSystemOssObjectProcesser : IFileSystemOssObjectProcesserContributor
    {
        public Task ProcessAsync(FileSystemOssObjectContext context)
        {
            context.SetContent(context.OssObject.Content);

            return Task.CompletedTask;
        }
    }
}
