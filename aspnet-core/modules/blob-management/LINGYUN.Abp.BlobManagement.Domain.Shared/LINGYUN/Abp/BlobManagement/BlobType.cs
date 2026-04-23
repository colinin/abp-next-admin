using System.ComponentModel;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 对象类型
/// </summary>
[Description("对象类型")]
public enum BlobType
{
    /// <summary>
    /// 目录
    /// </summary>
    [Description("目录")]
    Folder = 0,
    /// <summary>
    /// 文件
    /// </summary>
    [Description("文件")]
    File = 1
}
