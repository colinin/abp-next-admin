using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore
{
    public static class AbpDataProtectionDbContextModelBuilderExtensions
    {
        public static void ConfigureLocalization(
            this ModelBuilder builder,
           Action<AbpDataProtectionModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));
        }
    }
}
