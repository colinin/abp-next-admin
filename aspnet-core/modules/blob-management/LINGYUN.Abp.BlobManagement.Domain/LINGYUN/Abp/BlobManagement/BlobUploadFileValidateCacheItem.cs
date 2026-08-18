namespace LINGYUN.Abp.BlobManagement;

public class BlobUploadFileValidateCacheItem
{
    public const string CacheKey = "Abp.BlobManagement.FileValidation";
    public long SizeLimit { get; set; }
    public string[] AllowedExtensions { get; set; }
    public BlobUploadFileValidateCacheItem()
    {
        AllowedExtensions = new string[0];
    }

    public BlobUploadFileValidateCacheItem(
        long sizeLimit,
        string[] allowedExtensions)
    {
        SizeLimit = sizeLimit;
        AllowedExtensions = allowedExtensions;
    }
}
