namespace LINGYUN.Abp.Cli.Blob;

public interface ICurrentBlob
{
    string BaseUrl { get; set; }

    string TenantId { get; set; }
}
