using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement;

/// <summary>
/// 描述了一个容器的状态信息
/// </summary>
public class OssContainer
{
    public string Name { get; }
    public long Size { get; }
    public DateTime CreationDate { get; }
    public DateTime? LastModifiedDate { get; }
    public IDictionary<string, string> Metadata { get; }

    public OssContainer(
        string name,
        DateTime creationDate,
        long size = 0,
        DateTime? lastModifiedDate = null,
        IDictionary<string, string> metadata = null)
    {
        Name = name;
        CreationDate = creationDate;
        LastModifiedDate = lastModifiedDate;
        Size = size;
        Metadata = metadata ?? new Dictionary<string, string>();
    }
}
