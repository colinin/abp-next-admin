using LINGYUN.Abp.PrintDesigner.CustomElements;
using LINGYUN.Abp.PrintDesigner.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.PrintDesigner.EntityFrameworkCore;

public static class PrintDesignerDbContextModelCreatingExtensions
{
    public static void ConfigurePrintDesigner(
        this ModelBuilder builder,
        Action<PrintDesignerModelBuilderConfigurationOptions>? optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new PrintDesignerModelBuilderConfigurationOptions(
            PrintDesignerDbProperties.DbTablePrefix,
            PrintDesignerDbProperties.DbSchema
        );
        optionsAction?.Invoke(options);

        builder.Entity<CustomElement>(e =>
        {
            e.ToTable(options.TablePrefix + "CustomElements", options.Schema);

            e.Property(p => p.Name)
                .HasMaxLength(CustomElementConsts.MaxNameLength)
                .HasColumnName(nameof(CustomElement.Name))
                .IsRequired();
            e.Property(p => p.Description)
                .HasMaxLength(CustomElementConsts.MaxDescriptionLength)
                .HasColumnName(nameof(CustomElement.Description));

            e.ConfigureByConvention();

            e.HasOne(p => p.Data)
                .WithOne()
                .HasPrincipalKey<CustomElement>(pk => pk.Id)
                .HasForeignKey<CustomElementData>(pk => pk.ElementId)
                .IsRequired();

            e.HasIndex(x => x.Name).IsUnique();
        });
        builder.Entity<CustomElementData>(e =>
        {
            e.ToTable(options.TablePrefix + "CustomElementDatas", options.Schema);

            e.ConfigureByConvention();
        });

        builder.Entity<Template>(e =>
        {
            e.ToTable(options.TablePrefix + "Templates", options.Schema);

            e.Property(p => p.Name)
                .HasMaxLength(TemplateConsts.MaxNameLength)
                .HasColumnName(nameof(Template.Name))
                .IsRequired();
            e.Property(p => p.Description)
                .HasMaxLength(TemplateConsts.MaxDescriptionLength)
                .HasColumnName(nameof(Template.Description));

            e.ConfigureByConvention();

            e.HasOne(p => p.Data)
                .WithOne()
                .HasPrincipalKey<Template>(pk => pk.Id)
                .HasForeignKey<TemplateData>(pk => pk.TemplateId);

            e.HasIndex(x => x.Name).IsUnique();
        });
        builder.Entity<TemplateData>(e =>
        {
            e.ToTable(options.TablePrefix + "TemplateDatas", options.Schema);

            e.ConfigureByConvention();
        });
    }
}
