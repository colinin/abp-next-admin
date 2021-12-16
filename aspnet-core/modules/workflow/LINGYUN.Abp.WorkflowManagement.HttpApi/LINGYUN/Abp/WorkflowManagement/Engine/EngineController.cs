using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WorkflowManagement.Engine
{
    [RemoteService(Name = WorkflowManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("WorkflowManagement")]
    [Route("api/workflow-management/engine")]
    public class EngineController : AbpControllerBase, IEngineAppService
    {
        private readonly IEngineAppService _service;

        public EngineController(IEngineAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("initialize")]
        public virtual async Task InitializeAsync()
        {
            await _service.InitializeAsync();
        }

        [HttpPost]
        [Route("register")]
        public virtual async Task RegisterAsync()
        {
            await _service.RegisterAsync();
        }
    }
}
