using Microsoft.Extensions.Caching.Memory;
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
    public abstract class FileProviderWorkflowRulesContributor : IWorkflowRulesContributor
    {
        protected IMemoryCache RulesCache { get; }
        protected IJsonSerializer JsonSerializer { get; }

        protected IFileProvider FileProvider { get; private set; }

        protected FileProviderWorkflowRulesContributor(
            IMemoryCache ruleCache,
            IJsonSerializer jsonSerializer)
        {
            RulesCache = ruleCache;
            JsonSerializer = jsonSerializer;
        }

        public void Initialize()
        {
            FileProvider = BuildFileProvider();
        }

        protected abstract IFileProvider BuildFileProvider();

        public async Task<WorkflowRules[]> LoadAsync<T>(CancellationToken cancellationToken = default)
        {
            if (FileProvider != null)
            {
                return await GetCachedRulesAsync<T>(cancellationToken);
            }
            return new WorkflowRules[0];
        }

        public void Shutdown()
        {
            if (FileProvider != null && FileProvider is IDisposable resource)
            {
                resource.Dispose();
            }
        }

        private async Task<WorkflowRules[]> GetCachedRulesAsync<T>(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ruleId = GetRuleId<T>();

            return await RulesCache.GetOrCreateAsync(ruleId,
                async (entry) =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                    return await GetFileSystemRulesAsync<T>(cancellationToken);
                });
        }
        protected abstract int GetRuleId<T>();

        protected abstract string GetRuleName<T>();

        protected virtual async Task<WorkflowRules[]> GetFileSystemRulesAsync<T>(CancellationToken cancellationToken = default)
        {
            var ruleId = GetRuleId<T>();
            var ruleFile = GetRuleName<T>();
            var fileInfo = FileProvider.GetFileInfo(ruleFile);
            if (fileInfo != null && fileInfo.Exists)
            {
                // 规则文件监控
                // TODO: 删除模块的规则缓存还需要删除RulesEngine中rulesCache已编译的规则缓存
                ChangeToken.OnChange(
                    () => FileProvider.Watch(ruleFile),
                    (int ruleId) =>
                    {
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
