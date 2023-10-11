using System.Net.Http;

namespace LINGYUN.Abp.Sonatype.Nexus.Components;
public abstract class NexusComponentUploadArgs
{
    public string Repository { get; }
    public string Directory { get; }

    protected NexusComponentUploadArgs(string repository, string directory)
    {
        Repository = repository;
        Directory = directory;
    }

    public abstract HttpContent BuildContent();
}
