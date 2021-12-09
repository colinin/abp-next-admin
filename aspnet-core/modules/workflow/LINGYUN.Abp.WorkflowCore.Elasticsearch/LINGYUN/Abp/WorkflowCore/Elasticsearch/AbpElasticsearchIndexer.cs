using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.WorkflowCore.Elasticsearch.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.Search;

namespace LINGYUN.Abp.WorkflowCore.Elasticsearch
{
    public class AbpElasticsearchIndexer : ISearchIndex
    {
        private IElasticClient _client;

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly AbpWorkflowCoreElasticsearchOptions _options;
        private readonly ILogger<AbpElasticsearchIndexer> _logger;

        public AbpElasticsearchIndexer(
            ILogger<AbpElasticsearchIndexer> logger,
            IOptions<AbpWorkflowCoreElasticsearchOptions> options,
            IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _logger = logger;
            _options = options.Value;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public async Task IndexWorkflow(WorkflowInstance workflow)
        {
            if (_client == null)
                throw new InvalidOperationException("Not started");

            var denormModel = WorkflowSearchModel.FromWorkflowInstance(workflow);

            var result = await _client.IndexAsync(
                denormModel, 
                idx => idx.Index(_options.IndexFormat));

            if (!result.ApiCall.Success)
            {
                _logger.LogError(default(EventId), result.ApiCall.OriginalException, $"Failed to index workflow {workflow.Id}");
                throw new AbpException($"Failed to index workflow {workflow.Id}", result.ApiCall.OriginalException);
            }
        }

        public async Task<Page<WorkflowSearchResult>> Search(string terms, int skip, int take, params SearchFilter[] filters)
        {
            if (_client == null)
                throw new InvalidOperationException("Not started");

            var result = await _client.SearchAsync<WorkflowSearchModel>(s => s
                .Index(_options.IndexFormat)
                .Skip(skip)
                .Take(take)
                .MinScore(!string.IsNullOrEmpty(terms) ? 0.1 : 0)
                .Query(query => query
                    .Bool(b => b
                        .Filter(BuildFilterQuery(filters))
                        .Should(
                            should => should.Match(t => t.Field(f => f.Reference).Query(terms).Boost(1.2)),
                            should => should.Match(t => t.Field(f => f.DataTokens).Query(terms).Boost(1.1)),
                            should => should.Match(t => t.Field(f => f.WorkflowDefinitionId).Query(terms).Boost(0.9)),
                            should => should.Match(t => t.Field(f => f.Status).Query(terms).Boost(0.9)),
                            should => should.Match(t => t.Field(f => f.Description).Query(terms))
                        )
                    )
                )
            );

            return new Page<WorkflowSearchResult>
            {
                Total = result.Total,
                Data = result.Hits.Select(x => x.Source).Select(x => x.ToSearchResult()).ToList()
            };
        }

        public async Task Start()
        {
            _client = _elasticsearchClientFactory.Create();
            var nodeInfo = await _client.Nodes.InfoAsync();
            if (nodeInfo.Nodes.Values.Any(x => Convert.ToUInt32(x.Version.Split('.')[0]) < 6))
                throw new NotSupportedException("Elasticsearch verison 6 or greater is required");

            var exists = await _client.Indices.ExistsAsync(_options.IndexFormat);
            if (!exists.Exists)
            {
                await _client.Indices.CreateAsync(_options.IndexFormat);
            }
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        private List<Func<QueryContainerDescriptor<WorkflowSearchModel>, QueryContainer>> BuildFilterQuery(SearchFilter[] filters)
        {
            var result = new List<Func<QueryContainerDescriptor<WorkflowSearchModel>, QueryContainer>>();

            foreach (var filter in filters)
            {
                var field = new Field(filter.Property);
                if (filter.IsData)
                {
                    Expression<Func<WorkflowSearchModel, object>> dataExpr = x => x.Data[filter.DataType.FullName];
                    var fieldExpr = Expression.Convert(filter.Property, typeof(Func<object, object>));
                    field = new Field(Expression.Lambda(Expression.Invoke(fieldExpr, dataExpr), Expression.Parameter(typeof(WorkflowSearchModel))));
                }

                switch (filter)
                {
                    case ScalarFilter f:
                        result.Add(x => x.Match(t => t.Field(field).Query(Convert.ToString(f.Value))));
                        break;
                    case DateRangeFilter f:
                        if (f.BeforeValue.HasValue)
                            result.Add(x => x.DateRange(t => t.Field(field).LessThan(f.BeforeValue)));
                        if (f.AfterValue.HasValue)
                            result.Add(x => x.DateRange(t => t.Field(field).GreaterThan(f.AfterValue)));
                        break;
                    case NumericRangeFilter f:
                        if (f.LessValue.HasValue)
                            result.Add(x => x.Range(t => t.Field(field).LessThan(f.LessValue)));
                        if (f.GreaterValue.HasValue)
                            result.Add(x => x.Range(t => t.Field(field).GreaterThan(f.GreaterValue)));
                        break;
                }
            }

            return result;
        }
    }
}
