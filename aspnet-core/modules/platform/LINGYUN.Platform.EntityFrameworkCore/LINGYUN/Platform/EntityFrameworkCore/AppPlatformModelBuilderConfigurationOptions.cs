using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    public class AppPlatformModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public AppPlatformModelBuilderConfigurationOptions(
           [NotNull] string tablePrefix = "",
           [CanBeNull] string schema = null)
           : base(
               tablePrefix,
               schema)
        {

        }
    }
}
