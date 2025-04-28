using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.OssManagement;

public class ValidationOssObjectInput
{
    /// <summary>
    /// 总文件大小
    /// </summary>
    [Required]
    public long TotalSize { get; set; }
    /// <summary>
    /// 文件名
    /// </summary>
    [Required]
    public string FileName { get; set; }
    /// <summary>
    /// 容器
    /// </summary>
    [Required]
    public string Bucket { get; set; }
    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }

    public bool Overwrite { get; set; }
}
