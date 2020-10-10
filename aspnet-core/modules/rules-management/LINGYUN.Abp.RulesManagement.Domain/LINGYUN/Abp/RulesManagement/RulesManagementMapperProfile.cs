using AutoMapper;
using LINGYUN.Abp.Rules;

namespace LINGYUN.Abp.RulesManagement
{
    public class RulesManagementMapperProfile : Profile
    {
        public RulesManagementMapperProfile()
        {
            CreateMap<EntityRuleParam, RuleParam>();
            CreateMap<EntityRule, Rule>()
                .ForMember(rule => rule.InjectRules, map => map.Ignore())
                .ForMember(rule => rule.Rules, map => map.Ignore());
            CreateMap<EntityRuleGroup, RuleGroup>()
                .ForMember(rule => rule.InjectRules, map => map.Ignore())
                .ForMember(rule => rule.Rules, map => map.Ignore());

            CreateMap<EntityRule, EntityRuleEto>();
            CreateMap<EntityRuleGroup, EntityRuleGroupEto>();
        }
    }
}
