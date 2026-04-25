using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchSecurityLogWriter : ISecurityLogWriter, ITransientDependency
{
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IGuidGenerator _guidGenerator;

    private readonly ILogger<ElasticsearchSecurityLogWriter> _logger;

    public ElasticsearchSecurityLogWriter(
        IElasticsearchClientFactory clientFactory,
        IIndexNameNormalizer indexNameNormalizer,
        IGuidGenerator guidGenerator,
        ILogger<ElasticsearchSecurityLogWriter> logger)
    {
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    public async virtual Task WriteAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var securityLog = new SecurityLog(
            _guidGenerator.Create(),
            securityLogInfo);

        var response = await client.IndexAsync(
            securityLog,
            dsl => dsl.Index(CreateIndex())
                      .Id(securityLog.Id),
            cancellationToken);

        if (!response.IsValidResponse)
        {
            _logger.LogWarning("Could not save the security log object: " + Environment.NewLine + securityLog.ToString());
            if (response.TryGetOriginalException(out var ex))
            {
                _logger.LogWarning(ex, ex.Message);
            }
            else if (response.ElasticsearchServerError != null)
            {
                _logger.LogWarning(response.ElasticsearchServerError.ToString());
            }
        }
    }

    public async virtual Task BulkWriteAsync(IEnumerable<SecurityLogInfo> securityLogInfos, CancellationToken cancellationToken = default)
    {
        if (!securityLogInfos.Any())
        {
            return;
        }

        var client = _clientFactory.Create();

        var indexName = CreateIndex();

        var bulkOperations = new List<BulkOperation>();
        foreach (var securityLogInfo in securityLogInfos)
        {
            var securityLog = new SecurityLog(
                _guidGenerator.Create(),
                securityLogInfo);

            bulkOperations.Add(new BulkCreateOperation<SecurityLog>(securityLog)
            {
                Id = securityLog.Id.ToString()
            });
        }

        var bulkRequest = new BulkRequest
        {
            Operations = [.. bulkOperations],
            Index = indexName
        };

        var response = await client.BulkAsync(bulkRequest, cancellationToken);

        if (!response.IsValidResponse)
        {
            await HandleBulkErrorsAsync(response, securityLogInfos);
        }
    }

    private Task HandleBulkErrorsAsync(BulkResponse response, IEnumerable<SecurityLogInfo> securityLogInfos)
    {
        foreach (var itemWithError in response.ItemsWithErrors)
        {
            _logger.LogError($"Failed to write security log: {itemWithError.Error?.Reason}");
        }

        foreach (var auditLog in securityLogInfos)
        {
            _logger.LogInformation(auditLog.ToString());
        }

        return Task.CompletedTask;
    }

    protected virtual string CreateIndex()
    {
        return _indexNameNormalizer.NormalizeIndex("security-log");
    }
}
