using LINGYUN.Abp.WorkflowManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowManagement.Engine
{
    [Authorize(WorkflowManagementPermissions.Engine.Default)]
    public class EngineAppService : WorkflowManagementAppServiceBase, IEngineAppService
    {
        private readonly IWorkflowEngineManager _engineManager;

        public EngineAppService(IWorkflowEngineManager engineManager)
        {
            _engineManager = engineManager;
        }

        [Authorize(WorkflowManagementPermissions.Engine.Initialize)]
        public virtual async Task InitializeAsync()
        {
            await _engineManager.InitializeAsync();
        }
    }
}
