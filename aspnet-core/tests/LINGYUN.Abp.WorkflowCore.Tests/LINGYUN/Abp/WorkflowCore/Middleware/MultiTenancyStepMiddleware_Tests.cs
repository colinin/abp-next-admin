using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using Xunit;

namespace LINGYUN.Abp.WorkflowCore.Middleware
{
    public class MultiTenancyStepMiddleware_Tests : AbpWorkflowCoreTestBase
    {
        public static bool IsCompleted = false;
        public static readonly Guid TenantId = Guid.NewGuid();
        private readonly IWorkflowController _controller;
        public MultiTenancyStepMiddleware_Tests()
        {
            _controller = GetRequiredService<IWorkflowController>();
        }

        [Fact]
        public async Task Should_Be_Resolved_Multi_Tenant_Id_On_Step()
        {
            await _controller.StartWorkflow(
                "MiddlewareWorkflow",
                new MiddlewareWorkflowData
                {
                    TenantId = TenantId
                });

        }
    }

    public class MiddlewareWorkflowData : IMultiTenant
    {
        public Guid? TenantId { get; set; }
    }

    public class MiddlewareWorkflow : IWorkflow<MiddlewareWorkflowData>
    {
        public string Id => "MiddlewareWorkflow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<MiddlewareWorkflowData> builder)
        {
            builder.StartWith((_) => Console.WriteLine("Start Workflow"))
                .Then<MessageStep>()
                .Then((_) => MultiTenancyStepMiddleware_Tests.IsCompleted = true)
                .EndWorkflow();
        }
    }

    public class MessageStep : StepBodyBase
    {
        private readonly ICurrentTenant _currentTenant;

        public MessageStep(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            MultiTenancyStepMiddleware_Tests.TenantId.ShouldBe(_currentTenant.Id.Value);

            Console.WriteLine("Current Tenant Id: {0}", _currentTenant.Id?.ToString() ?? "Null");

            return ExecutionResult.Next();
        }
    }
}
