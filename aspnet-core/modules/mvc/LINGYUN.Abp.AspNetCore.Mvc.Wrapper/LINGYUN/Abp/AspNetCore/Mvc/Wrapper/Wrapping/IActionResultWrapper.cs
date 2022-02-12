using Microsoft.AspNetCore.Mvc.Filters;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wrapping
{
    public interface IActionResultWrapper
    {
        void Wrap(FilterContext context);
    }
}
