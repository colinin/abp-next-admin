using System;
using Volo.Abp.Data;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowManagement
{
    public interface IStep
    {
        Guid WorkflowId { get; }
        string Name { get; }
        string StepType { get; }
        Guid? ParentId { get; }
        WorkflowErrorHandling? ErrorBehavior { get; }
        string CancelCondition { get; set; }
        TimeSpan? RetryInterval { get; set; }
        bool Saga { get; set; }
        ExtraPropertyDictionary Inputs { get; set; }
        ExtraPropertyDictionary Outputs { get; set; }
        ExtraPropertyDictionary SelectNextStep { get; set; }
    }
}
