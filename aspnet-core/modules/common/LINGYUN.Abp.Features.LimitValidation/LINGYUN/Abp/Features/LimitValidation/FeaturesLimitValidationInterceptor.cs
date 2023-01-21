using Microsoft.Extensions.Options;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class FeaturesLimitValidationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IFeatureChecker _featureChecker;
        private readonly AbpFeaturesLimitValidationOptions _options;
        private readonly IRequiresLimitFeatureChecker _limitFeatureChecker;
        private readonly IFeatureDefinitionManager _featureDefinitionManager;

        public FeaturesLimitValidationInterceptor(
            IFeatureChecker featureChecker,
            IRequiresLimitFeatureChecker limitFeatureChecker,
            IFeatureDefinitionManager featureDefinitionManager,
            IOptions<AbpFeaturesLimitValidationOptions> options)
        {
            _options = options.Value;
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

           var limitFeature = await GetRequiresLimitFeature(invocation.Method);

            if (limitFeature == null)
            {
                await invocation.ProceedAsync();
                return;
            }

            // 获取功能限制上限
            var limit = await _featureChecker.GetAsync(limitFeature.LimitFeature, limitFeature.DefaultLimit);
            // 获取功能限制时长
            var interval = await _featureChecker.GetAsync(limitFeature.IntervalFeature, limitFeature.DefaultInterval);
            // 必要的上下文参数
            var limitFeatureContext = new RequiresLimitFeatureContext(limitFeature.LimitFeature, _options, limitFeature.Policy, interval, limit);
            // 检查次数限制
            await PreCheckFeatureAsync(limitFeatureContext);
            // 执行代理方法
            await invocation.ProceedAsync();
            // 调用次数递增
            // TODO: 使用Redis结合Lua脚本?
            await PostCheckFeatureAsync(limitFeatureContext);
        }

        protected async virtual Task PreCheckFeatureAsync(RequiresLimitFeatureContext context)
        {
            var allowed = await _limitFeatureChecker.CheckAsync(context);
            if (!allowed)
            {
                throw new AbpFeatureLimitException(context.LimitFeature, context.Limit);
            }
        }

        protected async virtual Task PostCheckFeatureAsync(RequiresLimitFeatureContext context)
        {
            await _limitFeatureChecker.ProcessAsync(context);
        }

        protected async virtual Task<RequiresLimitFeatureAttribute> GetRequiresLimitFeature(MethodInfo methodInfo)
        {
            var limitFeature = methodInfo.GetCustomAttribute<RequiresLimitFeatureAttribute>(false);
            if (limitFeature != null)
            {
                // 限制次数定义的不是范围参数,则不参与限制功能
                var featureLimitDefinition = await _featureDefinitionManager.GetOrNullAsync(limitFeature.LimitFeature);
                if (featureLimitDefinition == null ||
                    !typeof(NumericValueValidator).IsAssignableFrom(featureLimitDefinition.ValueType.Validator.GetType()))
                {
                    return null;
                }
                // 时长刻度定义的不是范围参数,则不参与限制功能
                var featureIntervalDefinition = await _featureDefinitionManager.GetOrNullAsync(limitFeature.IntervalFeature);
                if (featureIntervalDefinition == null ||
                    !typeof(NumericValueValidator).IsAssignableFrom(featureIntervalDefinition.ValueType.Validator.GetType()))
                {
                    return null;
                }
            }
            return limitFeature;
        }
    }
}
