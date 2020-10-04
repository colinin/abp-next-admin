using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.Rules
{
    public class EntityRuleContext
    {
        public List<RuleGroup> Groups { get; }
        public IEntity Entity { get; }
        public EntityRuleContext(
            List<RuleGroup> groups,
            IEntity entity)
        {
            Groups = groups;
            Entity = entity;
        }
    }
}
