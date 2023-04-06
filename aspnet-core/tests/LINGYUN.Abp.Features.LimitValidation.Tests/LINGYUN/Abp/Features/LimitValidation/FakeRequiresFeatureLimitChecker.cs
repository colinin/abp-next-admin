using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Features.LimitValidation
{
    [Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    [ExposeServices(typeof(IRequiresLimitFeatureChecker))]
    public class FakeRequiresFeatureLimitChecker : IRequiresLimitFeatureChecker
    {
        private readonly IDictionary<string, LimitFeature> limitFeatures;

        public FakeRequiresFeatureLimitChecker()
        {
            limitFeatures = new Dictionary<string, LimitFeature>();
        }

        public virtual Task<bool> CheckAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            if (limitFeatures.ContainsKey(context.LimitFeature))
            {
                if (limitFeatures[context.LimitFeature].ExprieTime <= DateTime.Now)
                {
                    limitFeatures.Remove(context.LimitFeature);
                    return Task.FromResult(true);
                }
                return Task.FromResult(limitFeatures[context.LimitFeature].Limit + 1 <= context.Limit);
            }
            return Task.FromResult(true);
        }

        public Task ProcessAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            if (!limitFeatures.ContainsKey(context.LimitFeature))
            {
                limitFeatures.Add(context.LimitFeature, new LimitFeature(1, DateTime.Now.AddSeconds(context.GetEffectTicks(DateTime.Now))));
            }
            else
            {
                limitFeatures[context.LimitFeature].Invoke(1);
            }
            return Task.CompletedTask;
        }
    }

    public class LimitFeature
    {
        public int Limit { get; private set; }
        public DateTime ExprieTime { get; }
        public LimitFeature(int limit, DateTime exprieTime)
        {
            Limit = limit;
            ExprieTime = exprieTime;
        }

        public void Invoke(int count)
        {
            Limit += count;
        }
    }
}
