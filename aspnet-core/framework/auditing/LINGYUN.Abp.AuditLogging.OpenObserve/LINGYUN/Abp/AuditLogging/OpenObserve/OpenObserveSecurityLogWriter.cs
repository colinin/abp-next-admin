using LINGYUN.Abp.OpenObserve;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 安全日志 OpenObserve 写入器
/// </summary>
[Dependency(ReplaceServices = true)]
public class OpenObserveSecurityLogWriter : ISecurityLogWriter, ITransientDependency
{
    protected OpenObserveClient Client { get; }
    protected IClock Clock { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IOpenObserveStreamNameNormalizer StreamNameNormalizer { get; }
    protected AbpAuditLoggingOpenObserveOptions Options { get; }

    public ILogger<OpenObserveSecurityLogWriter> Logger { protected get; set; }

    public OpenObserveSecurityLogWriter(
        OpenObserveClient client,
        IClock clock,
        IGuidGenerator guidGenerator,
        IOpenObserveStreamNameNormalizer streamNameNormalizer,
        IOptions<AbpAuditLoggingOpenObserveOptions> options)
    {
        Client = client;
        Clock = clock;
        GuidGenerator = guidGenerator;
        StreamNameNormalizer = streamNameNormalizer;
        Options = options.Value;

        Logger = NullLogger<OpenObserveSecurityLogWriter>.Instance;
    }

    public async virtual Task WriteAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default)
    {
        var securityLog = CreateDocument(new SecurityLog(GuidGenerator.Create(), securityLogInfo));

        await Client.JsonRecordAsync(
            Options.Organization,
            GetSecurityLogStream(securityLog),
            securityLog,
            cancellationToken);
    }

    public async virtual Task BulkWriteAsync(IEnumerable<SecurityLogInfo> securityLogInfos, CancellationToken cancellationToken = default)
    {
        var securityLogs = new List<OpenObserveSecurityLog>();
        foreach (var securityLogInfo in securityLogInfos)
        {
            securityLogs.Add(CreateDocument(new SecurityLog(GuidGenerator.Create(), securityLogInfo)));
        }

        if (securityLogs.Count == 0)
        {
            return;
        }

        // 多租户: 按租户分组写入各自的数据流
        foreach (var group in securityLogs.GroupBy(securityLog => securityLog.TenantId))
        {
            await Client.JsonBulkAsync(
                Options.Organization,
                GetSecurityLogStream(group.Key),
                group,
                cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    /// 获取安全日志数据流名称(按日志所属租户)
    /// </summary>
    protected virtual string GetSecurityLogStream(SecurityLog securityLog)
    {
        return StreamNameNormalizer.NormalizeStream(Options.SecurityLogStream, securityLog.TenantId);
    }

    /// <summary>
    /// 获取安全日志数据流名称(按租户)
    /// </summary>
    protected virtual string GetSecurityLogStream(Guid? tenantId)
    {
        return StreamNameNormalizer.NormalizeStream(Options.SecurityLogStream, tenantId);
    }

    /// <summary>
    /// 创建安全日志文档(按 CreationTime 覆盖 _timestamp)
    /// </summary>
    protected virtual OpenObserveSecurityLog CreateDocument(SecurityLog securityLog)
    {
        var document = OpenObserveSecurityLog.From(securityLog);

        if (Options.UseLogTimeAsTimestamp)
        {
            document.Timestamp = OpenObserveTimestampHelper.TryGetTimestamp(
                securityLog.CreationTime,
                Clock.Now,
                Options.TimestampAllowedUptoHours);
        }

        return document;
    }
}
