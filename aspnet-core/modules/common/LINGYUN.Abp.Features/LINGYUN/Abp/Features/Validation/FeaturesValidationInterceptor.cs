using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Features.Validation
{
    public class FeaturesValidationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IFeatureChecker _featureChecker;
        private readonly IRequiresLimitFeatureChecker _limitFeatureChecker;
        private readonly IFeatureDefinitionManager _featureDefinitionManager;

        public FeaturesValidationInterceptor(
            IFeatureChecker featureChecker,
            IRequiresLimitFeatureChecker limitFeatureChecker,
            IFeatureDefinitionManager featureDefinitionManager)
        {
            _featureChecker = featureChecker;
            _limitFeatureChecker = limitFeatureChecker;
            _featureDefinitionManager = featureDefinitionManager;

        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.FeatureChecking))
            {
                await invocation.ProceedAsync();
                return;
            }

           var limitFeature = GetRequiresLimitFeature(invocation.Method);

            if (limitFeature == null)
            {
                await invocation.ProceedAsync();
                return;
            }

            // 获取功能限制时长
            var limit = await _featureChecker.GetAsync(limitFeature.Feature, limitFeature.DefaultLimit);

            var limitFeatureContext = new RequiresLimitFeatureContext(limitFeature.Feature, limitFeature.Policy, limit);
            // 检查次数限制
            await PreCheckFeatureAsync(limitFeatureContext);
            // 执行代理方法
            await invocation.ProceedAsync();
            // 调用次数递增
            // TODO: 使用Redis结合Lua脚本?
            await PostCheckFeatureAsync(limitFeatureContext);
        }

        protected virtual async Task PreCheckFeatureAsync(RequiresLimitFeatureContext context)
        {
            await _limitFeatureChecker.CheckAsync(context);
        }

        protected virtual async Task PostCheckFeatureAsync(RequiresLimitFeatureContext context)
        {
            await _limitFeatureChecker.ProcessAsync(context);
        }

        protected virtual RequiresLimitFeatureAttribute GetRequiresLimitFeature(MethodInfo methodInfo)
        {
            var limitFeature = methodInfo.GetCustomAttribute<RequiresLimitFeatureAttribute>(false);
            if (limitFeature != null)
            {
                var featureDefinition = _featureDefinitionManager.GetOrNull(limitFeature.Feature);
                if (featureDefinition != null &&
                    typeof(NumericValueValidator).IsAssignableFrom(featureDefinition.ValueType.Validator.GetType()))
                {
                    return limitFeature;
                }
            }
            return null;
        }
    }
}
