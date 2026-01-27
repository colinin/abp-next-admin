using JetBrains.Annotations;
using LINGYUN.Abp.AIManagement.Chats;
using LINGYUN.Abp.AIManagement.EntityFrameworkCore.ValueConversions;
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

        builder.Entity<ConversationRecord>(b =>
        {
            b.ToTable(AbpAIManagementDbProperties.DbTablePrefix + "Conversations", AbpAIManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .HasMaxLength(ConversationRecordConsts.MaxNameLength)
                .IsRequired();
        });
        builder.Entity<TextChatMessageRecord>(b =>
        {
            b.ToTable(AbpAIManagementDbProperties.DbTablePrefix + "TextChatMessages", AbpAIManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Workspace)
                .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxNameLength)
                .IsRequired();
            b.Property(x => x.Role)
                .HasMaxLength(ChatMessageRecordConsts.MaxChatRoleLength)
                .HasConversion(new ChatRoleValueConverter())
                .IsRequired();
            b.Property(x => x.Content)
                .HasMaxLength(TextChatMessageRecordConsts.MaxContentLength)
                .IsRequired();

            b.HasIndex(x => new { x.TenantId, x.ConversationId });

            b.ApplyObjectExtensionMappings();
        });

        if (builder.IsHostDatabase())
        {
            builder.Entity<WorkspaceDefinitionRecord>(b =>
            {
                b.ToTable(AbpAIManagementDbProperties.DbTablePrefix + "WorkspaceDefinitions", AbpAIManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxNameLength)
                    .IsRequired();
                b.Property(x => x.Provider)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxProviderLength)
                    .IsRequired();
                b.Property(x => x.ModelName)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxModelNameLength)
                    .IsRequired();
                b.Property(x => x.DisplayName)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxDisplayNameLength)
                    .IsRequired();
                b.Property(x => x.Description)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxDescriptionLength);
                b.Property(x => x.ApiKey)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxApiKeyLength);
                b.Property(x => x.ApiBaseUrl)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxApiKeyLength);
                b.Property(x => x.SystemPrompt)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxSystemPromptLength);
                b.Property(x => x.Instructions)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxInstructionsLength);
                b.Property(x => x.StateCheckers)
                    .HasMaxLength(WorkspaceDefinitionRecordConsts.MaxStateCheckersLength);

                b.HasIndex(x => new { x.Name }).IsUnique();

                b.ApplyObjectExtensionMappings();
            });
        }

        builder.TryConfigureObjectExtensions<AIManagementDbContext>();
    }
}
