using LINGYUN.Abp.Wrapper;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.HttpClient.Wrapper
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpWrapperModule))]
    public class AbpHttpClientWrapperModule : AbpModule
    {
    }
}
