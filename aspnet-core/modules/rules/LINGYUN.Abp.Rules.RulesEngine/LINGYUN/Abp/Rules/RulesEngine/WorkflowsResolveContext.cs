using JetBrains.Annotations;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class WorkflowsResolveContext : IWorkflowsResolveContext
    {
        public Type Type { get; }
        public IServiceProvider ServiceProvider { get; }
        public IEnumerable<Workflow> Workflows { get; set; }
        public bool Handled { get; set; }

        public bool HasResolved()
        {
            return Handled && Workflows?.Any() == true;
        }

        public WorkflowsResolveContext(
            [NotNull] Type type,
            IServiceProvider serviceProvider)
        {
            Type = Check.NotNull(type, nameof(type));
            ServiceProvider = serviceProvider;
        }
    }
}
