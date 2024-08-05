using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement.FileSystem;

public static class FileSystemOssOptionsExtensions
{
    public static void AddProcesser<TProcesserContributor>(
        this FileSystemOssOptions options,
        TProcesserContributor contributor)
        where TProcesserContributor : IFileSystemOssObjectProcesserContributor
    {
        options.Processers.InsertBefore((x) => x is NoneFileSystemOssObjectProcesser, contributor);
    }
}
