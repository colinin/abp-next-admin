namespace LINGYUN.Abp.OssManagement;

public class OssObjectUrlCacheItem
{
    public string Url {  get; set; }

    public string Bucket { get; set; }

    public string Path { get; set; }

    public string Object { get; set; }
    public OssObjectUrlCacheItem()
    {

    }

    public OssObjectUrlCacheItem(string url, string bucket, string path, string @object)
    {
        Url = url;
        Bucket = bucket;
        Path = path;
        Object = @object;
    }
}
