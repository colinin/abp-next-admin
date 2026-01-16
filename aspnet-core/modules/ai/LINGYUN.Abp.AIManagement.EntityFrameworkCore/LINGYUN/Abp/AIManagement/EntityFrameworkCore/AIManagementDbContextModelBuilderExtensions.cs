using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;
public static class AIManagementDbContextModelBuilderExtensions
{
    public static void ConfigureAIManagement(
        [NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.TryConfigureObjectExtensions<AIManagementDbContext>();
    }
}
