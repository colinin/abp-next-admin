using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using RulesEngine.Models;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Rules.RulesEngine.FileProviders;

public abstract class FileProviderWorkflowsResolveContributor : WorkflowsResolveContributorBase
{
    protected IMemoryCache RulesCache { get; private set; } = default!;
    protected IJsonSerializer JsonSerializer { get; private set; } = default!;
    protected IFileProvider? FileProvider { get; private set; }
    protected FileProviderWorkflowsResolveContributor()
    {
    }

    public override void Initialize(RulesInitializationContext context)
    {
        Initialize(context.ServiceProvider);

        RulesCache = context.GetRequiredService<IMemoryCache>();
        JsonSerializer = context.GetRequiredService<IJsonSerializer>();

        FileProvider = BuildFileProvider(context);
    }

    protected virtual void Initialize(IServiceProvider serviceProvider)
    {
    }

    protected abstract IFileProvider? BuildFileProvider(RulesInitializationContext context);

    public async override Task ResolveAsync(IWorkflowsResolveContext context)
    {
        if (FileProvider != null)
        {
            context.Workflows = await GetCachedRulesAsync(context.Type);
        }
        context.Handled = true;
    }

    public override void Shutdown()
    {
        if (FileProvider != null && FileProvider is IDisposable resource)
        {
            resource.Dispose();
        }
    }

    private async Task<Workflow[]> GetCachedRulesAsync(Type type, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ruleId = GetRuleId(type);

        var workflows = RulesCache.Get<Workflow[]>(ruleId);
        if (workflows == null)
        {
            workflows = await GetFileSystemRulesAsync(type, cancellationToken);

            RulesCache.Set(ruleId, workflows);
        }

        return workflows;
    }
    protected abstract int GetRuleId(Type type);

    protected abstract string GetRuleName(Type type);

    protected async virtual Task<Workflow[]> GetFileSystemRulesAsync(Type type, CancellationToken cancellationToken = default)
    {
        var ruleId = GetRuleId(type);
        var ruleFile = GetRuleName(type);
        var fileInfo = FileProvider?.GetFileInfo(ruleFile);
        if (fileInfo != null && fileInfo.Exists)
        {
            // 规则文件监控
            ChangeToken.OnChange(
                () => FileProvider!.Watch(ruleFile),
                (int ruleId) =>
                {
                    // 清除规则缓存
                    RulesCache.Remove(ruleId);
                }, ruleId);

            // 打开文本流
            using var stream = fileInfo.CreateReadStream();
            var result = new byte[stream.Length];
#if NETSTANDARD2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            await stream.ReadAsync(result, 0, result.Length, cancellationToken);
#else
            await stream.ReadExactlyAsync(result, 0, result.Length, cancellationToken);
#endif
            var ruleDsl = Encoding.UTF8.GetString(result);
            // 解析
            return JsonSerializer.Deserialize<Workflow[]>(ruleDsl);
        }
        return new Workflow[0];
    }
}
