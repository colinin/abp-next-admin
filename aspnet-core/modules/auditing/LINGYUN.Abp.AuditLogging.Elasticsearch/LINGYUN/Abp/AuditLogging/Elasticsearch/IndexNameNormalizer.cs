using Microsoft.Extensions.Options;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    public class IndexNameNormalizer : IIndexNameNormalizer, ISingletonDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly AbpAuditLoggingElasticsearchOptions _options;

        public IndexNameNormalizer(
            ICurrentTenant currentTenant, 
            IOptions<AbpAuditLoggingElasticsearchOptions> options)
        {
            _currentTenant = currentTenant;
            _options = options.Value;
        }

        public string NormalizeIndex(string index)
        {
            if (_currentTenant.IsAvailable)
            {
                return $"{_options.IndexPrefix}-{index}-{_currentTenant.Id:N}";
            }
            return _options.IndexPrefix.IsNullOrWhiteSpace()
                ? index
                : $"{_options.IndexPrefix}-{index}";
        }
    }
}
