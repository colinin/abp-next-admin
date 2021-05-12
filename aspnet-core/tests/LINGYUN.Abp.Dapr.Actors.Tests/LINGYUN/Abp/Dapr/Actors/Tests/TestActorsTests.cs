using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Dapr.Actors.Tests
{
    public class TestAppServiceTests : AbpDaprActorsTestBase
    {
        private readonly ITestActor _actor;

        public TestAppServiceTests()
        {
            _actor = GetRequiredService<ITestActor>();
        }

        [Fact]
        public async Task Get_Result_Items_Count_Should_5()
        {
            var result = await _actor.GetAsync();

            result.Count.ShouldBe(5);
        }

        [Fact]
        public async Task Update_Result_Value_Should_Value_Updated_1()
        {
            var result = await _actor.UpdateAsync();

            result.Value.ShouldBe("value:updated:1");
        }

        public override void Dispose()
        {
            _ = _actor.ClearAsync();
            base.Dispose();
        }
    }
}
