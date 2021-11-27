using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Wrapper
{
    public class ExceptionWrapHandlerFactory : IExceptionWrapHandlerFactory, ITransientDependency
    {
        private readonly AbpWrapperOptions _options;

        public ExceptionWrapHandlerFactory(
            IOptions<AbpWrapperOptions> options)
        {
            _options = options.Value;
        }

        public IExceptionWrapHandler CreateFor(ExceptionWrapContext context)
        {
            var exceptionType = context.Exception.GetType();
            var handler = _options.GetHandler(exceptionType);
            if (handler == null)
            {
                handler = new DefaultExceptionWrapHandler();
                _options.AddHandler(exceptionType, handler);
                return handler;
            }

            return handler;
        }
    }
}
