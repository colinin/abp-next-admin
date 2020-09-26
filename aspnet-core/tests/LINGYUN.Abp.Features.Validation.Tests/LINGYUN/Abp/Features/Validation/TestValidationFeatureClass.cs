using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Features.Validation
{
    public class TestValidationFeatureClass : ITransientDependency
    {
        [RequiresLimitFeature(TestFeatureNames.TestFeature1, LimitPolicy.Days, 1)]
        public virtual Task Test1DaysAsync()
        {
            Console.WriteLine("this limit 1 days feature");

            return Task.CompletedTask;
        }

        [RequiresLimitFeature(TestFeatureNames.TestFeature1, LimitPolicy.Month, 1)]
        public virtual Task Test1MonthsAsync()
        {
            Console.WriteLine("this limit 1 month feature");

            return Task.CompletedTask;
        }

        [RequiresLimitFeature(TestFeatureNames.TestFeature1, LimitPolicy.Weeks, 1)]
        public virtual Task Test1WeeksAsync()
        {
            Console.WriteLine("this limit 1 weeks feature");

            return Task.CompletedTask;
        }

        [RequiresLimitFeature(TestFeatureNames.TestFeature1, LimitPolicy.Hours, 1)]
        public virtual Task Test1HoursAsync()
        {
            Console.WriteLine("this limit 1 hours feature");

            return Task.CompletedTask;
        }

        [RequiresLimitFeature(TestFeatureNames.TestFeature1, LimitPolicy.Years, 1)]
        public virtual Task Test1YearsAsync()
        {
            Console.WriteLine("this limit 1 years feature");

            return Task.CompletedTask;
        }
    }
}
