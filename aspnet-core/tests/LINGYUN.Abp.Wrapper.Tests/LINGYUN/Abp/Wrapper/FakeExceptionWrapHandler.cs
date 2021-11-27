namespace LINGYUN.Abp.Wrapper
{
    public class FakeExceptionWrapHandler : IExceptionWrapHandler
    {
        public void Wrap(ExceptionWrapContext context)
        {
            context.WithCode("1001")
                   .WithMessage("自定义异常处理消息")
                   .WithDetails("自定义异常处理消息明细");
        }
    }
}
