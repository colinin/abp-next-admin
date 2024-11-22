using JetBrains.Annotations;
using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Feedbacks;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Packages;
using LINGYUN.Platform.Portal;
using LINGYUN.Platform.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Platform.EntityFrameworkCore;

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

            b.Property(p => p.Framework)
                .HasMaxLength(LayoutConsts.MaxFrameworkLength)
                .HasColumnName(nameof(Layout.Framework))
                .IsRequired();

            b.ConfigureRoute();
        });

        builder.Entity<Menu>(b =>
        {
            b.ToTable(options.TablePrefix + "Menus", options.Schema);

            b.ConfigureRoute();

            b.Property(p => p.Framework)
                .HasMaxLength(LayoutConsts.MaxFrameworkLength)
                .HasColumnName(nameof(Menu.Framework))
                .IsRequired();
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

        builder.Entity<UserFavoriteMenu>(x =>
        {
            x.ToTable(options.TablePrefix + "UserFavoriteMenus");

            x.Property(p => p.Framework)
                .HasMaxLength(LayoutConsts.MaxFrameworkLength)
                .HasColumnName(nameof(Menu.Framework))
                .IsRequired();
            x.Property(p => p.DisplayName)
                .HasMaxLength(RouteConsts.MaxDisplayNameLength)
                .HasColumnName(nameof(Route.DisplayName))
                .IsRequired();
            x.Property(p => p.Name)
                .HasMaxLength(RouteConsts.MaxNameLength)
                .HasColumnName(nameof(Route.Name))
                .IsRequired();
            x.Property(p => p.Path)
                .HasMaxLength(RouteConsts.MaxPathLength)
                .HasColumnName(nameof(Route.Path))
                .IsRequired();

            x.Property(p => p.Icon)
                .HasMaxLength(UserFavoriteMenuConsts.MaxIconLength)
                .HasColumnName(nameof(UserFavoriteMenu.Icon));
            x.Property(p => p.Color)
                .HasMaxLength(UserFavoriteMenuConsts.MaxColorLength)
                .HasColumnName(nameof(UserFavoriteMenu.Color));
            x.Property(p => p.AliasName)
                .HasMaxLength(UserFavoriteMenuConsts.MaxAliasNameLength)
                .HasColumnName(nameof(UserFavoriteMenu.AliasName));

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

        builder.Entity<Package>(x =>
        {
            x.ToTable(options.TablePrefix + "Packages", options.Schema);

            x.Property(p => p.Name)
                .IsRequired()
                .HasColumnName(nameof(Package.Name))
                .HasMaxLength(PackageConsts.MaxNameLength);
            x.Property(p => p.Note)
                .IsRequired()
                .HasColumnName(nameof(Package.Note))
                .HasMaxLength(PackageConsts.MaxNoteLength);
            x.Property(p => p.Version)
               .IsRequired()
               .HasColumnName(nameof(Package.Version))
               .HasMaxLength(PackageConsts.MaxVersionLength);

            x.Property(p => p.Description)
                .HasColumnName(nameof(Package.Description))
                .HasMaxLength(PackageConsts.MaxDescriptionLength);
            x.Property(p => p.Authors)
                .HasColumnName(nameof(Package.Authors))
                .HasMaxLength(PackageConsts.MaxAuthorsLength);

            x.ConfigureByConvention();

            x.HasIndex(i => new { i.Name, i.Version });

            x.HasMany(p => p.Blobs)
                .WithOne(q => q.Package)
                .HasPrincipalKey(pk => pk.Id)
                .HasForeignKey(fk => fk.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<PackageBlob>(x =>
        {
            x.ToTable(options.TablePrefix + "PackageBlobs", options.Schema);

            x.Property(p => p.Name)
                .IsRequired()
                .HasColumnName(nameof(PackageBlob.Name))
                .HasMaxLength(PackageBlobConsts.MaxNameLength);

            x.Property(p => p.SHA256)
                .HasColumnName(nameof(PackageBlob.SHA256))
                .HasMaxLength(PackageBlobConsts.MaxSHA256Length);
            x.Property(p => p.Url)
                .HasColumnName(nameof(PackageBlob.Url))
                .HasMaxLength(PackageBlobConsts.MaxUrlLength);
            x.Property(p => p.Summary)
                .HasColumnName(nameof(PackageBlob.Summary))
                .HasMaxLength(PackageBlobConsts.MaxSummaryLength);
            x.Property(p => p.Authors)
               .HasColumnName(nameof(PackageBlob.Authors))
               .HasMaxLength(PackageBlobConsts.MaxAuthorsLength);
            x.Property(p => p.License)
                .HasColumnName(nameof(PackageBlob.License))
                .HasMaxLength(PackageBlobConsts.MaxLicenseLength);
            x.Property(p => p.ContentType)
                .HasColumnName(nameof(PackageBlob.ContentType))
                .HasMaxLength(PackageBlobConsts.MaxContentTypeLength);

            x.ConfigureByConvention();

            x.HasIndex(i => new { i.PackageId, i.Name });
        });

        builder.Entity<Enterprise>(x =>
        {
            x.ToTable(options.TablePrefix + "Enterprises", options.Schema);

            x.Property(p => p.Name)
                .IsRequired()
                .HasColumnName(nameof(Enterprise.Name))
                .HasMaxLength(EnterpriseConsts.MaxNameLength);

            x.Property(p => p.EnglishName)
                .HasColumnName(nameof(Enterprise.EnglishName))
                .HasMaxLength(EnterpriseConsts.MaxEnglishNameLength);
            x.Property(p => p.Address)
                .HasColumnName(nameof(Enterprise.Address))
                .HasMaxLength(EnterpriseConsts.MaxAddressLength);
            x.Property(p => p.Logo)
                .HasColumnName(nameof(Enterprise.Logo))
                .HasMaxLength(EnterpriseConsts.MaxLogoLength);
            x.Property(p => p.LegalMan)
                .HasColumnName(nameof(Enterprise.LegalMan))
                .HasMaxLength(EnterpriseConsts.MaxLegalManLength);
            x.Property(p => p.TaxCode)
               .HasColumnName(nameof(Enterprise.TaxCode))
               .HasMaxLength(EnterpriseConsts.MaxTaxCodeLength);
            x.Property(p => p.OrganizationCode)
                .HasColumnName(nameof(Enterprise.OrganizationCode))
                .HasMaxLength(EnterpriseConsts.MaxOrganizationCodeLength);
            x.Property(p => p.RegistrationCode)
                .HasColumnName(nameof(Enterprise.RegistrationCode))
                .HasMaxLength(EnterpriseConsts.MaxRegistrationCodeLength);

            x.ConfigureByConvention();
        });

        builder.Entity<Feedback>(x =>
        {
            x.ToTable(options.TablePrefix + "Feedbacks", options.Schema);

            x.Property(p => p.Category)
                .IsRequired()
                .HasColumnName(nameof(Feedback.Category))
                .HasMaxLength(FeedbackConsts.MaxCategoryLength);
            x.Property(p => p.Content)
               .IsRequired()
               .HasColumnName(nameof(Feedback.Content))
               .HasMaxLength(FeedbackConsts.MaxContentLength);

            x.HasMany(p => p.Attachments)
             .WithOne()
             .HasForeignKey(fk => fk.FeedbackId)
             .IsRequired();
            x.HasMany(p => p.Comments)
             .WithOne()
             .HasForeignKey(fk => fk.FeedbackId)
             .IsRequired();

            x.ConfigureByConvention();
        });
        builder.Entity<FeedbackComment>(x =>
        {
            x.ToTable(options.TablePrefix + "FeedbackComments", options.Schema);

            x.Property(p => p.Capacity)
                .IsRequired()
                .HasColumnName(nameof(FeedbackComment.Capacity))
                .HasMaxLength(FeedbackCommentConsts.MaxCapacityLength);
            x.Property(p => p.Content)
                .IsRequired()
                .HasColumnName(nameof(FeedbackComment.Content))
                .HasMaxLength(FeedbackCommentConsts.MaxContentLength);

            x.ConfigureByConvention();
        });
        builder.Entity<FeedbackAttachment>(x =>
        {
            x.ToTable(options.TablePrefix + "FeedbackAttachments", options.Schema);

            x.Property(p => p.Name)
                .IsRequired()
                .HasColumnName(nameof(FeedbackAttachment.Name))
                .HasMaxLength(FeedbackAttachmentConsts.MaxNameLength);
            x.Property(p => p.Url)
               .IsRequired()
               .HasColumnName(nameof(FeedbackAttachment.Url))
               .HasMaxLength(FeedbackAttachmentConsts.MaxUrlLength);

            x.ConfigureByConvention();
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
