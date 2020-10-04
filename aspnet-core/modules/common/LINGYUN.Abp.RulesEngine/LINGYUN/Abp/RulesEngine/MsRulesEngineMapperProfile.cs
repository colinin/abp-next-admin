using AutoMapper;
using LINGYUN.Abp.Rules;
using MsExpressionType = RulesEngine.Models.RuleExpressionType;
using MsRule = RulesEngine.Models.Rule;
using MsRuleErrorType = RulesEngine.Models.ErrorType;
using MsRuleParam = RulesEngine.Models.LocalParam;
using MsWorkflowRules = RulesEngine.Models.WorkflowRules;

namespace LINGYUN.Abp.RulesEngine
{
    public class MsRulesEngineMapperProfile : Profile
    {
        public MsRulesEngineMapperProfile()
        {
            CreateMap<RuleParam, MsRuleParam>();
            CreateMap<Rule, MsRule>()
                .ForMember(r => r.LocalParams, map => map.MapFrom(m => m.Params))
                .ForMember(r => r.WorkflowRulesToInject, map => map.MapFrom(m => m.InjectRules))
                .ForMember(r => r.ErrorType, map => map.MapFrom(m => (MsRuleErrorType)m.ErrorType.GetHashCode()))
                .ForMember(r => r.RuleExpressionType, map => map.MapFrom(m => (MsExpressionType)m.ExpressionType.GetHashCode()));
            CreateMap<RuleGroup, MsWorkflowRules>()
                .ForMember(wr => wr.WorkflowName, map => map.MapFrom(m => m.Name))
                .ForMember(wr => wr.Rules, map => map.MapFrom(m => m.Rules))
                .ForMember(wr => wr.WorkflowRulesToInject, map => map.MapFrom(m => m.InjectRules));
        }
    }
}
