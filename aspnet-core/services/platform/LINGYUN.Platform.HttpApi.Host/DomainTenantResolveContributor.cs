using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Text.Formatting;

namespace LINGYUN.Platform
{
    public class DomainTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Domain";

        public override string Name => ContributorName;

        private static readonly string[] ProtocolPrefixes = { "http://", "https://" };

        private readonly string _domainFormat;

        public DomainTenantResolveContributor(string domainFormat)
        {
            _domainFormat = domainFormat.RemovePreFix(ProtocolPrefixes);
        }

        protected override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            if (!httpContext.Request.Host.HasValue)
            {
                return Task.FromResult<string>(null);
            }

            var hostName = httpContext.Request.Host.Value.RemovePreFix(ProtocolPrefixes);
            var extractResult = FormattedStringValueExtracter.Extract(hostName, _domainFormat, ignoreCase: true);

            context.Handled = true;

            return Task.FromResult(extractResult.IsMatch ? extractResult.Matches[0].Value : null);
        }
    }
}
