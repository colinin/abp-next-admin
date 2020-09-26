using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Xunit;

namespace LINGYUN.Abp.Features.Validation.Redis
{
    public class RedisRequiresLimitFeatureCheckerTests : AbpFeaturesValidationRedisTestBase
    {
        protected IRequiresLimitFeatureChecker RequiresLimitFeatureChecker { get; }
        protected TestValidationFeatureClass TestValidationFeatureClass { get; }
        public RedisRequiresLimitFeatureCheckerTests()
        {
            RequiresLimitFeatureChecker = GetRequiredService<IRequiresLimitFeatureChecker>();
            TestValidationFeatureClass = GetRequiredService<TestValidationFeatureClass>();
        }

        [Fact]
        public virtual async Task Check_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestFeature1, LimitPolicy.Days, 10);
            await RequiresLimitFeatureChecker.CheckAsync(context);
        }

        [Fact]
        public virtual async Task Process_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestFeature1, LimitPolicy.Days, 10);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
        }

        [Fact]
        public virtual async Task Check_Limit_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestFeature1, LimitPolicy.Days, 5);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            try
            {
                await RequiresLimitFeatureChecker.CheckAsync(context);
            }
            catch(Exception ex)
            {
                ex.ShouldBeOfType<AbpAuthorizationException>();
            }
        }
    }
}
