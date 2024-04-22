using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    public static class LocalizationDbContextModelBuilderExtensions
    {
        public static void ConfigureLocalization(
            this ModelBuilder builder,
           Action<LocalizationModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new LocalizationModelBuilderConfigurationOptions(
                LocalizationDbProperties.DbTablePrefix,
                LocalizationDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Language>(x =>
            {
                x.ToTable(options.TablePrefix + "Languages", options.Schema);

                x.Property(p => p.CultureName)
                    .IsRequired()
                    .HasMaxLength(LanguageConsts.MaxCultureNameLength)
                    .HasColumnName(nameof(Language.CultureName));
                x.Property(p => p.UiCultureName)
                    .IsRequired()
                    .HasMaxLength(LanguageConsts.MaxUiCultureNameLength)
                    .HasColumnName(nameof(Language.UiCultureName));
                x.Property(p => p.DisplayName)
                    .IsRequired()
                    .HasMaxLength(LanguageConsts.MaxDisplayNameLength)
                    .HasColumnName(nameof(Language.DisplayName));

                x.Property(p => p.FlagIcon)
                    .IsRequired(false)
                    .HasMaxLength(LanguageConsts.MaxFlagIconLength)
                    .HasColumnName(nameof(Language.FlagIcon));

                x.Property(p => p.Enable)
                    .HasDefaultValue(true);

                x.ConfigureByConvention();

                x.HasIndex(p => p.CultureName);
            });

            builder.Entity<Resource>(x =>
            {
                x.ToTable(options.TablePrefix + "Resources", options.Schema);

                x.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(ResourceConsts.MaxNameLength)
                    .HasColumnName(nameof(Resource.Name));

                x.Property(p => p.DisplayName)
                    .HasMaxLength(ResourceConsts.MaxDisplayNameLength)
                    .HasColumnName(nameof(Resource.DisplayName));
                x.Property(p => p.Description)
                    .HasMaxLength(ResourceConsts.MaxDescriptionLength)
                    .HasColumnName(nameof(Resource.Description));
                x.Property(p => p.DefaultCultureName)
                    .HasMaxLength(ResourceConsts.MaxDefaultCultureNameLength)
                    .HasColumnName(nameof(Resource.DefaultCultureName));

                x.Property(p => p.Enable)
                    .HasDefaultValue(true);

                x.ConfigureByConvention();

                x.HasIndex(p => p.Name);
            });

            builder.Entity<Text>(x =>
            {
                x.ToTable(options.TablePrefix + "Texts", options.Schema);

                x.Property(p => p.CultureName)
                    .IsRequired()
                    .HasMaxLength(LanguageConsts.MaxCultureNameLength)
                    .HasColumnName(nameof(Text.CultureName));
                x.Property(p => p.Key)
                    .IsRequired()
                    .HasMaxLength(TextConsts.MaxKeyLength)
                    .HasColumnName(nameof(Text.Key));
                x.Property(p => p.Value)
                    .HasMaxLength(TextConsts.MaxValueLength)
                    .HasColumnName(nameof(Text.Value));

                x.ConfigureByConvention();

                x.HasIndex(p => p.Key);
            });
        }
    }
}
