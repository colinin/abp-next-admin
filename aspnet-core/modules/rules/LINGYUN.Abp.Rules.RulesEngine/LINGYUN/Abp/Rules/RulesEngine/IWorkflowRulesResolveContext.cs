using JetBrains.Annotations;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public interface IWorkflowRulesResolveContext : IServiceProviderAccessor
    {
        [CanBeNull]
        IEnumerable<WorkflowRules> WorkflowRules { get; set; }

        [NotNull]
        Type Type { get; }

        bool Handled { get; set; }
    }
}
