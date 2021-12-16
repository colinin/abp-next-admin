using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Volo.Abp.ExceptionHandling;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.ExceptionHandling
{
    /// <summary>
    /// 不会自动注册到容器
    /// 需要时手动注册
    /// </summary>
    public class ExceptionNotifierHandler : IWorkflowErrorHandler
    {
        public WorkflowErrorHandling Type => WorkflowErrorHandling.Terminate;

        private readonly IExceptionNotifier _exceptionNotifier;

        public ExceptionNotifierHandler(IExceptionNotifier exceptionNotifier)
        {
            _exceptionNotifier = exceptionNotifier;
        }

        public void Handle(WorkflowInstance workflow, WorkflowDefinition def, ExecutionPointer pointer, WorkflowStep step, Exception exception, Queue<ExecutionPointer> bubbleUpQueue)
        {
            _ = _exceptionNotifier.NotifyAsync(
                new ExceptionNotificationContext(
                    exception,
                    LogLevel.Warning));
        }
    }
}
