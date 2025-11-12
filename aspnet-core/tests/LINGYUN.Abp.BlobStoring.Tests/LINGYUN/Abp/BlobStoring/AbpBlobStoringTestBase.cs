using Volo.Abp;
using Volo.Abp.Testing;

namespace LINGYUN.Abp.BlobStoring;

public abstract class AbpBlobStoringTestBase : AbpIntegratedTest<AbpBlobStoringTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
