namespace LINGYUN.Abp.Cli.Blob;

public class CurrentBlob : ICurrentBlob
{
    public string BaseUrl {  get; set; }

    public string TenantId { get; set; }
}
