using JetBrains.Annotations;
using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement.FileSystem;

public class FileSystemOssOptions
{
    [NotNull]
    public List<IFileSystemOssObjectProcesserContributor> Processers { get; }

    public FileSystemOssOptions()
    {
        Processers = new List<IFileSystemOssObjectProcesserContributor>
        {
            new NoneFileSystemOssObjectProcesser()
        };
    }
}
