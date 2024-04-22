namespace LINGYUN.Abp.CachingManagement;

public class GetCacheKeysRequest
{
    public string Prefix { get; }
    public string Filter { get; }
    public string Marker { get; }

    public GetCacheKeysRequest(
            string prefix = null,
            string filter = null,
            string marker = null)
    {
        Prefix = prefix;
        Filter = filter;
        Marker = marker;
    }
}
