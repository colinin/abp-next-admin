using LINGYUN.Abp.WorkflowManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowManagement.Engine
{
    [Authorize(WorkflowManagementPermissions.Engine.Default)]
    public class EngineAppService : WorkflowManagementAppServiceBase, IEngineAppService
    {
        private readonly IWorkflowEngineManager _engineManager;
        private readonly IWorkflowRegistryManager _registryManager;

        public EngineAppService(
            IWorkflowEngineManager engineManager,
            IWorkflowRegistryManager registryManager)
        {
            _engineManager = engineManager;
            _registryManager = registryManager;
        }

        [Authorize(WorkflowManagementPermissions.Engine.Initialize)]
        public virtual async Task InitializeAsync()
        {
            await _engineManager.InitializeAsync();
        }

        [Authorize(WorkflowManagementPermissions.Engine.Register)]
        public virtual async Task RegisterAsync()
        {
            // 暂时没有解决多租户自动注册工作流,管理员可以通过api主动注册
            await _registryManager.RegisterAsync();
        }
    }
}
