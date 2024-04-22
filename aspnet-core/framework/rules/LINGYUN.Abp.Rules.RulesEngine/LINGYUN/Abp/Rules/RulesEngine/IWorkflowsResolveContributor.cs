using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public interface IWorkflowsResolveContributor
    {
        string Name { get; }

        Task ResolveAsync(IWorkflowsResolveContext context);

        void Initialize(RulesInitializationContext context);

        void Shutdown();
    }
}
