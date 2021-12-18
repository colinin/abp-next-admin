using LINGYUN.Abp.Wrapper;
using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Localization;
using Xunit;

namespace LINGYUN.Abp.AspNetCore.Mvc.Results
{
    public class WrapResultController_Tests : AbpAspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Request_Headers()
        {
            RequestHeaders.Add(AbpHttpWrapConsts.AbpDontWrapResult, "true");

            var result = await GetResponseAsObjectAsync<TestResultObject>("/api/wrap-result-test/wrap");
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Wrap");
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Abp_Remote_Stream_Content()
        {
            var result = await GetResponseAsStringAsync("/api/wrap-result-test/get-text");
            result.ShouldNotBeNull();
            result.ShouldBe("text");
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Abp_Api_Definition()
        {
            var result = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Return_Application_Configuration_Dto()
        {
            var result = await GetResponseAsObjectAsync<ApplicationConfigurationDto>("/api/abp/application-configuration");

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Url_Prefix_With_Dont_Wrapper()
        {
            var result = await GetResponseAsObjectAsync<TestResultObject>("/api/dont/wrap-result-test");
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Not Wrap For Url Prefix");
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Interfaces_With_Dont_Wrapper()
        {
            var result = await GetResponseAsObjectAsync<TestResultObject>("/api/dont-base-type/wrap-result-test");
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Not Wrap For Base Type");
        }

        [Fact]
        public async Task Should_Return_Wrap_Result_For_Bussiness_Exception()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var result = await GetResponseAsObjectAsync<WrapResult<TestResultObject>>("/api/wrap-result-test/exception");
                result.ShouldNotBeNull();
                result.Code.ShouldBe("1001");
                result.Message.ShouldBe("测试包装后的异常消息.");
            }

            using (CultureHelper.Use("en"))
            {
                var result = await GetResponseAsObjectAsync<WrapResult<TestResultObject>>("/api/wrap-result-test/exception");
                result.ShouldNotBeNull();
                result.Code.ShouldBe("1001");
                result.Message.ShouldBe("Test the wrapped exception message.");
            }
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Has_Db_Exception()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/wrap-result-test/not-wrap-exception", System.Net.HttpStatusCode.InternalServerError);
                result.ShouldNotBeNull();
                result.Error.ShouldNotBeNull();
                result.Error.Message.ShouldBe("对不起,在处理你的请求期间,产生了一个服务器内部错误!");
            }
        }

        [Fact]
        public async Task Should_Return_Wrap_Result_For_Object_Return_Value()
        {
            var result = await GetResponseAsObjectAsync<WrapResult<TestResultObject>>("/api/wrap-result-test/wrap");
            result.ShouldNotBeNull();
            result.Result.Name.ShouldBe("Wrap");
        }

        [Fact]
        public async Task Should_Return_Wrap_Result_For_Empty_Object_Return_Value()
        {
            var result = await GetResponseAsObjectAsync<WrapResult<TestResultObject>>("/api/wrap-result-test/wrap-empty");
            result.Code.ShouldBe("0");
            result.Result.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Object_Return_Value()
        {
            var result = await GetResponseAsObjectAsync<TestResultObject>("/api/wrap-result-test/not-wrap");
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Not Wrap");
        }

        [Fact]
        public async Task Should_Return_Not_Wrap_Result_For_Response_204()
        {
            var result = await GetResponseAsObjectAsync<WrapResult>("/api/wrap-result-test/not-wrap-204", System.Net.HttpStatusCode.OK, method: HttpMethod.Put);
            result.ShouldNotBeNull();
            result.Code.ShouldBe("0");
            result.Result.ShouldBeNull();
        }
    }
}
