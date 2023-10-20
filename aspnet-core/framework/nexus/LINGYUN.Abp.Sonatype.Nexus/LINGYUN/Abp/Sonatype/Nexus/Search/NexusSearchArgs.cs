namespace LINGYUN.Abp.Sonatype.Nexus.Search;
public class NexusSearchArgs
{
    public string Keyword { get; }
    public string Repository { get; }
    public string Group { get; }
    public string Name { get; }
    public string Format { get; set; } = "raw";
    public int? Timeout { get; set; }
    public string Version { get; }

    public NexusSearchArgs(
        string repository,
        string group,
        string name,
        string format = "raw",
        string keyword = null,
        string version = null,
        int? timeout = null)
    {
        Keyword = keyword;
        Repository = repository;
        Group = group;
        Name = name;
        Format = format;
        Timeout = timeout;
        Version = version;
    }
}
