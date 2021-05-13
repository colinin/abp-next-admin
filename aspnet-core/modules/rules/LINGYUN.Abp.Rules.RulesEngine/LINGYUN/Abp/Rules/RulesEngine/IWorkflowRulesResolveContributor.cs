using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public interface IWorkflowRulesResolveContributor
    {
        string Name { get; }

        Task ResolveAsync(IWorkflowRulesResolveContext context);

        void Initialize(RulesInitializationContext context);

        void Shutdown();
    }
}
