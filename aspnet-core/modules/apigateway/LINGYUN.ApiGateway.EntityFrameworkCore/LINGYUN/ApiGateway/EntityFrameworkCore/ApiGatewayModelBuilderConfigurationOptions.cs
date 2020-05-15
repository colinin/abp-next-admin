using JetBrains.Annotations;
using LINGYUN.ApiGateway.Settings;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    public class ApiGatewayModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ApiGatewayModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = ApiGatewaySettingNames.DefaultDbTablePrefix,
            [CanBeNull] string schema = ApiGatewaySettingNames.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}
