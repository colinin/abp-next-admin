using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NRules;
using NRules.Fluent;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.NRules
{
    public class NRulesContributor : RuleContributorBase, ISingletonDependency
    {
        private readonly AbpNRulesOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public NRulesContributor(
            IServiceProvider serviceProvider,
            IOptions<AbpNRulesOptions> options)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public override void Initialize(RulesInitializationContext context)
        {
            context.GetRequiredService<RuleRepository>()
                .Load(loader => loader.From(_options.DefinitionRules));
        }

        public override Task ExecuteAsync<T>(T input, object[] @params = null, CancellationToken cancellationToken = default)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var session = scope.ServiceProvider.GetRequiredService<ISession>();

                session.Insert(input);
                if (@params != null && @params.Any())
                {
                    session.InsertAll(@params);
                }

                // TODO: 需要研究源码
                session.Fire(cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
