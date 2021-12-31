using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobStoring.Tencent
{
    public class DefaultTencentBlobNameCalculator : ITencentBlobNameCalculator, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        public DefaultTencentBlobNameCalculator(
            ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        public string Calculate(BlobProviderArgs args)
        {
            return CurrentTenant.Id == null
                ? $"host/{args.BlobName}"
                : $"tenants/{CurrentTenant.Id.Value:D}/{args.BlobName}";
        }
    }
}
