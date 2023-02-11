using System;

namespace LINGYUN.Abp.OssManagement;
public class ExprieOssObjectRequest
{
    public string Bucket { get; }
    public int Batch { get; }
    public DateTimeOffset ExpirationTime { get; }
    public ExprieOssObjectRequest(
        string bucket, 
        int batch,
        DateTimeOffset expirationTime)
    {
        Bucket = bucket;
        Batch = batch;
        ExpirationTime = expirationTime;
    }
}
