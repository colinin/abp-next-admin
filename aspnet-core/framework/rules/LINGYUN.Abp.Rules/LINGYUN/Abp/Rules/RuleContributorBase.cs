using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules
{
    public abstract class RuleContributorBase : IRuleContributor
    {
        public ILogger Logger { get; protected set; }

        protected RuleContributorBase()
        {
            Logger = NullLogger<RuleContributorBase>.Instance;
        }

        public virtual void Initialize(RulesInitializationContext context)
        {
        }

        public virtual Task ExecuteAsync<T>(T input, object[] @params = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public virtual void Shutdown()
        {

        }
    }
}
