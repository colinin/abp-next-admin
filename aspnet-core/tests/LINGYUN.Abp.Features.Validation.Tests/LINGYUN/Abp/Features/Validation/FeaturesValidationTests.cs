using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.Features.Validation
{
    public class FeaturesValidationTests : AbpFeaturesValidationTestBase
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected TestValidationFeatureClass TestValidationFeatureClass { get; }
        public FeaturesValidationTests()
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
                await TestValidationFeatureClass.Test1HoursAsync();
                await TestValidationFeatureClass.Test1HoursAsync();
                await TestValidationFeatureClass.Test1HoursAsync();

                await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
                {
                    await TestValidationFeatureClass.Test1HoursAsync();
                });
            }
        }

        private static Guid? ParseNullableGuid(string tenantIdValue)
        {
            return tenantIdValue == null ? (Guid?)null : new Guid(tenantIdValue);
        }
    }
}
