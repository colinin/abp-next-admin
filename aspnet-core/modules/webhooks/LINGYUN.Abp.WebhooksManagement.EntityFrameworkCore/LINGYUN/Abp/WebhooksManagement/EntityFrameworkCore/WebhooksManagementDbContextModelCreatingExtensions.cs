using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;

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
    }
}
