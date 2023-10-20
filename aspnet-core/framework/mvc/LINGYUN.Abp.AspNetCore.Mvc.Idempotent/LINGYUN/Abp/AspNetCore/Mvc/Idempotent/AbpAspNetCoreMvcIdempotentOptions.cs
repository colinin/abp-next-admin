using System.Collections.Generic;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent;
public class AbpAspNetCoreMvcIdempotentOptions
{
    public List<string> SupportedMethods { get; }
    public AbpAspNetCoreMvcIdempotentOptions()
    {
        SupportedMethods = new List<string>
        {
            "POST",
            "PUT",
            "PATCH",
            // "DELETE"
        };
    }
}
