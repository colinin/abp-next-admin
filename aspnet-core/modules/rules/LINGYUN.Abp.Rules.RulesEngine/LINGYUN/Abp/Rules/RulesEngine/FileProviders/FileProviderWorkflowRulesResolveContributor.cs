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

namespace LINGYUN.Abp.Rules.RulesEngine.FileProviders
{
    public abstract class FileProviderWorkflowRulesResolveContributor : WorkflowRulesResolveContributorBase
    {
        protected IMemoryCache RulesCache { get; private set; }
        protected IJsonSerializer JsonSerializer { get; private set; }

        protected IFileProvider FileProvider { get; private set; }
        protected FileProviderWorkflowRulesResolveContributor()
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

        protected abstract IFileProvider BuildFileProvider(RulesInitializationContext context);

        public override async Task ResolveAsync(IWorkflowRulesResolveContext context)
        {
            if (FileProvider != null)
            {
                context.WorkflowRules = await GetCachedRulesAsync(context.Type);
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

        private async Task<WorkflowRules[]> GetCachedRulesAsync(Type type, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ruleId = GetRuleId(type);

            return await RulesCache.GetOrCreateAsync(ruleId,
                async (entry) =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                    return await GetFileSystemRulesAsync(type, cancellationToken);
                });
        }
        protected abstract int GetRuleId(Type type);

        protected abstract string GetRuleName(Type type);

        protected virtual async Task<WorkflowRules[]> GetFileSystemRulesAsync(Type type, CancellationToken cancellationToken = default)
        {
            var ruleId = GetRuleId(type);
            var ruleFile = GetRuleName(type);
            var fileInfo = FileProvider.GetFileInfo(ruleFile);
            if (fileInfo != null && fileInfo.Exists)
            {
                // 规则文件监控
                ChangeToken.OnChange(
                    () => FileProvider.Watch(ruleFile),
                    (int ruleId) =>
                    {
                        // 清除规则缓存
                        RulesCache.Remove(ruleId);
                    }, ruleId);

                // 打开文本流
                using (var stream = fileInfo.CreateReadStream())
                {
                    var result = new byte[stream.Length];
                    await stream.ReadAsync(result, 0, (int)stream.Length);
                    var ruleDsl = Encoding.UTF8.GetString(result);
                    // 解析
                    return JsonSerializer.Deserialize<WorkflowRules[]>(ruleDsl);
                }
            }
            return new WorkflowRules[0];
        }
    }
}
