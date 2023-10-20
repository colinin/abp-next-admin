using System;
using System.Linq;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.ExceptionHandling
{
    public class AbpExceptionHandlingOptions
    {
        public ITypeList<Exception> Handlers { get; }
        public AbpExceptionHandlingOptions()
        {
            Handlers = new TypeList<Exception>();
        }

        public bool HasNotifierError(Exception ex)
        {
            if (typeof(IHasNotifierErrorMessage).IsAssignableFrom(ex.GetType()))
            {
                return true;
            }
            return Handlers.Any(x => x.IsAssignableFrom(ex.GetType()));
        }
    }
}
