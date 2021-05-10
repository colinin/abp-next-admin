using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Rules
{
    public class RuleIdGenerator : ISingletonDependency
    {
        private readonly ICurrentTenant _currentTenant;

        public RuleIdGenerator(
            ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public virtual int CreateRuleId(Type type, bool ignoreMultiTenancy = false)
        {
            var typeId = type.GetHashCode();

            if (!ignoreMultiTenancy)
            {
                if (_currentTenant.Id.HasValue)
                {
                    return _currentTenant.Id.GetHashCode() & typeId;
                }
            }

            return typeId;
        }

        public virtual string CreateRuleName(Type type, bool ignoreMultiTenancy = false)
        {
            var ruleName = type.Name;
            if (!ignoreMultiTenancy)
            {
                if (_currentTenant.Id.HasValue)
                {
                    return $"{_currentTenant.Id.Value:D}/{ruleName}";
                }
            }
            return ruleName;
        }
    }
}
