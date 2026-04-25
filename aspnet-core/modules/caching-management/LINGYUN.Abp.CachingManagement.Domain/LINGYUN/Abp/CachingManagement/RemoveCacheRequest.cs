namespace LINGYUN.Abp.CachingManagement;

public class RemoveCacheRequest
{
    public string[] Keys { get; }
    public RemoveCacheRequest(string[] keys)
    {
        Keys = keys;
    }
}
