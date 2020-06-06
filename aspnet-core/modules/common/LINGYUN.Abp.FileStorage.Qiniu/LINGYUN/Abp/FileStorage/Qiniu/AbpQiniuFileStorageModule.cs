using Volo.Abp.Modularity;

namespace LINGYUN.Abp.FileStorage.Qiniu
{
    [DependsOn(typeof(AbpFileStorageModule))]
    public class AbpQiniuFileStorageModule : AbpModule
    {

    }
}
