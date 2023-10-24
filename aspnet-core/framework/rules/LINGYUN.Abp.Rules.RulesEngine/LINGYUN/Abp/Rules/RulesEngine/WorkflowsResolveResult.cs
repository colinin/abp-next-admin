using RulesEngine.Models;
using System.Collections.Generic;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class WorkflowsResolveResult
    {
        public List<Workflow> Workflows { get; set; }

        public List<string> AppliedResolvers { get; }

        public WorkflowsResolveResult()
        {
            AppliedResolvers = new List<string>();
            Workflows = new List<Workflow>();
        }
    }
}
