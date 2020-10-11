using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class TestValidationFeatureClass : ITransientDependency
    {
        [RequiresLimitFeature(TestFeatureNames.TestLimitFeature, TestFeatureNames.TestIntervalFeature, LimitPolicy.Minute)]
        public virtual Task Test1MinuteAsync()
        {
            Console.WriteLine("this limit 1 minute feature");

            return Task.CompletedTask;
        }
    }
}
