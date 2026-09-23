using Moq.AutoMock;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;
using Volo.Abp.Specifications;
using Volo.Abp.Testing;
using Xunit;

namespace LINGYUN.Abp.AuditLogging;

public abstract class AuditLogManager_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IAuditLogWriter _writer;
    private readonly IAuditLogManager _manager;

    public AuditLogManager_Tests()
    {
        _writer = GetRequiredService<IAuditLogWriter>();
        _manager = GetRequiredService<IAuditLogManager>();
    }

    [Fact]
    public async Task Save_Audit_Log_Should_Be_Find_By_Id()
    {
        var mock = new AutoMocker();
        var auditLogInfo = mock.CreateInstance<AuditLogInfo>();

        var id = await _writer.WriteAsync(auditLogInfo);
        id.ShouldNotBeNullOrWhiteSpace();

        var findId = Guid.Parse(id);
        var auditLog = await _manager.GetAsync(findId);

        auditLog.ShouldNotBeNull();
        auditLog.Id.ShouldBe(findId);

        await _manager.DeleteAsync(findId);
    }

    [Fact]
    public async Task Save_Audit_Log_Should_Get_List()
    {
        var count = 10;
        await MockcAsync(count);

        // 延迟等待写入完成
        await Task.Delay(5000);

        // 异常应该只有3个
        (await _manager.GetCountAsync(
            hasException: true)).ShouldBe(3);

        // 正常可以查询7个
        (await _manager.GetCountAsync(
            hasException: false)).ShouldBe(7);

        // POST方法能查到5个
        (await _manager.GetCountAsync(
            httpMethod: "POST")).ShouldBe(5);

        (await _manager.GetCountAsync(
            startTime: DateTime.Now.AddDays(-1).AddHours(5))).ShouldBe(6);

        (await _manager.GetCountAsync(
            endTime: DateTime.Now.AddDays(-1))).ShouldBe(4);

        (await _manager.GetCountAsync(
            startTime: DateTime.Now.AddDays(-3).AddHours(1),
            endTime: DateTime.Now)).ShouldBe(8);

        // 索引5只存在一个
        (await _manager.GetCountAsync(
            userName: "_user_5",
            clientId: "_client_5")).ShouldBe(1);

        var logs = await _manager.GetListAsync(
            userName: "_user_5",
            clientId: "_client_5",
            maxResultCount: count);

        logs.Count.ShouldBe(1);
        logs[0].Url.ShouldBe("_url_5");
        logs[0].BrowserInfo.ShouldBe("_browser_5");
        logs[0].ApplicationName.ShouldBe("_app_5");

        await _manager.DeleteManyAsync(logs.Select(x => x.Id).ToList());
    }

    [Fact]
    public async Task Save_Audit_Log_Should_Get_List_With_Specification()
    {
        var count = 10;
        await MockcAsync(count);

        // 延迟等待写入完成
        await Task.Delay(5000);

        // 请求参数中包含 AAAAA 应该只有3个
        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.Actions.Any(a => a.Parameters.Contains("AAAAA"))))).ShouldBe(3);

        // 异常应该只有3个
        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.Exceptions != null))).ShouldBe(3);

        // 正常可以查询7个
        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.Exceptions == null))).ShouldBe(7);

        // POST方法能查到5个
        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.HttpMethod == "POST"))).ShouldBe(5);

        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.ExecutionTime >= DateTime.Now.AddDays(-1).AddHours(5)))).ShouldBe(6);

        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.ExecutionTime <= DateTime.Now.AddDays(-1)))).ShouldBe(4);

        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.ExecutionTime >= DateTime.Now.AddDays(-3).AddHours(1) &&
            x.ExecutionTime <= DateTime.Now))).ShouldBe(8);

        // 索引5只存在一个
        (await _manager.GetCountAsync(
            new ExpressionSpecification<AuditLog>(x => x.UserName == "_user_5" && x.ClientId == "_client_5"))).ShouldBe(1);

        var logs = await _manager.GetListAsync(
            new ExpressionSpecification<AuditLog>(x => x.UserName == "_user_5" && x.ClientId == "_client_5"),
            maxResultCount: count);

        logs.Count.ShouldBe(1);
        logs[0].Url.ShouldBe("_url_5");
        logs[0].BrowserInfo.ShouldBe("_browser_5");
        logs[0].ApplicationName.ShouldBe("_app_5");

        await _manager.DeleteManyAsync(logs.Select(x => x.Id).ToList());
    }

    protected async virtual Task<List<string>> MockcAsync(int count)
    {
        var mock = new AutoMocker();

        var auditLogIds = new List<string>();

        for (var i = 1; i <= count; i++)
        {
            var auditLogInfo = mock.CreateInstance<AuditLogInfo>();
            auditLogInfo.ClientId = $"_client_{i}";
            auditLogInfo.Url = $"_url_{i}";
            auditLogInfo.UserName = $"_user_{i}";
            auditLogInfo.ApplicationName = $"_app_{i}";
            auditLogInfo.BrowserInfo = $"_browser_{i}";
            auditLogInfo.ExecutionTime = DateTime.Now;
            if (i % 3 == 0)
            {
                auditLogInfo.Exceptions.Add(new Exception($"_exception_{i}"));
                auditLogInfo.Actions.Add(
                    new AuditLogActionInfo
                    {
                        ServiceName = $"_service_{i}",
                        MethodName = $"_method_{i}",
                        ExecutionTime = DateTime.Now,
                        ExecutionDuration = new Random().Next(1, 1000),
                        Parameters = JsonConvert.SerializeObject(new
                        {
                            Paramter = "AAAAA",
                        }),
                    });
            }

            if (i % 2 == 0)
            {
                auditLogInfo.HttpMethod = "POST";
            }

            if (i % 4 == 0)
            {
                auditLogInfo.ExecutionTime = DateTime.Now.AddDays(-3);
            }

            if (i % 5 == 0)
            {
                auditLogInfo.ExecutionTime = DateTime.Now.AddDays(-2);
            }

            auditLogIds.Add(await _writer.WriteAsync(auditLogInfo));
        }

        return auditLogIds;
    }
}
