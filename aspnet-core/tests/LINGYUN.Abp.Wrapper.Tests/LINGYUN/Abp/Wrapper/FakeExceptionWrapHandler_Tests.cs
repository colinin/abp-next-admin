using Shouldly;
using Volo.Abp;
using Volo.Abp.Http;
using Xunit;

namespace LINGYUN.Abp.Wrapper.Tests
{
    public class FakeExceptionWrapHandler_Tests: AbpWrapperTestsBase
    {
        private readonly IExceptionWrapHandlerFactory _exceptionWrapHandlerFactory;

        public FakeExceptionWrapHandler_Tests()
        {
            _exceptionWrapHandlerFactory = GetRequiredService<IExceptionWrapHandlerFactory>();
        }

        [Fact]
        public void Should_Return_Wraped_Result_With_Fake_Exception()
        {
            var exception = new FakeException();
            var exceptionWrapContext = new ExceptionWrapContext(
                exception, 
                new RemoteServiceErrorInfo(),
                ServiceProvider);

            var handler = _exceptionWrapHandlerFactory.CreateFor(exceptionWrapContext);
            handler.Wrap(exceptionWrapContext);

            exceptionWrapContext.ErrorInfo.Code.ShouldBe("1001");
            exceptionWrapContext.ErrorInfo.Message.ShouldBe("自定义异常处理消息");
            exceptionWrapContext.ErrorInfo.Details.ShouldBe("自定义异常处理消息明细");
        }

        [Fact]
        public void Should_Return_Wraped_Result_With_Default_Exception()
        {
            var exception = new AbpException();
            var errorInfo = new RemoteServiceErrorInfo(
                "默认异常处理消息",
                "默认异常处理消息明细",
                "1000");
            var exceptionWrapContext = new ExceptionWrapContext(
                exception,
                errorInfo,
                ServiceProvider);

            var handler = _exceptionWrapHandlerFactory.CreateFor(exceptionWrapContext);
            handler.Wrap(exceptionWrapContext);

            exceptionWrapContext.ErrorInfo.Code.ShouldBe("1000");
            exceptionWrapContext.ErrorInfo.Message.ShouldBe("默认异常处理消息");
            exceptionWrapContext.ErrorInfo.Details.ShouldBe("默认异常处理消息明细");
        }

        [Fact]
        public void Should_Return_Wraped_Result_Code_500_With_Unhandled_Exception()
        {
            var exception = new AbpException();
            var errorInfo = new RemoteServiceErrorInfo(
                "默认异常处理消息",
                "默认异常处理消息明细");
            var exceptionWrapContext = new ExceptionWrapContext(
                exception,
                errorInfo,
                ServiceProvider);

            var handler = _exceptionWrapHandlerFactory.CreateFor(exceptionWrapContext);
            handler.Wrap(exceptionWrapContext);

            exceptionWrapContext.ErrorInfo.Code.ShouldBe("500");
            exceptionWrapContext.ErrorInfo.Message.ShouldBe("默认异常处理消息");
            exceptionWrapContext.ErrorInfo.Details.ShouldBe("默认异常处理消息明细");
        }
    }
}
