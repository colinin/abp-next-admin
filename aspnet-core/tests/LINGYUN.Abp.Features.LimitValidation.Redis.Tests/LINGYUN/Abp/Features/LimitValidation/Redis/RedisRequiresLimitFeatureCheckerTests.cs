using Microsoft.Extensions.Options;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Features.LimitValidation.Redis
{
    public class RedisRequiresLimitFeatureCheckerTests : AbpFeaturesLimitValidationRedisTestBase
    {
        protected AbpFeaturesLimitValidationOptions Options { get; }
        protected IRequiresLimitFeatureChecker RequiresLimitFeatureChecker { get; }
        protected TestValidationFeatureClass TestValidationFeatureClass { get; }
        public RedisRequiresLimitFeatureCheckerTests()
        {
            Options = GetRequiredService<IOptions<AbpFeaturesLimitValidationOptions>>().Value;
            RequiresLimitFeatureChecker = GetRequiredService<IRequiresLimitFeatureChecker>();
            TestValidationFeatureClass = GetRequiredService<TestValidationFeatureClass>();
        }

        [Fact]
        public async virtual Task Check_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestLimitFeature, Options, LimitPolicy.Minute);
            await RequiresLimitFeatureChecker.CheckAsync(context);
        }

        [Fact]
        public async virtual Task Process_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestLimitFeature, Options, LimitPolicy.Minute);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
        }

        [Fact]
        public async virtual Task Check_Limit_Test_Async()
        {
            var context = new RequiresLimitFeatureContext(TestFeatureNames.TestLimitFeature, Options, LimitPolicy.Minute, 1 ,5);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);
            await RequiresLimitFeatureChecker.ProcessAsync(context);

            var current = await RequiresLimitFeatureChecker.CheckAsync(context);
            current.ShouldBeFalse();

            Thread.Sleep(61000);
            // it's ok
            var nowCheckResult = await RequiresLimitFeatureChecker.CheckAsync(context);
            nowCheckResult.ShouldBeTrue();
        }
    }
}
