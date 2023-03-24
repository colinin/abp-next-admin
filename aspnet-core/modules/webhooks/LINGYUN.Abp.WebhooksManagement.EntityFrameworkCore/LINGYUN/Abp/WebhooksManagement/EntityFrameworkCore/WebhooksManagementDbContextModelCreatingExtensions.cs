using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

public static class WebhooksManagementDbContextModelCreatingExtensions
{
    public static void ConfigureWebhooksManagement(
        this ModelBuilder builder,
        Action<WebhooksManagementModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new WebhooksManagementModelBuilderConfigurationOptions(
            WebhooksManagementDbProperties.DbTablePrefix,
            WebhooksManagementDbProperties.DbSchema
        );
        optionsAction?.Invoke(options);

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<WebhookEventRecord>(b =>
        {
            b.ToTable(options.TablePrefix + "Events", options.Schema);

            b.Property(p => p.WebhookName)
             .IsRequired()
             .HasColumnName(nameof(WebhookEventRecord.WebhookName))
             .HasMaxLength(WebhookEventRecordConsts.MaxWebhookNameLength);
            b.Property(p => p.Data)
             .HasColumnName(nameof(WebhookEventRecord.Data))
             .HasMaxLength(WebhookEventRecordConsts.MaxDataLength);

            b.ConfigureByConvention();
        });

        builder.Entity<WebhookSendRecord>(b =>
        {
            b.ToTable(options.TablePrefix + "SendAttempts", options.Schema);

            b.Property(p => p.Response)
             .HasColumnName(nameof(WebhookSendRecord.Response))
             .HasMaxLength(WebhookSendRecordConsts.MaxResponseLength);
            b.Property(p => p.RequestHeaders)
             .HasColumnName(nameof(WebhookSendRecord.RequestHeaders))
             .HasMaxLength(WebhookSendRecordConsts.MaxHeadersLength);
            b.Property(p => p.ResponseHeaders)
             .HasColumnName(nameof(WebhookSendRecord.ResponseHeaders))
             .HasMaxLength(WebhookSendRecordConsts.MaxHeadersLength);

            b.ConfigureByConvention();

            b.HasOne(p => p.WebhookEvent)
             .WithOne()
             .HasForeignKey<WebhookSendRecord>(fk => fk.WebhookEventId)
             .HasPrincipalKey<WebhookEventRecord>(pk => pk.Id );

            b.HasIndex(p => p.WebhookEventId)
             .IsUnique(false);
        });

        builder.Entity<WebhookSubscription>(b =>
        {
            b.ToTable(options.TablePrefix + "Subscriptions", options.Schema);

            b.Property(p => p.WebhookUri)
             .IsRequired()
             .HasColumnName(nameof(WebhookSubscription.WebhookUri))
             .HasMaxLength(WebhookSubscriptionConsts.MaxWebhookUriLength);

            b.Property(p => p.Secret)
             .HasColumnName(nameof(WebhookSubscription.Secret))
             .HasMaxLength(WebhookSubscriptionConsts.MaxSecretLength);
            b.Property(p => p.Webhooks)
             .HasColumnName(nameof(WebhookSubscription.Webhooks))
             .HasMaxLength(WebhookSubscriptionConsts.MaxWebhooksLength);
            b.Property(p => p.Headers)
             .HasColumnName(nameof(WebhookSubscription.Headers))
             .HasMaxLength(WebhookSubscriptionConsts.MaxHeadersLength);
            b.Property(p => p.Description)
             .HasColumnName(nameof(WebhookSubscription.Description))
             .HasMaxLength(WebhookSubscriptionConsts.MaxDescriptionLength);

            b.ConfigureByConvention();
        });

        builder.Entity<WebhookGroupDefinitionRecord>(b =>
        {
            b.ToTable(options.TablePrefix + "WebhookGroups", options.Schema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).HasMaxLength(WebhookGroupDefinitionRecordConsts.MaxNameLength).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(WebhookGroupDefinitionRecordConsts.MaxDisplayNameLength).IsRequired();

            b.HasIndex(x => new { x.Name }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<WebhookDefinitionRecord>(b =>
        {
            b.ToTable(options.TablePrefix + "Webhooks", options.Schema);

            b.ConfigureByConvention();

            b.Property(x => x.GroupName).HasMaxLength(WebhookGroupDefinitionRecordConsts.MaxNameLength).IsRequired();
            b.Property(x => x.Name).HasMaxLength(WebhookDefinitionRecordConsts.MaxNameLength).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(WebhookDefinitionRecordConsts.MaxDisplayNameLength).IsRequired();
            b.Property(x => x.Description).HasMaxLength(WebhookDefinitionRecordConsts.MaxDescriptionLength);
            b.Property(x => x.RequiredFeatures).HasMaxLength(WebhookDefinitionRecordConsts.MaxRequiredFeaturesLength);

            b.HasIndex(x => new { x.Name }).IsUnique();
            b.HasIndex(x => new { x.GroupName });

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<WebhooksManagementDbContext>();
    }
}
