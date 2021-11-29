using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OpenApi
{
    [Dependency(ReplaceServices = true)]
    public class FakeAppKeyStore : IAppKeyStore, ISingletonDependency
    {
        public readonly static AppDescriptor AppDescriptor = 
            new AppDescriptor(
                "TEST",
                Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"),
                signLifeTime: 5);

        public virtual Task<AppDescriptor> FindAsync(string appKey)
        {
            AppDescriptor app = null;
            if (AppDescriptor.AppKey.Equals(appKey))
            {
                app = AppDescriptor;
            }

            return Task.FromResult(app);
        }
    }
}
