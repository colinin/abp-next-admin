namespace LINGYUN.Platform.Packages;

public class PackageFilter
{
    public string Filter { get; set; }

    public string Name { get; set; }

    public string Note { get; set; }

    public string Version { get; set; }

    public string Description { get; set; }

    public bool? ForceUpdate { get; set; }

    public string Authors { get; set; }

    public PackageFilter(
        string filter = null,
        string name = null, 
        string note = null,
        string version = null, 
        string description = null,
        bool? forceUpdate = null, 
        string authors = null)
    {
        Filter = filter;
        Name = name;
        Note = note;
        Version = version;
        Description = description;
        ForceUpdate = forceUpdate;
        Authors = authors;
    }
}
