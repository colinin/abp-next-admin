using System;
using System.IO;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OssManagement.FileSystem;

public class FileSystemOssObjectContext : IServiceProviderAccessor
{
    public string Process { get; }
    public OssObject OssObject { get; }
    public bool Handled { get; private set; }
    public Stream Content { get; private set; }
    public IServiceProvider ServiceProvider { get; }

    public FileSystemOssObjectContext(
        string process,
        OssObject ossObject,
        IServiceProvider serviceProvider)
    {
        Process = process;
        OssObject = ossObject;
        ServiceProvider = serviceProvider;
    }

    public void SetContent(Stream content)
    {
        Content = content;
        Content.Seek(0, SeekOrigin.Begin);
        Handled = true;
    }
}
