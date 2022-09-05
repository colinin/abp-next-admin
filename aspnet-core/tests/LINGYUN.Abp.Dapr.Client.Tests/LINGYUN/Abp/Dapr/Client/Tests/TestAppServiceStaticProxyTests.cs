using LINGYUN.Abp.Dapr.ServiceInvocation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Dapr.Client.Tests
{
    public class TestAppServiceStaticProxyTests : AbpDaptClientTestBase
    {
        private readonly ITestAppService _service;

        public TestAppServiceStaticProxyTests()
        {
            _service = GetRequiredService<ITestAppService>();
        }

        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.AddStaticDaprClientProxies(
                typeof(AbpDaprTestModule).Assembly,
                "TestDapr");
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
            var result = await _service.UpdateAsync(1);

            result.Value.ShouldBe("value:updated:1");
        }
    }
}
