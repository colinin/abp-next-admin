using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.DataProtection
{
    public class FakeDataProtectdChecker : IDataProtectdChecker, ISingletonDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FakeDataProtectdChecker(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual Task<ResourceGrantedResult> IsGrantedAsync<T>(ProtectBehavior behavior = ProtectBehavior.All)
        {
            var cacheItem = _unitOfWorkManager.Current.Items["ResourceGranted"];
            var result = cacheItem.As<ResourceGrantedResult>();

            return Task.FromResult(result);
        }
    }
}
