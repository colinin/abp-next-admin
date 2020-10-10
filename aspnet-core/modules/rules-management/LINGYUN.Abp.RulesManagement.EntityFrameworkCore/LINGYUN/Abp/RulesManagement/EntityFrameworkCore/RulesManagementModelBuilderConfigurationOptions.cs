using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.RulesManagement.EntityFrameworkCore
{
    public class RulesManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public RulesManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema) 
            : base(tablePrefix, schema)
        {
        }
    }
}
