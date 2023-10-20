using Microsoft.Extensions.Options;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp
{
    public class DynamicOptionsProvider<TValue> : IOptionsProvider<TValue>, ITransientDependency
        where TValue : class, new()
    {
        public TValue Value => _lazyValueFactory.Value;

        private readonly Lazy<TValue> _lazyValueFactory;
        private readonly OneTimeRunner _oneTimeRunner;

        public DynamicOptionsProvider(IOptions<TValue> options)
        {
            _oneTimeRunner = new OneTimeRunner();
            _lazyValueFactory = new Lazy<TValue>(() => CreateOptions(options));
        }

        protected virtual TValue CreateOptions(IOptions<TValue> options)
        {
            // 用于简化需要在使用配置前自行调用此接口的繁复步骤
            // await options.SetAsync();
            // _onTimeRunner.Run(async () => await options.SetAsync());
            return options.Value;
        }
    }
}
