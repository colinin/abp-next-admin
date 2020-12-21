using JetBrains.Annotations;
using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    public static class PlatformDbContextModelBuilderExtensions
    {
        public static void ConfigurePlatform(
           this ModelBuilder builder,
           Action<PlatformModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PlatformModelBuilderConfigurationOptions(
                PlatformDbProperties.DbTablePrefix,
                PlatformDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Layout>(b =>
            {
                b.ToTable(options.TablePrefix + "Layouts", options.Schema);

                b.ConfigureRoute();
            });

            builder.Entity<Menu>(b =>
            {
                b.ToTable(options.TablePrefix + "Menus", options.Schema);

                b.ConfigureRoute();

                b.Property(p => p.Component)
                    .HasMaxLength(MenuConsts.MaxComponentLength)
                    .HasColumnName(nameof(Menu.Component))
                    .IsRequired();
                b.Property(p => p.Code)
                    .HasMaxLength(MenuConsts.MaxCodeLength)
                    .HasColumnName(nameof(Menu.Code))
                    .IsRequired();
            });

            builder.Entity<RoleMenu>(x =>
            {
                x.ToTable(options.TablePrefix + "RoleMenus");

                x.Property(p => p.RoleName)
                    .IsRequired()
                    .HasMaxLength(RoleRouteConsts.MaxRoleNameLength)
                    .HasColumnName(nameof(RoleMenu.RoleName));

                x.ConfigureByConvention();

                x.HasIndex(i => new { i.RoleName, i.MenuId });
            });

            builder.Entity<UserMenu>(x =>
            {
                x.ToTable(options.TablePrefix + "UserMenus");

                x.ConfigureByConvention();

                x.HasIndex(i => new { i.UserId, i.MenuId });
            });

            builder.Entity<Data>(x =>
            {
                x.ToTable(options.TablePrefix + "Datas");

                x.Property(p => p.Code)
                    .HasMaxLength(DataConsts.MaxCodeLength)
                    .HasColumnName(nameof(Data.Code))
                    .IsRequired();
                x.Property(p => p.Name)
                    .HasMaxLength(DataConsts.MaxNameLength)
                    .HasColumnName(nameof(Data.Name))
                    .IsRequired();
                x.Property(p => p.DisplayName)
                   .HasMaxLength(DataConsts.MaxDisplayNameLength)
                   .HasColumnName(nameof(Data.DisplayName))
                   .IsRequired();
                x.Property(p => p.Description)
                    .HasMaxLength(DataConsts.MaxDescriptionLength)
                    .HasColumnName(nameof(Data.Description));

                x.ConfigureByConvention();

                x.HasMany(p => p.Items)
                    .WithOne()
                    .HasForeignKey(fk => fk.DataId)
                    .IsRequired();

                x.HasIndex(i => new { i.Name });
            });

            builder.Entity<DataItem>(x =>
            {
                x.ToTable(options.TablePrefix + "DataItems");

                x.Property(p => p.DefaultValue)
                    .HasMaxLength(DataItemConsts.MaxValueLength)
                    .HasColumnName(nameof(DataItem.DefaultValue));
                x.Property(p => p.Name)
                    .HasMaxLength(DataItemConsts.MaxNameLength)
                    .HasColumnName(nameof(DataItem.Name))
                    .IsRequired();
                x.Property(p => p.DisplayName)
                   .HasMaxLength(DataItemConsts.MaxDisplayNameLength)
                   .HasColumnName(nameof(DataItem.DisplayName))
                   .IsRequired();
                x.Property(p => p.Description)
                    .HasMaxLength(DataItemConsts.MaxDescriptionLength)
                    .HasColumnName(nameof(DataItem.Description));

                x.Property(p => p.AllowBeNull).HasDefaultValue(true);

                x.ConfigureByConvention();

                x.HasIndex(i => new { i.Name });
            });


            builder.Entity<AppVersion>(x =>
            {
                x.ToTable(options.TablePrefix + "Version", options.Schema);

                x.Property(p => p.Title)
                    .IsRequired()
                    .HasColumnName(nameof(AppVersion.Title))
                    .HasMaxLength(AppVersionConsts.MaxTitleLength);
                x.Property(p => p.Version)
                    .IsRequired()
                    .HasColumnName(nameof(AppVersion.Version))
                    .HasMaxLength(AppVersionConsts.MaxVersionLength);

                x.Property(p => p.Description)
                    .HasColumnName(nameof(AppVersion.Description))
                    .HasMaxLength(AppVersionConsts.MaxDescriptionLength);

                x.ConfigureByConvention();

                x.HasIndex(i => i.Version);

                x.HasMany(p => p.Files)
                    .WithOne(q => q.AppVersion)
                    .HasPrincipalKey(pk => pk.Id)
                    .HasForeignKey(fk => fk.AppVersionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<VersionFile>(x =>
            {
                x.ToTable(options.TablePrefix + "VersionFile", options.Schema);

                x.Property(p => p.Name)
                    .IsRequired()
                    .HasColumnName(nameof(VersionFile.Name))
                    .HasMaxLength(VersionFileConsts.MaxNameLength);
                x.Property(p => p.SHA256)
                    .IsRequired()
                    .HasColumnName(nameof(VersionFile.SHA256))
                    .HasMaxLength(VersionFileConsts.MaxSHA256Length);
                x.Property(p => p.Version)
                    .IsRequired()
                    .HasColumnName(nameof(VersionFile.Version))
                    .HasMaxLength(VersionFileConsts.MaxVersionLength);

                x.Property(p => p.Path)
                    .HasColumnName(nameof(VersionFile.Path))
                    .HasMaxLength(VersionFileConsts.MaxPathLength);

                x.ConfigureAudited();
                x.ConfigureMultiTenant();

                x.HasIndex(i => new { i.Path, i.Name, i.Version }).IsUnique();
                
            });
        }

        public static EntityTypeBuilder<TRoute> ConfigureRoute<TRoute>(
            this EntityTypeBuilder<TRoute> builder)
            where TRoute : Route
        {
            builder
                .Property(p => p.DisplayName)
                .HasMaxLength(RouteConsts.MaxDisplayNameLength)
                .HasColumnName(nameof(Route.DisplayName))
                .IsRequired();
            builder
                .Property(p => p.Name)
                .HasMaxLength(RouteConsts.MaxNameLength)
                .HasColumnName(nameof(Route.Name))
                .IsRequired();
            builder
                .Property(p => p.Path)
                .HasMaxLength(RouteConsts.MaxPathLength)
                .HasColumnName(nameof(Route.Path));
            builder
                .Property(p => p.Redirect)
                .HasMaxLength(RouteConsts.MaxRedirectLength)
                .HasColumnName(nameof(Route.Redirect));

            builder.ConfigureByConvention();

            return builder;
        }

        public static OwnedNavigationBuilder<TEntity, TRoute> ConfigureRoute<TEntity, TRoute>(
            [NotNull] this OwnedNavigationBuilder<TEntity, TRoute> builder,
            [CanBeNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            where TEntity : class
            where TRoute : Route
        {
            builder.ToTable(tablePrefix + "Routes", schema);

            builder
                .Property(p => p.DisplayName)
                .HasMaxLength(RouteConsts.MaxDisplayNameLength)
                .HasColumnName(nameof(Route.DisplayName))
                .IsRequired();
            builder
                .Property(p => p.Name)
                .HasMaxLength(RouteConsts.MaxNameLength)
                .HasColumnName(nameof(Route.Name))
                .IsRequired();
            builder
                .Property(p => p.Path)
                .HasMaxLength(RouteConsts.MaxPathLength)
                .HasColumnName(nameof(Route.Path));
            builder
                .Property(p => p.Redirect)
                .HasMaxLength(RouteConsts.MaxRedirectLength)
                .HasColumnName(nameof(Route.Redirect));

            return builder;
        }
    }
}
