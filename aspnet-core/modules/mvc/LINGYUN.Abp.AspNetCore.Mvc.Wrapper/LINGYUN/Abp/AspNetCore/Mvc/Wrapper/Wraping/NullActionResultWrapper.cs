using Microsoft.AspNetCore.Mvc.Filters;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wraping
{
    public class NullActionResultWrapper : IActionResultWrapper
    {
        public void Wrap(FilterContext context)
        {

        }
    }
}
