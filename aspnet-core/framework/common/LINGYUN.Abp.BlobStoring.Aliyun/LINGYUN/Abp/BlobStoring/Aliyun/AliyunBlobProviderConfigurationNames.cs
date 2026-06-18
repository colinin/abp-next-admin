namespace LINGYUN.Abp.BlobStoring.Aliyun;

public static class AliyunBlobProviderConfigurationNames
{
    /// <summary>
    /// 数据中心
    /// </summary>
    public const string Endpoint = "Aliyun:OSS:Endpoint";
    /// <summary>
    /// 命名空间
    /// </summary>
    public const string BucketName = "Aliyun:OSS:BucketName";
    /// <summary>
    /// 签名版本（可选项：v1、v4）
    /// </summary>
    public const string SignatureVersion = "Aliyun:OSS:SignatureVersion";
    /// <summary>
    /// 跳过服务器证书验证
    /// </summary>
    public const string InsecureSkipVerify = "Aliyun:OSS:InsecureSkipVerify";
    /// <summary>
    /// 命名空间不存在是否创建
    /// </summary>
    public const string CreateBucketIfNotExists = "Aliyun:OSS:CreateBucketIfNotExists";
    /// <summary>
    /// 创建命名空间时防盗链列表
    /// </summary>
    public const string CreateBucketReferer = "Aliyun:OSS:CreateBucketReferer";
    /// <summary>
    /// 创建命名空间时的Acl
    /// </summary>
    public const string CreateBucketAcl = "Aliyun:OSS:CreateBucketAcl"; 
    /// <summary>
    /// 生成预签名Uri的过期时间(s)
    /// </summary>
    public const string PresignedGetExpirySeconds = "Aliyun:OSS:PresignedGetExpirySeconds";

    public const string UsePathStyle = "Aliyun:OSS:UsePathStyle";
    public const string UseCName = "Aliyun:OSS:UseCName";
    public const string UseDualStackEndpoint = "Aliyun:OSS:UseDualStackEndpoint";
    public const string UseAccelerateEndpoint = "Aliyun:OSS:UseAccelerateEndpoint";
    public const string UseInternalEndpoint = "Aliyun:OSS:UseInternalEndpoint";
    public const string DisableUploadCrc64Check = "Aliyun:OSS:DisableUploadCrc64Check";
    public const string DisableDownloadCrc64Check = "Aliyun:OSS:DisableDownloadCrc64Check";
}
