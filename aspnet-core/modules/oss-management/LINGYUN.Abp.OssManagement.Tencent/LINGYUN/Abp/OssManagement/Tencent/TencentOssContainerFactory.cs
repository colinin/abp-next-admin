using LINGYUN.Abp.BlobStoring.Tencent;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.OssManagement.Tencent
{
    public class TencentOssContainerFactory : IOssContainerFactory
    {
        protected IClock Clock { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ICosClientFactory CosClientFactory { get; }

        public TencentOssContainerFactory(
            IClock clock,
            ICurrentTenant currentTenant,
            ICosClientFactory cosClientFactory)
        {
            Clock = clock;
            CurrentTenant = currentTenant;
            CosClientFactory = cosClientFactory;
        }

        public IOssContainer Create()
        {
            return new TencentOssContainer(
                Clock,
                CurrentTenant,
                CosClientFactory);
        }
    }
}
