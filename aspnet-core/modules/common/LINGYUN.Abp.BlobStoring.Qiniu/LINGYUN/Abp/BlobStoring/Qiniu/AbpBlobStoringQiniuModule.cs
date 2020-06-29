using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobStoring.Qiniu
{
    [DependsOn(typeof(AbpBlobStoringModule))]
    public class AbpBlobStoringQiniuModule : AbpModule
    {
    }
}
