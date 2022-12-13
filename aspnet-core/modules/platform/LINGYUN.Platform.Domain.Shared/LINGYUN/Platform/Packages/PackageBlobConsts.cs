namespace LINGYUN.Platform.Packages;
public static class PackageBlobConsts
{
    public static int MaxNameLength { get; set; } = 255;
    public static int MaxUrlLength { get; set; } = 512;
    public static int MaxContentTypeLength { get; set; } = 256;
    public static int MaxSummaryLength { get; set; } = 1024;
    public static int MaxLicenseLength { get; set; } = 1024;
    public static int MaxAuthorsLength { get; set; } = 100;
    public static int MaxSHA256Length { get; set; } = 256;
}
