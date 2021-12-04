using Volo.Abp.Collections;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore
{
    public class AbpWorkflowCoreOptions
    {
        public ITypeList<IWorkflow> DefinitionProviders { get; }
    }
}
