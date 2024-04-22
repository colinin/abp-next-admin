namespace LINGYUN.Abp.CachingManagement;

public class GetCacheKeysInput
{
    public string Prefix { get; set; }
    public string Marker { get; set; }
    public string Filter { get; set; }
}
