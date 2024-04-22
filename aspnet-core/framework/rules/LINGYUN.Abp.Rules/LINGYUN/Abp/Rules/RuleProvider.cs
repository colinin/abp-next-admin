using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules
{
    public class RuleProvider : IRuleProvider, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RuleProvider> _logger;

        private readonly IEnumerable<IRuleContributor> _rulesEngineContributors;

        public RuleProvider(
            IServiceProvider serviceProvider,
            IOptions<AbpRulesOptions> options,
            ILogger<RuleProvider> logger)
        {
            _rulesEngineContributors = options.Value
                .Contributors
                .Select(serviceProvider.GetRequiredService)
                .Cast<IRuleContributor>()
                .ToArray();

            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async virtual Task ExecuteAsync<T>(T input, object[] @params = null, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Starting all typed rules engine.");

            foreach (var contributor in _rulesEngineContributors)
            {
                await contributor.ExecuteAsync(input, @params, cancellationToken);
            }

            _logger.LogDebug("Executed all typed rules engine.");
        }

        internal void Initialize()
        {
            var context = new RulesInitializationContext(_serviceProvider);

            foreach (var contributor in _rulesEngineContributors)
            {
                contributor.Initialize(context);
            }
        }

        internal void Shutdown()
        {
            foreach (var contributor in _rulesEngineContributors)
            {
                contributor.Shutdown();
            }
        }
    }
}
