using Microsoft.Extensions.DependencyInjection;
using NRules;
using NRules.RuleModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.NRules
{
    public class NRulesContributor : RuleContributorBase, ISingletonDependency
    {
        private ISessionFactory _sessionFactory;

        public override void Initialize(RulesInitializationContext context)
        {
            var repository = context.GetRequiredService<IRuleRepository>();
            _sessionFactory = repository.Compile();
            _sessionFactory.DependencyResolver = new DefaultDependencyResolver(context.ServiceProvider);
            _sessionFactory.ActionInterceptor = new DefaultActionInterceptor();
        }

        public override Task ExecuteAsync<T>(T input, object[] @params = null, CancellationToken cancellationToken = default)
        {
            var session = _sessionFactory.CreateSession();

            session.Insert(input);
            if (@params != null && @params.Any())
            {
                session.InsertAll(@params);
            }

            // TODO: 需要研究源码
            session.Fire(cancellationToken);

            return Task.CompletedTask;
        }
    }
}
