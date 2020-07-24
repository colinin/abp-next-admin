using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;
using Microsoft.EntityFrameworkCore;
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

            builder.Entity<Route>(x =>
            {
                x.ToTable(options.TablePrefix + "Route");

                x.Property(p => p.Code)
                    .IsRequired()
                    .HasMaxLength(RouteConsts.MaxCodeLength)
                    .HasColumnName(nameof(Route.Code));
                x.Property(p => p.DisplayName)
                    .IsRequired()
                    .HasMaxLength(RouteConsts.MaxDisplayNameLength)
                    .HasColumnName(nameof(Route.DisplayName));
                x.Property(p => p.LinkUrl)
                    .IsRequired()
                    .HasMaxLength(RouteConsts.MaxLinkUrlLength)
                    .HasColumnName(nameof(Route.LinkUrl));
                x.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(RouteConsts.MaxNameLength)
                    .HasColumnName(nameof(Route.Name));

                x.Property(p => p.Icon)
                    .HasMaxLength(RouteConsts.MaxIconLength)
                    .HasColumnName(nameof(Route.Icon));
                x.Property(p => p.FullName)
                   .HasMaxLength(RouteConsts.MaxFullNameLength)
                   .HasColumnName(nameof(Route.FullName));
                x.Property(p => p.Description)
                    .HasMaxLength(RouteConsts.MaxDescriptionLength)
                    .HasColumnName(nameof(Route.Description));

                x.ConfigureByConvention();

                x.HasMany<Route>().WithOne().HasForeignKey(p => p.ParentId);

                x.HasIndex(i => i.Code);
            });

            builder.Entity<RoleRoute>(x =>
            {
                x.ToTable(options.TablePrefix + "RoleRoute");

                x.Property(p => p.RoleName)
                    .IsRequired()
                    .HasMaxLength(RoleRouteConsts.MaxRoleNameLength)
                    .HasColumnName(nameof(RoleRoute.RoleName));

                x.ConfigureMultiTenant();

                x.HasIndex(i => new { i.RoleName, i.RouteId });
            });

            builder.Entity<UserRoute>(x =>
            {
                x.ToTable(options.TablePrefix + "UserRoute");

                x.ConfigureMultiTenant();

                x.HasIndex(i => new { i.UserId, i.RouteId });
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

                x.ConfigureAudited();
                x.ConfigureMultiTenant();

                x.HasIndex(i => new { i.Name, i.Version });
            });
        }
    }
}
