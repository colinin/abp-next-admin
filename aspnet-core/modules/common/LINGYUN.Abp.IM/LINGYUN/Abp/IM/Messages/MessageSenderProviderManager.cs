using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM.Messages
{
    public class MessageSenderProviderManager : IMessageSenderProviderManager, ISingletonDependency
    {
        public List<IMessageSenderProvider> Providers => _lazyProviders.Value;

        protected AbpIMOptions Options { get; }

        private readonly Lazy<List<IMessageSenderProvider>> _lazyProviders;

        public MessageSenderProviderManager(
            IServiceProvider serviceProvider,
            IOptions<AbpIMOptions> options)
        {
            Options = options.Value;

            _lazyProviders = new Lazy<List<IMessageSenderProvider>>(
                () => Options
                    .Providers
                    .Select(type => serviceProvider.GetRequiredService(type) as IMessageSenderProvider)
                    .ToList(),
                true
            );
        }
    }
}
