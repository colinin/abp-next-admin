using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Rules.RulesEngine.FileProviders.Physical
{
    public class PhysicalFileWorkflowRulesContributor : FileProviderWorkflowRulesContributor, ISingletonDependency
    {
        private readonly RuleIdGenerator _ruleIdGenerator;
        private readonly AbpRulesEngineOptions _options;
        public PhysicalFileWorkflowRulesContributor(
            IMemoryCache ruleCache,
            RuleIdGenerator ruleIdGenerator,
            IJsonSerializer jsonSerializer,
            IOptions<AbpRulesEngineOptions> options)
            : base(ruleCache, jsonSerializer)
        {
            _ruleIdGenerator = ruleIdGenerator;

            _options = options.Value;
        }

        protected override IFileProvider BuildFileProvider()
        {
            // 未指定路径不启用
            if (!_options.PhysicalPath.IsNullOrWhiteSpace() &&
                Directory.Exists(_options.PhysicalPath))
            {
                return new PhysicalFileProvider(_options.PhysicalPath);
            }
            return null;
        }

        protected override int GetRuleId<T>() => _ruleIdGenerator.CreateRuleId(typeof(T), _options.IgnoreMultiTenancy);

        protected override string GetRuleName<T>() => $"{_ruleIdGenerator.CreateRuleName(typeof(T), _options.IgnoreMultiTenancy)}.json";
    }
}
