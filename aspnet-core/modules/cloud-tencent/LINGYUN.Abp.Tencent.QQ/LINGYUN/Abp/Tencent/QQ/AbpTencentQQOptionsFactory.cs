using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Tencent.QQ
{
    public class AbpTencentQQOptionsFactory : ITransientDependency
    {
        protected IOptions<AbpTencentQQOptions> Options { get; }

        public AbpTencentQQOptionsFactory(
            IOptions<AbpTencentQQOptions> options)
        {
            Options = options;
        }

        public async virtual Task<AbpTencentQQOptions> CreateAsync()
        {
            await Options.SetAsync();

            return Options.Value;
        }
    }
}
