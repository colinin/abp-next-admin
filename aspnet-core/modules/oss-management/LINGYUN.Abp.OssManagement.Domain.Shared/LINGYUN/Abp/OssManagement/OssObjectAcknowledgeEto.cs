using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.OssManagement;

/// <summary>
/// 对象缓存确认信号
/// </summary>
[Serializable]
[EventName("abp.blob-storing.oss-object.ack")]
public class OssObjectAcknowledgeEto
{
    public string TempPath { get; set; }
    public string Bucket { get; set; }
    public string Path { get; set; }
    public string Object { get; set; }
    public TimeSpan? ExpirationTime { get; set; }
    public OssObjectAcknowledgeEto()
    {

    }

    public OssObjectAcknowledgeEto(
        string tempPath,
        string bucket,
        string path,
        string @object,
        TimeSpan? expirationTime = null)
    {
        TempPath = tempPath;
        Bucket = bucket;
        Path = path;
        Object = @object;
        ExpirationTime = expirationTime;
    }
}
