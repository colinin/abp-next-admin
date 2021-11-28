using LINGYUN.Abp.Dapr.ServiceInvocation;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Dapr.Client.Tests
{
    public class TestAppServiceTests : AbpDaptClientTestBase
    {
        private readonly ITestAppService _service;

        public TestAppServiceTests()
        {
            _service = GetRequiredService<ITestAppService>();
        }

        [Fact]
        public async Task Get_Result_Items_Count_Should_5()
        {
            var result = await _service.GetAsync();

            result.Items.Count.ShouldBe(5);
        }

        [Fact]
        public async Task Should_Get_Wraped_Object()
        {
            var result = await _service.GetWrapedAsync("Test");

            result.Name.ShouldBe("Test");
        }

        [Fact]
        public async Task Update_Result_Value_Should_Value_Updated_1()
        {
            var result = await _service.UpdateAsync();

            result.Value.ShouldBe("value:updated:1");
        }
    }
}
