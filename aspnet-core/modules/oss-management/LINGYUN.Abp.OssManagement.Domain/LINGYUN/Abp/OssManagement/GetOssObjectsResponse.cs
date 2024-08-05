using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement;

public class GetOssObjectsResponse
{
    public string Bucket { get; }
    public string Prefix { get; }
    public string Delimiter { get; }
    public string Marker { get; }
    public string NextMarker { get; }
    public int MaxKeys { get; }
    public List<OssObject> Objects { get; }
    public GetOssObjectsResponse(
        string bucket,
        string prefix,
        string marker,
        string nextMarker,
        string delimiter,
        int maxKeys,
        List<OssObject> ossObjects)
    {
        Bucket = bucket;
        Prefix = prefix;
        Marker = marker;
        NextMarker = nextMarker;
        Delimiter = delimiter;
        MaxKeys = maxKeys;

        Objects = ossObjects;
    }
}
