namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// Blob 下载密钥生成器
/// </summary>
/// <remarks>
/// 用于生成预签名Url的替代密钥, 有效期与签名密钥由 <see cref="AbpBlobManagementOptions"/> 决定
/// </remarks>
public interface IBlobDownloadKeyGenerator
{
    /// <summary>
    /// 生成下载密钥
    /// </summary>
    /// <param name="blob">Blob实体</param>
    string Generate(Blob blob);

    /// <summary>
    /// 解析并校验下载密钥
    /// </summary>
    /// <param name="key">下载密钥</param>
    /// <returns>密钥无效、被篡改或已过期时返回null</returns>
    BlobDownloadKey? Parse(string key);
}
