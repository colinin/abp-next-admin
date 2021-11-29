using OpenApi;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Xunit;

namespace LINGYUN.Abp.OpenApi
{
    public class FakeInvokeController_Tests : AbpOpenApiTestBase
    {
        private readonly IClientProxy _clientProxy;

        public FakeInvokeController_Tests()
        {
            _clientProxy = GetRequiredService<IClientProxy>();
        }

        [Fact]
        public async Task Should_Invoke_Controller_And_Result_With_Hello()
        {
            var result = await _clientProxy.RequestAsync<string>(
                Client,
                "/api/invoke",
                FakeAppKeyStore.AppDescriptor.AppKey,
                FakeAppKeyStore.AppDescriptor.AppSecret,
                HttpMethod.Get);

            result.Code.ShouldBe("0");
            result.Message.ShouldBe("OK");
            result.Result.ShouldBe("Hello");
            result.Details.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Invoke_Controller_With_Invalid_App_Key()
        {
            using (CultureHelper.Use("en"))
            {
                var appKey = Guid.NewGuid().ToString("N");
                var result = await _clientProxy.RequestAsync<string>(
                    Client,
                    "/api/invoke",
                    appKey,
                    Guid.NewGuid().ToString("N"),
                    HttpMethod.Get);

                result.Code.ShouldBe("9100");
                result.Message.ShouldBe($"Invalid appKey {appKey}.");
                result.Result.ShouldBeNull();
                result.Details.ShouldBeNull();
            }
        }

        [Fact]
        public async Task Should_Invoke_Controller_With_Invalid_Signature()
        {
            using (CultureHelper.Use("en"))
            {
                var result = await _clientProxy.RequestAsync<string>(
                    Client,
                    "/api/invoke",
                    FakeAppKeyStore.AppDescriptor.AppKey,
                    Guid.NewGuid().ToString("N"),
                    HttpMethod.Get);

                result.Code.ShouldBe("9110");
                result.Message.ShouldBe("Invalid sign.");
                result.Result.ShouldBeNull();
                result.Details.ShouldBeNull();
            }
        }
    }
}
