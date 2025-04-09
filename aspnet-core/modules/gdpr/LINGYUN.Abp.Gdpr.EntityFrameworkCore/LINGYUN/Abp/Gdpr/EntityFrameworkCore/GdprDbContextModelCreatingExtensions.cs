using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.Gdpr.EntityFrameworkCore;
public static class GdprDbContextModelCreatingExtensions
{
    public static void ConfigureGdpr(
        this ModelBuilder builder,
        Action<GdprModelBuilderConfigurationOptions>? optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new GdprModelBuilderConfigurationOptions(
            GdprDbProterties.DbTablePrefix,
            GdprDbProterties.DbSchema
        );
        optionsAction?.Invoke(options);

        builder.Entity<GdprRequest>(b =>
        {
            b.ToTable(options.TablePrefix + "Requests", options.Schema);

            b.HasMany(p => p.Infos)
             .WithOne()
             .HasForeignKey(fk => fk.RequestId)
             .IsRequired(true);

            b.HasIndex(p => p.UserId);

            b.ConfigureByConvention();
        });
        builder.Entity<GdprInfo>(b =>
        {
            b.ToTable(options.TablePrefix + "Infos", options.Schema);

            b.Property(p => p.Provider)
             .HasColumnName(nameof(GdprInfo.Provider))
             .HasMaxLength(GdprInfoConsts.MaxProviderLength)
             .IsRequired();

            b.Property(p => p.Data)
             .HasColumnName(nameof(GdprInfo.Data))
             .IsRequired();

            b.HasIndex(p => p.RequestId);

            b.ConfigureByConvention();
        });

        builder.TryConfigureObjectExtensions<GdprDbContext>();
    }
}
