using RulesEngine.Models;
using System.Collections.Generic;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class WorkflowRulesResolveResult
    {
        public List<WorkflowRules> WorkflowRules { get; set; }

        public List<string> AppliedResolvers { get; }

        public WorkflowRulesResolveResult()
        {
            AppliedResolvers = new List<string>();
            WorkflowRules = new List<WorkflowRules>();
        }
    }
}
