using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity.EntityFrameworkCore;

public static class IdentityUserInactiveDbContextModelBuilderExtensions
{
    public static void ConfigureIdentityUserInactive([NotNull] this ModelBuilder builder)
    {
        builder.Entity<IdentityUserInactive>(b =>
        {
            b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UserInactives", AbpIdentityDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasIndex(x => new { x.TenantId, x.UserId });

            b.ApplyObjectExtensionMappings();
        });
    }
}
