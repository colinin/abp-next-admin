using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class FeaturesLimitValidationTests : AbpFeaturesLimitValidationTestBase
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected TestValidationFeatureClass TestValidationFeatureClass { get; }
        public FeaturesLimitValidationTests()
        {
            CurrentTenant = GetRequiredService<ICurrentTenant>();
            TestValidationFeatureClass = GetRequiredService<TestValidationFeatureClass>();
        }

        [Theory]
        [InlineData(TestFeatureTenant.TenantId)] //Features were not enabled for Tenant 2
        public async Task Should_Not_Allow_To_Call_Method_If_Has_Limit_Feature_Async(string tenantId)
        {
            using (CurrentTenant.Change(ParseNullableGuid(tenantId)))
            {
                // it's ok
                await TestValidationFeatureClass.Test1MinuteAsync();
                await TestValidationFeatureClass.Test1MinuteAsync();
                await Assert.ThrowsAsync<AbpFeatureLimitException>(TestValidationFeatureClass.Test1MinuteAsync);

                Thread.Sleep(61000);
                await TestValidationFeatureClass.Test1MinuteAsync();
            }
        }

        private static Guid? ParseNullableGuid(string tenantIdValue)
        {
            return tenantIdValue == null ? (Guid?)null : new Guid(tenantIdValue);
        }
    }
}
