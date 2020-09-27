using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Xunit;

namespace LINGYUN.Abp.Features.Validation.Redis
{
    public class RedisRequiresLimitFeatureCheckerTests : AbpFeaturesValidationRedisTestBase
    {
        protected AbpFeaturesValidationOptions Options { get; }
        protected IRequiresLimitFeatureChecker RequiresLimitFeatureChecker { get; }
        protected TestValidationFeatureClass TestValidationFeatureClass { get; }
        public RedisRequiresLimitFeatureCheckerTests()
        {
            Options = GetRequiredService<IOptions<AbpFeaturesValidationOptions>>().Value;
            RequiresLimitFeatureChecker = GetRequiredService<IRequiresLimitFeatureChecker>();
            TestValidationFeatureClass = GetRequiredService<TestValidationFeatureClass>();
        }

        [Fact]
        public virtual async Task Check_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestLimitFeature, Options, LimitPolicy.Minute);
            await RequiresLimitFeatureChecker.CheckAsync(context);
        }

        [Fact]
        public virtual async Task Process_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestLimitFeature, Options, LimitPolicy.Minute);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
        }

        [Fact]
        public virtual async Task Check_Limit_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestLimitFeature, Options, LimitPolicy.Minute, 1 ,5);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
            {
                await RequiresLimitFeatureChecker.CheckAsync(context);
            });
            Thread.Sleep(61000);
            // it's ok
            await RequiresLimitFeatureChecker.ProcessAsync(context);
        }
    }
}
