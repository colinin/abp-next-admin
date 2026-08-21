using Elastic.Clients.Elasticsearch;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.Threading;
using Xunit;

namespace LINGYUN.Abp.Elasticsearch;

public abstract class ExpressionQueryService_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    private List<TestDocument> _documents = new List<TestDocument>();
    protected IExpressionQueryService ExpressionQueryService { get; }
    protected ExpressionQueryService_Tests()
    {
        ExpressionQueryService = GetRequiredService<IExpressionQueryService>();
    }

    #region 索引初始化

    protected override void AfterInitialize()
    {
        AsyncHelper.RunSync(async () => await ApplicationInitializationAsync());
    }

    public override void Dispose()
    {
        AsyncHelper.RunSync(async () => await ApplicationShutdownAsync());
    }

    protected async virtual Task ApplicationInitializationAsync()
    {
        var clientFactory = GetRequiredService<IElasticsearchClientFactory>();
        var client = clientFactory.Create();

        _documents.AddRange(new[]
        {
            new TestDocument
            {
                Name = "Name1",
                Age = 20,
                Address = new Address
                {
                    City = "HANGZHOU",
                },
                CreatedTime = new DateTime(2026, 8, 1, 0, 0, 0),
                Status = TestEnum.Active,
                StringValueStatus = TestEnum.Pending,
                IsActive = true,
                Id = 1,
                Salary = 3m,
            },
            new TestDocument
            {
                Name = "Name2",
                Age = 10,
                Address = new Address
                {
                    City = "GUANGZHOU",
                },
                CreatedTime = new DateTime(2026, 5, 1, 0, 0, 0),
                Status = TestEnum.Inactive,
                StringValueStatus = TestEnum.Active,
                IsActive = false,
                Id = 2,
                Salary = 7m,
                Tags = new List<string>{ "B" },
            },
            new TestDocument
            {
                Name = "Test1",
                Age = 30,
                Address = new Address
                {
                    City = "BEIJING",
                },
                CreatedTime = new DateTime(2026, 3, 1, 0, 0, 0),
                Status = TestEnum.Pending,
                StringValueStatus = TestEnum.Inactive,
                IsActive = true,
                Id = 3,
                Salary = 10m,
                Tags = new List<string> { "B", "C" },
            },
        });

        await client.BulkAsync(b =>
            b.Index(TestDocumentIndexNames.Index)
             .Refresh(Refresh.WaitFor)
             .IndexMany(_documents));
    }

    protected async virtual Task ApplicationShutdownAsync()
    {
        var clientFactory = GetRequiredService<IElasticsearchClientFactory>();
        var client = clientFactory.Create();

        await client.Indices.DeleteAsync(TestDocumentIndexNames.Index);
    }

    #endregion

    [Fact]
    public async Task Should_Get_Count()
    {
        (await ExpressionQueryService.GetCountAsync<TestDocument>(
            TestDocumentIndexNames.Index,
            x => x.Name!.StartsWith("Name") && x.Salary <= 10m)).ShouldBe(2);

        (await ExpressionQueryService.GetCountAsync<TestDocument>(
            TestDocumentIndexNames.Index,
            x => !string.IsNullOrWhiteSpace(x.Name) && x.Tags != null && x.Tags.Contains("B"))).ShouldBe(2);

        (await ExpressionQueryService.GetCountAsync<TestDocument>(
            TestDocumentIndexNames.Index,
            x =>
                x.Name != null && x.Name.Contains("1") &&
                (x.Status == TestEnum.Active || x.Status == TestEnum.Pending))).ShouldBe(2);
    }

    [Fact]
    public async Task Should_Get_List()
    {
        var list1 = await ExpressionQueryService.GetListAsync<TestDocument>(
            TestDocumentIndexNames.Index,
            x => x.Name!.StartsWith("Name") && x.Salary <= 10m);
        list1.Count.ShouldBe(2);
        list1[0].Name.ShouldBe("Name1");
        list1[0].Age.ShouldBe(20);
        list1[0].Status.ShouldBe(TestEnum.Active);
        list1[0].StringValueStatus.ShouldBe(TestEnum.Pending);
        list1[0].IsActive.ShouldBeTrue();
        list1[0].Tags.ShouldBeNull();
        list1[0].Address.ShouldNotBeNull();
        list1[0].Address!.City.ShouldBe("HANGZHOU");

        var list2 = await ExpressionQueryService.GetListAsync<TestDocument>(
            TestDocumentIndexNames.Index,
            x => !string.IsNullOrWhiteSpace(x.Name) && x.Tags != null && x.Tags.Contains("B"));
        list2.Count.ShouldBe(2);
        list2[0].Name.ShouldBe("Name2");
        list2[0].Age.ShouldBe(10);
        list2[0].Status.ShouldBe(TestEnum.Inactive);
        list2[0].StringValueStatus.ShouldBe(TestEnum.Active);
        list2[0].IsActive.ShouldBeFalse();
        list2[0].Tags.ShouldNotBeEmpty();
        list2[0].Tags!.ShouldContain("B");
        list2[0].Address!.City.ShouldBe("GUANGZHOU");

        var list3 = await ExpressionQueryService.GetListAsync<TestDocument>(
            TestDocumentIndexNames.Index,
            x =>
                x.Name != null && x.Name.Contains("1") &&
                (x.Status == TestEnum.Active || x.Status == TestEnum.Pending));
        list3.Count.ShouldBe(2);
        list3[1].Name.ShouldBe("Test1");
        list3[1].Age.ShouldBe(30);
        list3[1].Status.ShouldBe(TestEnum.Pending);
        list3[1].StringValueStatus.ShouldBe(TestEnum.Inactive);
        list3[1].IsActive.ShouldBeTrue();
        list3[1].Tags.ShouldNotBeEmpty();
        list3[1].Tags!.ShouldContain("C");
        list3[1].Address!.City.ShouldBe("BEIJING");
    }
}
