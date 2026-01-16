using JetBrains.Annotations;
using LINGYUN.Abp.AIManagement.Workspaces;
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

        builder.Entity<Workspace>(b =>
        {
            b.ToTable(AbpAIManagementDbProperties.DbTablePrefix + "Workspaces", AbpAIManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).HasMaxLength(WorkspaceConsts.MaxNameLength).IsRequired();
            b.Property(x => x.Provider).HasMaxLength(WorkspaceConsts.MaxProviderLength).IsRequired();
            b.Property(x => x.ModelName).HasMaxLength(WorkspaceConsts.MaxModelNameLength).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(WorkspaceConsts.MaxDisplayNameLength).IsRequired();
            b.Property(x => x.Description).HasMaxLength(WorkspaceConsts.MaxDescriptionLength);
            b.Property(x => x.ApiKey).HasMaxLength(WorkspaceConsts.MaxApiKeyLength);
            b.Property(x => x.ApiBaseUrl).HasMaxLength(WorkspaceConsts.MaxApiKeyLength);
            b.Property(x => x.SystemPrompt).HasMaxLength(WorkspaceConsts.MaxSystemPromptLength);
            b.Property(x => x.Instructions).HasMaxLength(WorkspaceConsts.MaxInstructionsLength);
            b.Property(x => x.StateCheckers).HasMaxLength(WorkspaceConsts.MaxStateCheckersLength);

            b.HasIndex(x => new { x.Name }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<AIManagementDbContext>();
    }
}
