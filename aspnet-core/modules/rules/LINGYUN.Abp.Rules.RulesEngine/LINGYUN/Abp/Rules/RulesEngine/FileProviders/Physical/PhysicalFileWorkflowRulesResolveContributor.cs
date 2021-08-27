using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine.FileProviders.Physical
{
    public class PhysicalFileWorkflowRulesResolveContributor : FileProviderWorkflowRulesResolveContributor, ISingletonDependency
    {
        public override string Name => "PhysicalFile";

        private RuleIdGenerator _ruleIdGenerator;
        private AbpRulesEngineOptions _rulesEngineOptions;
        private AbpRulesEnginePhysicalFileResolveOptions _fileResolveOptions;

        public PhysicalFileWorkflowRulesResolveContributor()
        {
        }

        protected override void Initialize(IServiceProvider serviceProvider)
        {
            _ruleIdGenerator = serviceProvider.GetRequiredService<RuleIdGenerator>();
            _rulesEngineOptions = serviceProvider.GetRequiredService<IOptions<AbpRulesEngineOptions>>().Value;
            _fileResolveOptions = serviceProvider.GetRequiredService<IOptions<AbpRulesEnginePhysicalFileResolveOptions>>().Value;
        }

        protected override IFileProvider BuildFileProvider(RulesInitializationContext context)
        {
            // 未指定路径不启用
            if (!_fileResolveOptions.PhysicalPath.IsNullOrWhiteSpace() &&
                Directory.Exists(_fileResolveOptions.PhysicalPath))
            {
                return new PhysicalFileProvider(_fileResolveOptions.PhysicalPath);
            }
            return null;
        }

        protected override int GetRuleId(Type type) => _ruleIdGenerator.CreateRuleId(type, _rulesEngineOptions.IgnoreMultiTenancy);

        protected override string GetRuleName(Type type) => $"{_ruleIdGenerator.CreateRuleName(type, _rulesEngineOptions.IgnoreMultiTenancy)}.json";
    }
}
