using JetBrains.Annotations;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class WorkflowRulesResolveContext : IWorkflowRulesResolveContext
    {
        public Type Type { get; }
        public IServiceProvider ServiceProvider { get; }
        public IEnumerable<WorkflowRules> WorkflowRules { get; set; }
        public bool Handled { get; set; }

        public bool HasResolved()
        {
            return Handled && WorkflowRules?.Any() == true;
        }

        public WorkflowRulesResolveContext(
            [NotNull] Type type,
            IServiceProvider serviceProvider)
        {
            Type = Check.NotNull(type, nameof(type));
            ServiceProvider = serviceProvider;
        }
    }
}
