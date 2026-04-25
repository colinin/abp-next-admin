using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchAuditLogWriter : IAuditLogWriter, ITransientDependency
{
    private readonly IAuditLogInfoToAuditLogConverter _auditLogConverter;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IIndexNameNormalizer _indexNameNormalizer;

    private readonly ILogger<ElasticsearchAuditLogWriter> _logger;

    public ElasticsearchAuditLogWriter(
        IAuditLogInfoToAuditLogConverter auditLogConverter,
        IElasticsearchClientFactory clientFactory,
        IIndexNameNormalizer indexNameNormalizer,
        ILogger<ElasticsearchAuditLogWriter> logger)
    {
        _auditLogConverter = auditLogConverter;
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _logger = logger;
    }

    public async virtual Task WriteAsync(AuditLogInfo auditLogInfo, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();
        var auditLog = await _auditLogConverter.ConvertAsync(auditLogInfo);
        var response = await client.IndexAsync(
            auditLog,
            dsl => dsl.Index(CreateIndex())
                      .Id(auditLog.Id),
            cancellationToken);

        if (!response.IsValidResponse)
        {
            _logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditLog.ToString());
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

    public async virtual Task BulkWriteAsync(IEnumerable<AuditLogInfo> auditLogInfos, CancellationToken cancellationToken = default)
    {
        if (!auditLogInfos.Any())
        {
            return;
        }

        var client = _clientFactory.Create();

        var indexName = CreateIndex();

        var bulkOperations = new List<BulkOperation>();
        foreach (var auditLogInfo in auditLogInfos)
        {
            var auditLog = await _auditLogConverter.ConvertAsync(auditLogInfo);
            bulkOperations.Add(new BulkCreateOperation<AuditLog>(auditLog)
            {
                Id = auditLog.Id.ToString()
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
            await HandleBulkErrorsAsync(response, auditLogInfos);
        }
    }

    private Task HandleBulkErrorsAsync(BulkResponse response, IEnumerable<AuditLogInfo> auditLogInfos)
    {
        foreach (var itemWithError in response.ItemsWithErrors)
        {
            _logger.LogError($"Failed to write audit log: {itemWithError.Error?.Reason}");
        }

        foreach (var auditLog in auditLogInfos)
        {
            _logger.LogInformation(auditLog.ToString());
        }

        return Task.CompletedTask;
    }

    protected virtual string CreateIndex()
    {
        return _indexNameNormalizer.NormalizeIndex("audit-log");
    }
}
