using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Auditing;
public class AspNetCoreRecordHeaderAuditLogContributor : AuditLogContributor, ITransientDependency
{
    private const string HttpHeaderRecordKey = "HttpHeaders";

    public AspNetCoreRecordHeaderAuditLogContributor()
    {
    }

    public override void PreContribute(AuditLogContributionContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreAuditingHeaderOptions>>();
        if (!options.Value.IsEnabled)
        {
            return;
        }

        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null)
        {
            return;
        }

        if (context.AuditInfo.HasProperty(HttpHeaderRecordKey))
        {
            return;
        }

        var headerRcords = new Dictionary<string, string>();
        var httpHeaders = httpContext.Request.Headers.ToImmutableDictionary();

        foreach (var headerKey in options.Value.HttpHeaders)
        {
            if (httpHeaders.TryGetValue(headerKey, out var headers))
            {
                headerRcords[headerKey] = headers.JoinAsString(";");
            }
        }

        context.AuditInfo.SetProperty(HttpHeaderRecordKey, headerRcords);
    }
}
