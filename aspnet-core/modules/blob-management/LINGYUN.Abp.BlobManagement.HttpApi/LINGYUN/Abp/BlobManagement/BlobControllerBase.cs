using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.BlobManagement;

public abstract class BlobControllerBase : AbpControllerBase
{
    private const string CompareMd5HeaderKey = "X-Abp-Blob-Md5";

    protected virtual string? GetCompareMd5InHeader()
    {
        return Request.Headers.GetOrDefault(CompareMd5HeaderKey);
    }
}
