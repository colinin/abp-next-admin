using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Transport.Diagnostics.Auditing;
using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Elasticsearch;

[DependsOn(
    typeof(AbpTestsBaseModule),
    typeof(AbpElasticsearchModule))]
public class AbpElasticsearchTestModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private const string UserSecretsId = "D4327320-718E-4A7F-A987-85838EDD8675";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () => await OnPostApplicationInitializationAsync(context));
    }

    public async override Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var clientFactory = context.ServiceProvider.GetRequiredService<IElasticsearchClientFactory>();
        var client = clientFactory.Create();
        var indexPatterns = new[] { TestDocumentIndexNames.Index + "*" };
        var indexTemplateName = TestDocumentIndexNames.Index + "-generic";
        var dateTimeFormat = "yyyy-MM-dd HH:mm:ss||strict_date_optional_time||epoch_millis";

        var indexTemplateExists = await client.Indices.ExistsIndexTemplateAsync(indexTemplateName, _cancellationTokenSource.Token);
        if (indexTemplateExists.Exists)
        {
            await client.Indices.DeleteIndexTemplateAsync(indexTemplateName, _cancellationTokenSource.Token);
        }
        var putTemplateResponse = await client.Indices.PutIndexTemplateAsync(indexTemplateName, setup =>
        {
            setup.IndexPatterns(indexPatterns);
            setup.Priority(100);
            setup.Version(1);
            setup.Template(template =>
            {
                template.Settings(new IndexSettings()
                {
                    NumberOfReplicas = 1,
                    NumberOfShards = 3,
                    Mapping = new MappingLimitSettings
                    {
                        TotalFields = new MappingLimitSettingsTotalFields
                        {
                            Limit = 1000,
                        },
                        NestedFields = new MappingLimitSettingsNestedFields
                        {
                            Limit = 50,
                        },
                        Depth = new MappingLimitSettingsDepth
                        {
                            Limit = 10,
                        },
                    }
                });
                template.Mappings(mp => mp
                    .Dynamic(DynamicMapping.False)
                    .Properties<TestDocument>(pd =>
                    {
                        pd.IntegerNumber(k => k.Id);
                        pd.Text(k => k.Name, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(100))));
                        pd.Text(t => t.Description);
                        pd.IntegerNumber(k => k.Age);
                        pd.DoubleNumber(k => k.Salary);
                        pd.Boolean(k => k.IsActive);
                        pd.Date(k => k.CreatedTime, d => d.Format(dateTimeFormat));
                        pd.Date(k => k.UpdatedTime, d => d.Format(dateTimeFormat));
                        pd.ByteNumber(k => k.Status);
                        pd.ByteNumber(k => k.NullableStatus);
                        pd.Text(k => k.StringValueStatus, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(100))));
                        pd.Keyword(k => k.Tags);
                        pd.Wildcard(k => k.Exceptions);
                        pd.Nested(n => n.Items, np =>
                        {
                            np.Dynamic(DynamicMapping.False);
                            np.Properties(npd =>
                            {
                                npd.IntegerNumber(nameof(SubDocument.Id));
                                npd.Text(nameof(SubDocument.Name), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(100))));
                                npd.DoubleNumber(nameof(SubDocument.Price));
                            });
                        });
                        pd.Nested(n => n.Address, np =>
                        {
                            np.Dynamic(DynamicMapping.False);
                            np.Properties(npd =>
                            {
                                npd.Text(nameof(Address.City), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(255))));
                                npd.Text(nameof(Address.Street));
                            });
                        });
                    }));
            });
        }, _cancellationTokenSource.Token);

        await client.Indices.DeleteAsync(TestDocumentIndexNames.Index, _cancellationTokenSource.Token);
        await client.IndexAsync(
            new TestDocument(),
            dsl => dsl.Index(TestDocumentIndexNames.Index),
            _cancellationTokenSource.Token);
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(async () => await OnApplicationShutdownAsync(context));
    }

    public async override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        var clientFactory = context.ServiceProvider.GetRequiredService<IElasticsearchClientFactory>();
        var client = clientFactory.Create();
        var indexTemplateName = TestDocumentIndexNames.Index + "-generic";
        var indexTemplateExists = await client.Indices.ExistsIndexTemplateAsync(indexTemplateName, _cancellationTokenSource.Token);
        if (indexTemplateExists.Exists)
        {
            await client.Indices.DeleteIndexTemplateAsync(indexTemplateName, _cancellationTokenSource.Token);
        }
        await client.Indices.DeleteAsync(TestDocumentIndexNames.Index, _cancellationTokenSource.Token);

        _cancellationTokenSource.Cancel();
    }
}
