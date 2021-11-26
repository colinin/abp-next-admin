using Microsoft.AspNetCore.Mvc.Filters;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wraping
{
    public interface IActionResultWrapper
    {
        void Wrap(FilterContext context);
    }
}
