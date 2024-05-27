using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.TextTemplating.EntityFrameworkCore;

public static class AbpTextTemplatingDbContextModelCreatingExtensions
{
    public static void ConfigureTextTemplating(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<TextTemplate>(b =>
        {
            b.ToTable(AbpTextTemplatingDbProperties.DbTablePrefix + "TextTemplates", AbpTextTemplatingDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name)
                .HasColumnName(nameof(TextTemplate.Name))
                .HasMaxLength(TextTemplateConsts.MaxNameLength)
                .IsRequired();
            b.Property(t => t.DisplayName)
                .HasColumnName(nameof(TextTemplate.DisplayName))
                .HasMaxLength(TextTemplateConsts.MaxDisplayNameLength)
                .IsRequired();

            b.Property(t => t.Culture)
                .HasColumnName(nameof(TextTemplate.Culture))
                .HasMaxLength(TextTemplateConsts.MaxCultureLength);
            b.Property(t => t.Content)
                .HasColumnName(nameof(TextTemplate.Content))
                .HasMaxLength(TextTemplateConsts.MaxContentLength);

            b.HasIndex(p => p.Name)
                 .HasDatabaseName("IX_Tenant_Text_Template_Name");

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<TextTemplateDefinition>(b =>
        {
            b.ToTable(AbpTextTemplatingDbProperties.DbTablePrefix + "TextTemplateDefinitions", AbpTextTemplatingDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name)
                .HasColumnName(nameof(TextTemplateDefinition.Name))
                .HasMaxLength(TextTemplateDefinitionConsts.MaxNameLength)
                .IsRequired();
            b.Property(t => t.DisplayName)
                .HasColumnName(nameof(TextTemplateDefinition.DisplayName))
                .HasMaxLength(TextTemplateDefinitionConsts.MaxDisplayNameLength)
                .IsRequired();

            b.Property(t => t.Layout)
                .HasColumnName(nameof(TextTemplateDefinition.Layout))
                .HasMaxLength(TextTemplateDefinitionConsts.MaxLayoutLength);
            b.Property(t => t.DefaultCultureName)
                .HasColumnName(nameof(TextTemplateDefinition.DefaultCultureName))
                .HasMaxLength(TextTemplateDefinitionConsts.MaxDefaultCultureNameLength);
            b.Property(t => t.RenderEngine)
               .HasColumnName(nameof(TextTemplateDefinition.RenderEngine))
               .HasMaxLength(TextTemplateDefinitionConsts.MaxRenderEngineLength);
            b.Property(t => t.LocalizationResourceName)
               .HasColumnName(nameof(TextTemplateDefinition.LocalizationResourceName))
               .HasMaxLength(TextTemplateDefinitionConsts.MaxLocalizationResourceNameLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<TextTemplatingDbContext>();
    }
}
